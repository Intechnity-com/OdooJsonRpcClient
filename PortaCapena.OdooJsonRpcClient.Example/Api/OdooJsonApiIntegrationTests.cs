using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example
{
    /// <summary>
    /// The same client methods over the JSON API against Odoo 19 from docker/odoo19 (http://localhost:8070).
    /// </summary>
    public class OdooJsonApiIntegrationTests : RequestTestBase
    {
        #region Connection

        [Fact]
        public async Task Version_is_read_without_api_key()
        {
            var config = new OdooConfig(JsonApiTestConfig.ApiUrl, JsonApiTestConfig.DbName, "admin", "", protocol: OdooApiProtocol.JsonApi);

            var result = await new OdooClient(config).GetVersionAsync();

            result.Succeed.Should().BeTrue(result.Message);
            result.Value.ServerSerie.Should().Be("19.0");
            result.Value.ServerVersion.Should().StartWith("19.0");
        }

        [Fact]
        public async Task Login_returns_user_of_api_key()
        {
            var result = await new OdooClient(JsonApiTestConfig).LoginAsync();

            result.Succeed.Should().BeTrue(result.Message);
            result.Value.Should().Be(2);
        }

        [Fact]
        public async Task Wrong_api_key_returns_failed_result_with_odoo_error()
        {
            var config = new OdooConfig(JsonApiTestConfig.ApiUrl, JsonApiTestConfig.DbName, "admin", "wrong-key", protocol: OdooApiProtocol.JsonApi);

            var result = await new OdooClient(config).GetAsync<IntegrationPartnerOdooModel>();

            result.Failed.Should().BeTrue();
            result.Error.Code.Should().Be(401);
            result.Message.Should().Be("Invalid apikey");
            result.Error.Data.Name.Should().Be("werkzeug.exceptions.Unauthorized");
        }

        [Fact]
        public async Task Databases_are_listed()
        {
            var result = await new OdooClient(JsonApiTestConfig).GetDatabasesAsync();

            result.Succeed.Should().BeTrue(result.Message);
            result.Value.Should().Contain(JsonApiTestConfig.DbName);
        }

        #endregion

        #region Read

        [Fact]
        public async Task Query_with_filter_order_paging_and_fields()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(JsonApiTestConfig);

            var result = await repository.Query()
                .Where(x => x.Name, OdooOperator.CaseInsensitiveLike, "test partner")
                .OrderByDescending(x => x.Name)
                .Select(x => new { x.Name, x.CountryId })
                .Take(2)
                .ToListAsync();

            result.Succeed.Should().BeTrue(result.Message);
            result.Value.Select(x => x.Name).Should().Equal("Test Partner US", "Test Partner PL");
            result.Value.Should().OnlyContain(x => x.CountryId > 0);
        }

        [Fact]
        public async Task Related_field_filter_count_any_and_first()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(JsonApiTestConfig);

            var polish = await repository.Query().Where<IntegrationCountryOdooModel>(x => x.CountryId, c => c.Code, OdooOperator.EqualsTo, "PL").ToListAsync();
            var count = await repository.Query().Where(x => x.IsCompany, OdooOperator.EqualsTo, true).CountAsync();
            var any = await repository.Query().Where(x => x.Name, OdooOperator.EqualsTo, "not existing partner").AnyAsync();
            var first = await repository.Query().OrderBy(x => x.Id).FirstOrDefaultAsync();

            polish.Value.Should().Contain(x => x.Name == "Test Partner PL");
            count.Value.Should().BeGreaterThan(2);
            any.Value.Should().BeFalse();
            first.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Untyped_read_search_ids_and_read_by_ids()
        {
            var client = new OdooClient(JsonApiTestConfig);

            var dictionaries = await client.GetAsync("res.partner", new OdooQuery { Limit = 3, ReturnFields = { "name" } });
            var ids = await client.SearchIdsAsync("res.partner", OdooQuery<IntegrationPartnerOdooModel>.Create().Take(3));
            var read = await client.ReadAsync<IntegrationPartnerOdooModel>(ids.Value);

            dictionaries.Value.Should().HaveCount(3).And.OnlyContain(x => x.ContainsKey("name"));
            ids.Value.Should().HaveCount(3);
            read.Value.Select(x => x.Id).Should().Equal(ids.Value);
        }

        [Fact]
        public async Task Model_definition_metadata_access_and_defaults()
        {
            var client = new OdooClient(JsonApiTestConfig);

            var model = await client.GetModelAsync("res.partner");
            var attributes = await client.GetModelAsync("res.partner", new[] { "type", "string" });
            var metadata = await client.GetMetadataAsync("res.company", new long[] { 1 });
            var access = await client.CheckAccessRightsAsync("res.partner", OdooAccessOperation.Write);
            var defaults = await client.DefaultGetAsync("res.partner", new[] { "type", "active" });

            model.Value["name"].PropertyValueType.Should().Be(OdooValueTypeEnum.Char);
            attributes.Value["name"].Help.Should().BeNull();
            metadata.Value.Single()["xmlid"].Should().Be("base.main_company");
            access.Value.Should().BeTrue(access.Message);
            defaults.Value["type"].Should().Be("contact");
        }

        [Fact]
        public async Task Name_search_and_read_group()
        {
            var client = new OdooClient(JsonApiTestConfig);

            var names = await client.NameSearchAsync("res.partner", "test partner", limit: 10);
            var groups = await client.ReadGroupAsync("res.partner", OdooFilter.Create().EqualTo("is_company", true), new[] { "id:count" }, new[] { "country_id" });

            names.Value.Select(x => x["display_name"]).Should().Contain(new[] { "Test Partner PL", "Test Partner BE", "Test Partner US" });
            groups.Succeed.Should().BeTrue(groups.Message);
            groups.Value.Should().NotBeEmpty();
        }

        #endregion

        #region Write

        [Fact]
        public async Task Create_update_delete_with_repository()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(JsonApiTestConfig);
            var name = $"JsonApi {Guid.NewGuid():N}";

            var created = await repository.CreateAsync(OdooDictionaryModel.Create(() => new IntegrationPartnerOdooModel { Name = name, Email = "json2@example.com" }));
            created.Succeed.Should().BeTrue(created.Message);
            try
            {
                created.Value.Should().BePositive();

                var updated = await repository.UpdateAsync(OdooDictionaryModel.Create(() => new IntegrationPartnerOdooModel { Email = null }), created.Value);
                updated.Value.Should().BeTrue(updated.Message);

                var record = (await repository.Query().ById(created.Value).FirstAsync()).Value;
                record.Name.Should().Be(name);
                record.Email.Should().BeNull();
            }
            finally
            {
                (await repository.DeleteAsync(created.Value)).Value.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Create_many_and_delete_range()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(JsonApiTestConfig);
            var prefix = $"JsonApi batch {Guid.NewGuid():N}";

            var created = await repository.CreateAsync(Enumerable.Range(1, 3).Select(i => new OdooDictionaryModel { ["name"] = $"{prefix} {i}" }));

            created.Succeed.Should().BeTrue(created.Message);
            created.Value.Should().HaveCount(3);
            var records = (await repository.Query().ByIds(created.Value).ToListAsync()).Value;
            (await repository.DeleteRangeAsync(records)).Value.Should().BeTrue();
            (await repository.Query().ByIds(created.Value).CountAsync()).Value.Should().Be(0);
        }

        [Fact]
        public async Task Action_copy_and_name_create()
        {
            var client = new OdooClient(JsonApiTestConfig);
            var tag = await client.NameCreateAsync("res.partner.category", $"JsonApi tag {Guid.NewGuid():N}");
            tag.Succeed.Should().BeTrue(tag.Message);
            var tagId = (long)tag.Value["id"];
            long copyId = 0;
            try
            {
                var copy = await client.CopyAsync("res.partner.category", tagId);
                copy.Succeed.Should().BeTrue(copy.Message);
                copyId = copy.Value;

                var archive = await client.ActionAsync("res.partner.category", "action_archive", new[] { copyId });
                archive.Succeed.Should().BeTrue(archive.Message);

                var archived = await client.ReadAsync("res.partner.category", new[] { copyId }, new OdooQuery { ReturnFields = { "active" } });
                archived.Value.Single()["active"].Should().Be(false);
            }
            finally
            {
                await client.DeleteRangeAsync("res.partner.category", new[] { tagId, copyId }.Where(x => x > 0).ToArray());
            }
        }

        [Fact]
        public async Task Call_method_with_named_arguments()
        {
            var client = new OdooClient(JsonApiTestConfig);
            var partnerId = (await client.SearchIdsAsync("res.partner", OdooQuery<IntegrationPartnerOdooModel>.Create()
                .Where(x => x.Name, OdooOperator.EqualsTo, "Test Partner PL"))).Value.Single();
            var body = $"JsonApi note {Guid.NewGuid():N}";

            var posted = await client.CallMethodAsync("res.partner", "message_post", new object[] { new[] { partnerId } },
                new Dictionary<string, object> { ["body"] = body, ["message_type"] = "comment", ["subtype_xmlid"] = "mail.mt_note" });

            posted.Succeed.Should().BeTrue(posted.Message);
            var messageId = Convert.ToInt64(((Newtonsoft.Json.Linq.JArray)posted.Value)[0]);
            await client.DeleteAsync("mail.message", messageId);
        }

        [Fact]
        public async Task Positional_arguments_are_not_supported()
        {
            var client = new OdooClient(JsonApiTestConfig);

            var result = await client.ActionCollectionAsync("res.partner", "message_post", new object[] { new long[] { 1 }, "body as positional argument" });

            result.Failed.Should().BeTrue();
            result.Message.Should().Contain("positional arguments").And.Contain(nameof(OdooClient.CallMethodAsync));
        }

        [Fact]
        public async Task Odoo_error_is_returned_as_failed_result()
        {
            var result = await new OdooClient(JsonApiTestConfig).CreateAsync(new OdooDictionaryModel("res.partner") { ["email"] = "no-name@example.com" });

            result.Failed.Should().BeTrue();
            result.Error.Data.Should().NotBeNull();
            result.Error.Code.Should().BeGreaterOrEqualTo(400);
        }

        #endregion
    }
}
