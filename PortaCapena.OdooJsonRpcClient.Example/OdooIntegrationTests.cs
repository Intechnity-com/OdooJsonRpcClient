using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Shared;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example
{
    /// <summary>
    /// Runs against the local Odoo from docker-compose.yml (seeded by docker/odoo/seed.py).
    /// </summary>
    public class OdooIntegrationTests : RequestTestBase
    {
        private static string UniqueName(string prefix) => $"{prefix} {Guid.NewGuid():N}";

        #region Login / transport

        [Fact]
        public async Task Wrong_password_returns_failed_login()
        {
            var config = new OdooConfig(TestConfig.ApiUrl, TestConfig.DbName, TestConfig.UserName, "wrong-password");

            var result = await new OdooClient(config).GetAsync<IntegrationPartnerOdooModel>();

            result.Failed.Should().BeTrue();
            result.Message.Should().Contain("Login failed");
        }

        [Fact]
        public async Task Wrong_database_returns_failed_result()
        {
            var config = new OdooConfig(TestConfig.ApiUrl, "db-does-not-exist", TestConfig.UserName, TestConfig.Password);

            var result = await new OdooClient(config).LoginAsync();

            result.Failed.Should().BeTrue();
        }

        [Fact]
        public async Task Wrong_url_returns_failed_result_with_http_status()
        {
            var config = new OdooConfig(TestConfig.ApiUrl + "/not-existing-path", TestConfig.DbName, TestConfig.UserName, TestConfig.Password);

            var result = await new OdooClient(config).GetVersionAsync();

            result.Failed.Should().BeTrue();
            result.Error.Code.Should().Be(404);
        }

        [Fact]
        public async Task Very_small_timeout_returns_failed_result()
        {
            var config = new OdooConfig(TestConfig.ApiUrl, TestConfig.DbName, TestConfig.UserName, TestConfig.Password, TimeSpan.FromMilliseconds(1));

            var result = await new OdooClient(config).GetAsync("product.template");

            result.Failed.Should().BeTrue();
        }

        [Fact]
        public async Task Static_api_with_explicit_user_id_works()
        {
            var login = await OdooClient.LoginAsync(TestConfig);
            login.Succeed.Should().BeTrue();

            var result = await OdooClient.GetAsync<IntegrationPartnerOdooModel>(TestConfig, login.Value, OdooQuery<IntegrationPartnerOdooModel>.Create().Take(3));

            result.Succeed.Should().BeTrue();
            result.Value.Should().HaveCount(3);
        }

        [Fact]
        public async Task Version_is_returned()
        {
            var result = await new OdooClient(TestConfig).GetVersionAsync();

            result.Succeed.Should().BeTrue();
            result.Value.ServerSerie.Should().NotBeNullOrEmpty();
            result.Value.ProtocolVersion.Should().Be(1);
        }

        [Fact]
        public async Task Parallel_requests_succeed()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(TestConfig);

            var results = await Task.WhenAll(Enumerable.Range(0, 20).Select(_ => repository.Query().Take(5).ToListAsync()));

            results.Should().OnlyContain(x => x.Succeed, "all parallel requests should succeed: {0}", string.Join("; ", results.Where(x => x.Failed).Select(x => x.Message)));
        }

        #endregion

        #region Queries

        [Fact]
        public async Task First_does_not_limit_following_queries()
        {
            var query = new OdooRepository<IntegrationPartnerOdooModel>(TestConfig).Query();

            var first = await query.FirstOrDefaultAsync();
            var all = await query.ToListAsync();

            first.Value.Should().NotBeNull();
            all.Value.Length.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task Count_matches_list()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(TestConfig);

            var count = await repository.Query().Where(x => x.IsCompany, OdooOperator.EqualsTo, true).CountAsync();
            var list = await repository.Query().Where(x => x.IsCompany, OdooOperator.EqualsTo, true).Select(x => new { x.Id }).ToListAsync();

            count.Value.Should().Be(list.Value.Length).And.BeGreaterThan(0);
        }

        [Fact]
        public async Task Between_filter_is_accepted_by_odoo_and_inclusive()
        {
            var repository = new OdooRepository<IntegrationProductOdooModel>(TestConfig);
            var all = await repository.Query().ToListAsync();
            var prices = all.Value.Select(x => x.ListPrice).Where(x => x > 0).OrderBy(x => x).ToArray();
            var min = prices[prices.Length / 4];
            var max = prices[prices.Length * 3 / 4];

            var filter = OdooFilter<IntegrationProductOdooModel>.Create().Between(x => x.ListPrice, min, max);
            var result = await repository.Query().Where(filter).ToListAsync();

            result.Error.Should().BeNull();
            result.Value.Should().NotBeEmpty().And.OnlyContain(x => x.ListPrice >= min && x.ListPrice <= max);
            result.Value.Length.Should().Be(all.Value.Count(x => x.ListPrice >= min && x.ListPrice <= max));
        }

        [Fact]
        public async Task ChildOf_and_ParentOf_filters_are_accepted_by_odoo()
        {
            var repository = new OdooRepository<IntegrationProductCategoryOdooModel>(TestConfig);
            var child = (await repository.Query().Where(x => x.ParentId, OdooOperator.NotEqualsTo, false).FirstAsync()).Value;

            var children = await repository.Query().Where(OdooFilter<IntegrationProductCategoryOdooModel>.Create().ChildOf(x => x.Id, child.ParentId.Value)).ToListAsync();
            var parents = await repository.Query().Where(OdooFilter<IntegrationProductCategoryOdooModel>.Create().ParentOf(x => x.Id, child.Id)).ToListAsync();

            children.Error.Should().BeNull();
            children.Value.Select(x => x.Id).Should().Contain(new[] { child.Id, child.ParentId.Value });
            parents.Error.Should().BeNull();
            parents.Value.Select(x => x.Id).Should().Contain(new[] { child.Id, child.ParentId.Value });
        }

        [Fact]
        public async Task Filter_on_related_field_path()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(TestConfig);

            var result = await repository.Query()
                .Where<IntegrationCountryOdooModel>(x => x.CountryId, x => x.Code, OdooOperator.EqualsTo, "PL")
                .ToListAsync();

            result.Error.Should().BeNull();
            result.Value.Should().Contain(x => x.Name == "Test Partner PL");
        }

        [Fact]
        public async Task Or_filter_with_like()
        {
            var filter = OdooFilter<IntegrationPartnerOdooModel>.Create()
                .Or()
                .ILike(x => x.Name, "test partner pl")
                .ILike(x => x.Name, "test partner be");

            var result = await new OdooRepository<IntegrationPartnerOdooModel>(TestConfig).Query().Where(filter).ToListAsync();

            result.Value.Select(x => x.Name).Should().Contain(new[] { "Test Partner PL", "Test Partner BE" });
        }

        [Fact]
        public async Task Order_skip_take()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(TestConfig);

            var all = await repository.Query().OrderBy(x => x.Id).ToListAsync();
            var page = await repository.Query().OrderBy(x => x.Id).Skip(2).Take(3).ToListAsync();

            page.Value.Select(x => x.Id).Should().Equal(all.Value.Skip(2).Take(3).Select(x => x.Id));
        }

        [Fact]
        public async Task Select_returns_only_requested_fields()
        {
            var result = await new OdooClient(TestConfig).GetAsync("res.partner", OdooQuery<IntegrationPartnerOdooModel>.Create().Select(x => new { x.Name }).Take(1));

            result.Value.Single().Keys.Should().BeEquivalentTo("id", "name");
        }

        [Fact]
        public async Task Context_language_translates_values()
        {
            var repository = new OdooRepository<IntegrationProductOdooModel>(TestConfig);
            var product = (await repository.Query().Where(x => x.Name, OdooOperator.EqualsTo, "Acoustic Bloc Screens").FirstAsync()).Value;

            var dutch = await repository.Query().ById(product.Id).WithContext(new OdooContext("nl_NL")).FirstAsync();

            dutch.Value.Name.Should().Be("Akoestische blokschermen");
        }

        [Fact]
        public async Task Archived_records_are_returned_with_active_test_context()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(TestConfig);
            var id = (await repository.CreateAsync(new OdooDictionaryModel { ["name"] = UniqueName("Archived"), ["active"] = false })).Value;
            try
            {
                var withoutContext = await repository.Query().ById(id).FirstOrDefaultAsync();
                var withContext = await repository.Query().ById(id).WithContext("active_test", false).FirstOrDefaultAsync();

                withoutContext.Value.Should().BeNull();
                withContext.Value.Should().NotBeNull();
                withContext.Value.Active.Should().BeFalse();
            }
            finally
            {
                await repository.DeleteAsync(id);
            }
        }

        [Fact]
        public async Task Many2one_and_many2many_are_mapped()
        {
            var result = await new OdooRepository<IntegrationPartnerOdooModel>(TestConfig).Query()
                .Where(x => x.Name, OdooOperator.EqualsTo, "Test Partner PL")
                .FirstAsync();

            result.Value.CountryId.Should().BePositive();
            result.Value.CategoryId.Should().NotBeNull();
        }

        #endregion

        #region Create / update / delete

        [Fact]
        public async Task Crud_with_dictionary_model()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(TestConfig);
            var name = UniqueName("Crud");

            var created = await repository.CreateAsync(OdooDictionaryModel.Create(() => new IntegrationPartnerOdooModel { Name = name, Email = "crud@example.com", IsCompany = true }));
            created.Succeed.Should().BeTrue(created.Message);
            try
            {
                var read = await repository.Query().ById(created.Value).FirstAsync();
                read.Value.Name.Should().Be(name);
                read.Value.Email.Should().Be("crud@example.com");
                read.Value.IsCompany.Should().BeTrue();

                var updated = await repository.UpdateAsync(OdooDictionaryModel.Create(() => new IntegrationPartnerOdooModel { Email = null }), created.Value);
                updated.Value.Should().BeTrue();

                (await repository.Query().ById(created.Value).FirstAsync()).Value.Email.Should().BeNull();
            }
            finally
            {
                var deleted = await repository.DeleteAsync(created.Value);
                deleted.Value.Should().BeTrue();
            }

            (await repository.Query().ById(created.Value).AnyAsync()).Value.Should().BeFalse();
        }

        [Fact]
        public async Task Create_many_and_delete_range()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(TestConfig);
            var prefix = UniqueName("Batch");

            var created = await repository.CreateAsync(Enumerable.Range(1, 3).Select(i => new OdooDictionaryModel { ["name"] = $"{prefix} {i}" }));
            created.Succeed.Should().BeTrue(created.Message);
            created.Value.Should().HaveCount(3);

            var records = await repository.Query().ByIds(created.Value).ToListAsync();
            records.Value.Should().HaveCount(3);

            var deleted = await repository.DeleteRangeAsync(records.Value);
            deleted.Succeed.Should().BeTrue(deleted.Message + " / " + deleted.Error?.Data?.Message);
            deleted.Value.Should().BeTrue();
            (await repository.Query().ByIds(created.Value).CountAsync()).Value.Should().Be(0);
        }

        [Fact]
        public async Task Validation_error_is_returned_as_failed_result()
        {
            // res.partner.name is required for contacts without parent
            var result = await new OdooClient(TestConfig).CreateAsync(new OdooDictionaryModel("res.partner") { ["email"] = "no-name@example.com" });

            result.Failed.Should().BeTrue();
            result.Error.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task Unknown_model_is_returned_as_failed_result()
        {
            var result = await new OdooClient(TestConfig).GetAsync("model.does.not.exist");

            result.Failed.Should().BeTrue();
        }

        [Fact]
        public async Task Action_archives_record()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(TestConfig);
            var id = (await repository.CreateAsync(new OdooDictionaryModel { ["name"] = UniqueName("Action") })).Value;
            try
            {
                var action = await repository.ActionAsync("action_archive", new[] { id });
                action.Succeed.Should().BeTrue(action.Message);

                var record = await repository.Query().ById(id).WithContext("active_test", false).FirstAsync();
                record.Value.Active.Should().BeFalse();
            }
            finally
            {
                await repository.DeleteAsync(id);
            }
        }

        [Fact]
        public async Task Sale_order_can_be_created_and_confirmed()
        {
            var client = new OdooClient(TestConfig);
            var partner = (await new OdooRepository<IntegrationPartnerOdooModel>(TestConfig).Query().Where(x => x.Name, OdooOperator.EqualsTo, "Test Partner PL").FirstAsync()).Value;
            var product = (await client.GetAsync("product.product", OdooQuery<IntegrationProductOdooModel>.Create().Where(x => x.Name, OdooOperator.EqualsTo, "Test Consumable").Select("id").Take(1))).Value.Single();

            var orderId = await client.CreateAsync(new OdooDictionaryModel("sale.order")
            {
                ["partner_id"] = partner.Id,
                ["order_line"] = new object[] { new object[] { 0, 0, new { product_id = (long)product["id"], product_uom_qty = 2 } } }
            });
            orderId.Succeed.Should().BeTrue(orderId.Message);

            var confirm = await client.ActionAsync("sale.order", "action_confirm", new[] { orderId.Value });
            confirm.Succeed.Should().BeTrue(confirm.Message);

            var order = await client.GetAsync("sale.order", OdooQuery<IntegrationPartnerOdooModel>.Create().ById(orderId.Value).Select("state", "amount_total"));
            order.Value.Single()["state"].Should().Be("sale");
        }

        #endregion

        #region Model generation

        [Theory]
        [InlineData("res.partner")]
        [InlineData("product.template")]
        [InlineData("sale.order")]
        [InlineData("account.move")]
        [InlineData("stock.picking")]
        public async Task Dotnet_model_can_be_generated(string tableName)
        {
            var model = await new OdooClient(TestConfig).GetModelAsync(tableName);

            model.Succeed.Should().BeTrue(model.Message);
            var code = OdooModelMapper.GetDotNetModel(tableName, model.Value);
            code.Should().Contain($"[OdooTableName(\"{tableName}\")]");
        }

        [Fact]
        public async Task All_partner_fields_can_be_read_as_dictionary()
        {
            var result = await new OdooClient(TestConfig).GetAsync("res.partner", new OdooQuery { Limit = 20 });

            result.Succeed.Should().BeTrue(result.Message);
            result.Value.Should().HaveCount(20);
        }

        #endregion
    }

    [OdooTableName("res.partner")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationPartnerOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("is_company")]
        public bool IsCompany { get; set; }

        [JsonProperty("country_id")]
        public long? CountryId { get; set; }

        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }

        [JsonProperty("category_id")]
        public long[] CategoryId { get; set; }
    }

    [OdooTableName("res.country")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationCountryOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }

    [OdooTableName("product.template")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationProductOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("list_price")]
        public double ListPrice { get; set; }

        [JsonProperty("categ_id")]
        public long CategId { get; set; }
    }

    [OdooTableName("product.category")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationProductCategoryOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }
    }
}
