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
    /// Posting a draft invoice on Odoo 19 (docker/odoo19) with the JSON API (issue #73).
    /// </summary>
    public class InvoicePostIntegrationTestsJsonApi : RequestTestBase
    {
        [Fact]
        public async Task Draft_invoice_can_be_posted()
        {
            var partner = await new OdooRepository<IntegrationPartnerOdooModel>(JsonApiTestConfig).Query()
                .Where(x => x.Name, OdooOperator.EqualsTo, "Test Partner PL")
                .FirstAsync();
            var repository = new OdooRepository<IntegrationInvoiceOdooModel>(JsonApiTestConfig);
            var invoiceId = await repository.CreateAsync(OdooDictionaryModel.Create(() => new IntegrationInvoiceOdooModel
            {
                MoveType = "out_invoice",
                PartnerId = partner.Value.Id
            }).Add(x => x.InvoiceLineIds, OdooCommand.Create(new OdooDictionaryModel { { "name", "Test line" }, { "quantity", 1 }, { "price_unit", 10 } })));
            try
            {
                var draft = await repository.Query().ById(invoiceId.Value).Select(x => new { x.State }).FirstAsync();

                var post = await repository.ActionAsync("action_post", invoiceId.Value);
                var posted = await repository.Query().ById(invoiceId.Value).Select(x => new { x.State, x.Name }).FirstAsync();

                invoiceId.Succeed.Should().BeTrue(invoiceId.Message);
                draft.Value.State.Should().Be("draft");
                post.Succeed.Should().BeTrue(post.Message);
                posted.Value.State.Should().Be("posted");
                posted.Value.Name.Should().StartWith("INV/");
            }
            finally
            {
                await repository.ActionAsync("button_draft", invoiceId.Value);
                await repository.DeleteAsync(invoiceId.Value);
            }
        }
    }
}
