using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example.Api
{
    /// <summary>
    /// File upload to ir.attachment on Odoo 19 (docker/odoo19) with the JSON API (issue #118).
    /// </summary>
    public class AttachmentIntegrationTestsJsonApi : RequestTestBase
    {
        [Fact]
        public async Task File_can_be_uploaded_to_record_and_downloaded()
        {
            var invoice = await new OdooRepository<IntegrationInvoiceOdooModel>(JsonApiTestConfig).Query()
                .Where(x => x.MoveType, OdooOperator.EqualsTo, "out_invoice")
                .Select(x => new { x.Name })
                .FirstAsync();
            var repository = new OdooRepository<IntegrationAttachmentOdooModel>(JsonApiTestConfig);
            var file = Encoding.ASCII.GetBytes($"%PDF-1.4\n%{Guid.NewGuid():N}\n");

            var attachmentId = await repository.CreateAsync(OdooDictionaryModel.Create(() => new IntegrationAttachmentOdooModel
            {
                Name = "upload-test.pdf",
                Datas = Convert.ToBase64String(file),
                Mimetype = "application/pdf",
                ResModel = "account.move",
                ResId = invoice.Value.Id
            }));
            try
            {
                var attachment = await repository.Query().ById(attachmentId.Value).FirstAsync();

                attachmentId.Succeed.Should().BeTrue(attachmentId.Message);
                Convert.FromBase64String(attachment.Value.Datas).Should().Equal(file);
                attachment.Value.FileSize.Should().Be(file.Length);
                attachment.Value.StoreFname.Should().NotBeNullOrEmpty("the file is stored in the filestore, not in the database");
                attachment.Value.ResId.Should().Be(invoice.Value.Id);
            }
            finally
            {
                await repository.DeleteAsync(attachmentId.Value);
            }
        }
    }
}
