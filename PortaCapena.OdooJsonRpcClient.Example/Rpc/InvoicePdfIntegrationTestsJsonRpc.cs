using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example.Rpc
{
    /// <summary>
    /// Invoice PDF from Odoo 15 (docker/odoo15) with the JSON-RPC API (issue #83).
    /// </summary>
    public class InvoicePdfIntegrationTestsJsonRpc : RequestTestBase
    {
        [Fact]
        public async Task Invoice_pdf_can_be_generated_from_mail_template()
        {
            var invoice = await new OdooRepository<IntegrationInvoiceOdooModel>(TestConfig).Query()
                .Where(x => x.MoveType, OdooOperator.EqualsTo, "out_invoice")
                .Where(x => x.State, OdooOperator.EqualsTo, "posted")
                .Select(x => new { x.Name })
                .FirstAsync();
            var template = await new OdooRepository<IntegrationModelDataOdooModel>(TestConfig).Query()
                .Where(x => x.Module, OdooOperator.EqualsTo, "account")
                .Where(x => x.Name, OdooOperator.EqualsTo, "email_template_edi_invoice")
                .FirstAsync();

            var email = await new OdooClient(TestConfig).CallMethodAsync("mail.template", "generate_email",
                new object[] { new[] { template.Value.ResId }, new[] { invoice.Value.Id }, new[] { "report_template", "attachment_ids" } });

            email.Succeed.Should().BeTrue(email.Message);
            var attachment = ((JObject)email.Value)[invoice.Value.Id.ToString()]["attachments"].Single();
            var pdf = Convert.FromBase64String(attachment[1].Value<string>());
            attachment[0].Value<string>().Should().EndWith(".pdf");
            Encoding.ASCII.GetString(pdf, 0, 5).Should().Be("%PDF-");
        }
    }
}
