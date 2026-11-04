using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example.Rpc
{
    /// <summary>
    /// Unbalanced journal entry lines with check_move_validity context on Odoo 15 (docker/odoo15) with the JSON-RPC API (issue #92).
    /// </summary>
    public class JournalEntryIntegrationTestsJsonRpc : RequestTestBase
    {
        [Fact]
        public async Task Unbalanced_line_can_be_added_with_check_move_validity_context()
        {
            var moveRepository = new OdooRepository<IntegrationInvoiceOdooModel>(TestConfig);
            var lineRepository = new OdooRepository<IntegrationMoveLineOdooModel>(TestConfig);
            var invoice = await moveRepository.Query()
                .Where(x => x.MoveType, OdooOperator.EqualsTo, "out_invoice")
                .Select(x => new { x.Name, x.CompanyId })
                .FirstAsync();
            var invoiceLine = await lineRepository.Query()
                .Where(x => x.MoveId, OdooOperator.EqualsTo, invoice.Value.Id)
                .FirstAsync();
            var entryId = await moveRepository.CreateAsync(OdooDictionaryModel.Create(() => new IntegrationInvoiceOdooModel
            {
                MoveType = "entry",
                CompanyId = invoice.Value.CompanyId
            }));
            try
            {
                var line = OdooDictionaryModel.Create(() => new IntegrationMoveLineOdooModel
                {
                    MoveId = entryId.Value,
                    Name = "Adjustment",
                    AccountId = invoiceLine.Value.AccountId,
                    Debit = 0,
                    Credit = 2
                });

                var withoutContext = await lineRepository.CreateAsync(line);
                var withContext = await lineRepository.CreateAsync(line, new OdooContext { ["check_move_validity"] = false });

                entryId.Succeed.Should().BeTrue(entryId.Message);
                withoutContext.Failed.Should().BeTrue();
                withoutContext.Error.ToString().Should().Contain("balanced");
                withContext.Succeed.Should().BeTrue(withContext.Message);
            }
            finally
            {
                await moveRepository.DeleteAsync(entryId.Value);
            }
        }
    }
}
