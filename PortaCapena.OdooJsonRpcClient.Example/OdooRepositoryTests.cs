using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example
{
    public class OdooRepositoryTests : RequestTestBase
    {
        [Fact]
        public async Task Can_get_all_products()
        {
            var repository = new OdooRepository<ProductProductOdooModel>(TestConfig);
            var products = await repository.Query().ToListAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().BeGreaterThan(0);
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Can_get_product_by_id()
        {
            var repository = new OdooRepository<ProductProductOdooModel>(TestConfig);
            var products = await repository.Query().ById(282).ToListAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Can_get_product_with_deep_where()
        {
            var repository = new OdooRepository<ProductProductOdooModel>(TestConfig);

            var products = await repository.Query()
                 .Where<ResCurrencyOdooModel>(x => x.CurrencyId, z => z.Name, OdooOperator.EqualsTo, "Euros")
                .ToListAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();
            products.Value.Should().NotBeEmpty();
        }

        [Fact(Skip = "Test for working on Odoo")]
        //   [Fact]
        public async Task Can_get_product_with_deep_secound_level_where()
        {
            var repository = new OdooRepository<ProductProductOdooModel>(TestConfig);

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
            var repository = new OdooRepository<ProductProductOdooModel>(TestConfig);
            var products = await repository.Query()
                .ById(1)
                .Select(x => new { x.Name })
                .FirstOrDefaultAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();
        }

        [Fact(Skip = "Test for working on Odoo")]
        //   [Fact]
        public async Task Can_get_product_with_selected_language_uning_query()
        {
            var repository = new OdooRepository<ProductProductOdooModel>(TestConfig);

            var context = new OdooContext
            {
                Language = "nl_BE",
            };

            var product = await repository.Query()
                .ById(282)
                .WithContext(context)
                .FirstOrDefaultAsync();

            product.Error.Should().BeNull();
            product.Value.Should().NotBeNull();
            product.Succeed.Should().BeTrue();
            product.Value.Name.Should().Contain("Dutch");

            var productWithoutLanguage = await repository.Query()
                .ById(282)
                .FirstOrDefaultAsync();

            productWithoutLanguage.Error.Should().BeNull();
            productWithoutLanguage.Value.Should().NotBeNull();
            productWithoutLanguage.Succeed.Should().BeTrue();
            productWithoutLanguage.Value.Name.Should().NotContain("Dutch");
        }

        [Fact(Skip = "Test for working on Odoo")]
        //   [Fact]
        public async Task Can_get_product_with_selected_language_using_repository_init()
        {
            var confing = new OdooConfig(TestConfig.ApiUrl, TestConfig.DbName, TestConfig.UserName, TestConfig.Password, "nl_BE");
            var repository = new OdooRepository<ProductProductOdooModel>(confing);

            var product = await repository.Query()
                .ById(23)
                .FirstOrDefaultAsync();

            product.Error.Should().BeNull();
            product.Value.Should().NotBeNull();
            product.Succeed.Should().BeTrue();
            product.Value.Name.Should().Contain("Dutch");

            var product2 = await repository.Query()
                .ById(23)
                .FirstOrDefaultAsync();

            product2.Error.Should().BeNull();
            product2.Value.Should().NotBeNull();
            product2.Succeed.Should().BeTrue();
            product2.Value.Name.Should().Contain("Dutch");

            var productWithoutLanguage = await repository.Query()
                .ById(282)
                .WithContext(new OdooContext())
                .FirstOrDefaultAsync();

            productWithoutLanguage.Error.Should().BeNull();
            productWithoutLanguage.Value.Should().NotBeNull();
            productWithoutLanguage.Succeed.Should().BeTrue();
            productWithoutLanguage.Value.Name.Should().NotContain("Dutch");

            var product3 = await repository.Query()
                .ById(282)
                .FirstOrDefaultAsync();

            product3.Error.Should().BeNull();
            product3.Value.Should().NotBeNull();
            product3.Succeed.Should().BeTrue();
            product3.Value.Name.Should().Contain("Dutch");
        }

        [Fact(Skip = "Test for working on Odoo")]
        //   [Fact]
        public async Task Can_get_product_with_selected_language_using_repository_prop()
        {
            var repository = new OdooRepository<ProductProductOdooModel>(TestConfig);
            repository.Config.Context.Language = "nl_BE";

            var product = await repository.Query()
                .ById(282)
                .FirstOrDefaultAsync();

            product.Error.Should().BeNull();
            product.Value.Should().NotBeNull();
            product.Succeed.Should().BeTrue();
            product.Value.Name.Should().Contain("Dutch");

            var product2 = await repository.Query()
                .ById(282)
                .FirstOrDefaultAsync();

            product2.Error.Should().BeNull();
            product2.Value.Should().NotBeNull();
            product2.Succeed.Should().BeTrue();
            product2.Value.Name.Should().Contain("Dutch");

            var productWithoutLanguage = await repository.Query()
                .ById(282)
                .WithContext(new OdooContext())
                .FirstOrDefaultAsync();

            productWithoutLanguage.Error.Should().BeNull();
            productWithoutLanguage.Value.Should().NotBeNull();
            productWithoutLanguage.Succeed.Should().BeTrue();
            productWithoutLanguage.Value.Name.Should().NotContain("Dutch");

            var product3 = await repository.Query()
                .ById(282)
                .FirstOrDefaultAsync();

            product3.Error.Should().BeNull();
            product3.Value.Should().NotBeNull();
            product3.Succeed.Should().BeTrue();
            product3.Value.Name.Should().Contain("Dutch");
        }



        [Fact]
        public async Task Can_get_AccountPaymentTermOdooModel_by_id()
        {
            var repository = new OdooRepository<ResPartnerOdooModel>(TestConfig);
            var context = new OdooContext() {ForceCompany = 3}; // 1 My Company (San Francisco), 2 PL Company, 3 My Company (Chicago) // default My Company (San Francisco)
            var products = await repository.Query().ById(14)
                .WithContext(context)
                .FirstOrDefaultAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();

            var dupa = products.Value.PropertyPaymentTermId;  // 7
        }

    }
}