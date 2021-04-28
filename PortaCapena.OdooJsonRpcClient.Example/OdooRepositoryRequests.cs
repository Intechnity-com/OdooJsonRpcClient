using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example
{
    public class OdooRepositoryRequests : TestBase
    {
        [Fact]
        public async Task Can_get_all_products()
        {
            var repository = new OdooRepository<AccountAccountTypeOdooModel>(Config);
            var products = await repository.Query().ToListAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().BeGreaterThan(0);
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Can_get_product_by_id()
        {
            var repository = new OdooRepository<ProductProductOdooDto>(Config);
            var products = await repository.Query().ById(66).FirstOrDefaultAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Can_get_product_with_deep_where()
        {
            var repository = new OdooRepository<ProductProductOdooDto>(Config);

            var products = await repository.Query()
                 .Where<ResCurrencyOdooModel>(x => x.CurrencyId, z => z.Name, OdooOperator.EqualsTo, "Euros")
                .ToListAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();
            products.Value.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Can_get_product_with_deep_secound_level_where()
        {
            var repository = new OdooRepository<ProductProductOdooDto>(Config);

            var products = await repository.Query()
                .Where<AccountAccountOdooModel, AccountAccountTypeOdooModel>(x => x.PropertyAccountIncomeId, x => x.UserTypeId, x => x.Type, OdooOperator.EqualsTo, TypeAccountAccountTypeOdooEnum.Regular)
                .ToListAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();
            products.Value.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Can_get_products_with_where_and_select()
        {
            var repository = new OdooRepository<ProductProductOdooDto>(Config);
            var products = await repository.Query()
                .ById(66)
                .Select(x => new { x.Name })
                .FirstOrDefaultAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Can_get_all_OdooVoucher()
        {
            var repository = new OdooRepository<CouponProgramOdooDto>(Config);

            var products = await repository.Query()
                .SelectSimplifiedModel()
                .FirstOrDefaultAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();
        }
    }
}