using System;
using System.Linq;
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
    /// Stock quantity of a product on Odoo 15 (docker/odoo15) with the JSON-RPC API (issue #79).
    /// </summary>
    public class StockIntegrationTestsJsonRpc : RequestTestBase
    {
        [Fact]
        public async Task Stock_quantity_can_be_set_and_read()
        {
            var productRepository = new OdooRepository<IntegrationStockProductOdooModel>(TestConfig);
            var quantRepository = new OdooRepository<IntegrationStockQuantOdooModel>(TestConfig);
            var product = await productRepository.Query()
                .Where(x => x.DefaultCode, OdooOperator.EqualsTo, "TEST-STORABLE")
                .Select(x => new { x.Name })
                .FirstOrDefaultAsync();
            var productId = product.Value?.Id ?? (await productRepository.CreateAsync(OdooDictionaryModel.Create(() => new IntegrationStockProductOdooModel
            {
                Name = "Test Storable Product",
                DefaultCode = "TEST-STORABLE",
                Type = "product"
            }))).Value;
            var stockLocation = await new OdooRepository<IntegrationModelDataOdooModel>(TestConfig).Query()
                .Where(x => x.Module, OdooOperator.EqualsTo, "stock")
                .Where(x => x.Name, OdooOperator.EqualsTo, "stock_location_stock")
                .FirstAsync();
            var quantity = new Random().Next(1, 1000);

            var quantId = await quantRepository.CreateAsync(OdooDictionaryModel.Create(() => new IntegrationStockQuantOdooModel
            {
                ProductId = productId,
                LocationId = stockLocation.Value.ResId,
                InventoryQuantity = quantity
            }), new OdooContext { ["inventory_mode"] = true });
            var apply = await quantRepository.ActionAsync("action_apply_inventory", quantId.Value);
            var quant = await quantRepository.Query().ById(quantId.Value).Select(x => new { x.Quantity }).FirstAsync();
            var quants = await quantRepository.Query()
                .Where(x => x.ProductId, OdooOperator.EqualsTo, productId)
                .Where(x => x.LocationId, OdooOperator.EqualsTo, stockLocation.Value.ResId)
                .Select(x => new { x.Quantity })
                .ToListAsync();
            var available = await productRepository.Query().ById(productId).Select(x => new { x.QtyAvailable }).FirstAsync();

            quantId.Succeed.Should().BeTrue(quantId.Message);
            apply.Succeed.Should().BeTrue(apply.Message);
            quant.Value.Quantity.Should().Be(quantity);
            available.Value.QtyAvailable.Should().Be(quants.Value.Sum(x => x.Quantity));
        }
    }
}
