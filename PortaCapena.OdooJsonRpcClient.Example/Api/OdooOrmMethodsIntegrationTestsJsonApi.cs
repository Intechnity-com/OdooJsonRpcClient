using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example.Api
{
    /// <summary>
    /// search / read / get_metadata / check_access_rights / default_get / fields_get(attributes) /
    /// read_group / name_search / name_create / copy against Odoo 19 (docker/odoo19) with the JSON API.
    /// </summary>
    public class OdooOrmMethodsIntegrationTestsJsonApi : RequestTestBase
    {
        [Fact]
        public async Task SearchIds_matches_count_and_paging()
        {
            var client = new OdooClient(JsonApiTestConfig);
            var query = OdooQuery<IntegrationPartnerOdooModel>.Create().Where(x => x.IsCompany, OdooOperator.EqualsTo, true);

            var ids = await client.SearchIdsAsync<IntegrationPartnerOdooModel>(query);
            var count = await client.GetCountAsync<IntegrationPartnerOdooModel>(query);
            var page = await client.SearchIdsAsync("res.partner", OdooQuery<IntegrationPartnerOdooModel>.Create()
                .Where(x => x.IsCompany, OdooOperator.EqualsTo, true).OrderBy(x => x.Id).Skip(1).Take(2));

            ids.Succeed.Should().BeTrue(ids.Message);
            ids.Value.Length.Should().Be((int)count.Value).And.BeGreaterThan(2);
            page.Value.Should().Equal(ids.Value.OrderBy(x => x).Skip(1).Take(2));
        }

        [Fact]
        public async Task Read_returns_records_with_requested_fields_in_order_of_ids()
        {
            var client = new OdooClient(JsonApiTestConfig);
            var ids = (await client.SearchIdsAsync("res.partner", OdooQuery<IntegrationPartnerOdooModel>.Create().Take(3))).Value.Reverse().ToArray();

            var typed = await client.ReadAsync<IntegrationPartnerOdooModel>(ids);
            var dictionaries = await client.ReadAsync("res.partner", ids, OdooQuery<IntegrationPartnerOdooModel>.Create().Select(x => new { x.Name }));

            typed.Succeed.Should().BeTrue(typed.Message);
            typed.Value.Select(x => x.Id).Should().Equal(ids);
            dictionaries.Value.Should().OnlyContain(x => x.Keys.OrderBy(k => k).SequenceEqual(new[] { "id", "name" }));
        }

        [Fact]
        public async Task Read_of_not_existing_records()
        {
            var client = new OdooClient(JsonApiTestConfig);
            var ids = new long[] { 999999999, 1 };

            var withFields = await client.ReadAsync("res.partner", ids, OdooQuery<IntegrationPartnerOdooModel>.Create().Select(x => new { x.Name }));
            var allFields = await client.ReadAsync("res.partner", ids);

            // Odoo skips missing records when fields are given, but fails when computing all fields
            withFields.Succeed.Should().BeTrue(withFields.Message);
            withFields.Value.Select(x => x["id"]).Should().Equal(1L);
            allFields.Failed.Should().BeTrue();
        }

        [Fact]
        public async Task GetMetadata_returns_xml_id()
        {
            var result = await new OdooClient(JsonApiTestConfig).GetMetadataAsync("res.company", new long[] { 1 });

            result.Succeed.Should().BeTrue(result.Message);
            var metadata = result.Value.Single();
            metadata["xmlid"].Should().Be("base.main_company");
            metadata.Should().ContainKeys("create_date", "write_date");
        }

        [Fact]
        public async Task CheckAccessRights_for_admin()
        {
            var client = new OdooClient(JsonApiTestConfig);

            var write = await client.CheckAccessRightsAsync<IntegrationPartnerOdooModel>(OdooAccessOperation.Write);
            var unlink = await client.CheckAccessRightsAsync("res.partner", OdooAccessOperation.Unlink);
            var unknownModel = await client.CheckAccessRightsAsync("model.does.not.exist", OdooAccessOperation.Read);

            write.Value.Should().BeTrue(write.Message);
            unlink.Value.Should().BeTrue(unlink.Message);
            unknownModel.Failed.Should().BeTrue();
        }

        [Fact]
        public async Task DefaultGet_returns_defaults()
        {
            var result = await new OdooClient(JsonApiTestConfig).DefaultGetAsync("res.partner", new[] { "type", "active" });

            result.Succeed.Should().BeTrue(result.Message);
            result.Value["type"].Should().Be("contact");
            result.Value["active"].Should().Be(true);
        }

        [Fact]
        public async Task GetModel_with_attributes_returns_only_them()
        {
            var result = await new OdooClient(JsonApiTestConfig).GetModelAsync("res.partner", new[] { "type", "string" });

            result.Succeed.Should().BeTrue(result.Message);
            result.Value["name"].PropertyValueType.Should().Be(OdooValueTypeEnum.Char);
            result.Value["name"].String.Should().NotBeNullOrEmpty();
            result.Value["name"].Help.Should().BeNull("help was not requested");
        }

        [Fact]
        public async Task ReadGroup_sums_amounts_per_customer()
        {
            var client = new OdooClient(JsonApiTestConfig);
            var filter = OdooFilter.Create().In("state", new object[] { "sale", "done" });

            var groups = await client.ReadGroupAsync("sale.order", filter, new[] { "amount_total:sum" }, new[] { "partner_id" });
            var orders = await client.GetAsync("sale.order", new OdooQuery { Filters = filter, ReturnFields = { "amount_total" } });

            groups.Succeed.Should().BeTrue(groups.Message);
            groups.Value.Should().NotBeEmpty().And.OnlyContain(x => x.ContainsKey("partner_id") && x.ContainsKey("partner_id_count"));
            var groupedTotal = groups.Value.Sum(x => Convert.ToDecimal(x["amount_total"]));
            var total = orders.Value.Sum(x => Convert.ToDecimal(x["amount_total"]));
            groupedTotal.Should().Be(total);
        }

        [Fact]
        public async Task NameSearch_finds_partners_by_part_of_name()
        {
            var result = await new OdooClient(JsonApiTestConfig).NameSearchAsync("res.partner", "test partner", limit: 10);

            result.Succeed.Should().BeTrue(result.Message);
            result.Value.Select(x => x["display_name"]).Should().Contain(new[] { "Test Partner PL", "Test Partner BE", "Test Partner US" });
            result.Value.Should().OnlyContain(x => (long)x["id"] > 0);
        }

        [Fact]
        public async Task NameCreate_creates_record_from_name()
        {
            var client = new OdooClient(JsonApiTestConfig);
            var name = $"Tag {Guid.NewGuid():N}";

            var created = await client.NameCreateAsync("res.partner.category", name);
            try
            {
                created.Succeed.Should().BeTrue(created.Message);
                created.Value["display_name"].Should().Be(name);
                (await client.ReadAsync("res.partner.category", new[] { (long)created.Value["id"] })).Value.Single()["name"].Should().Be(name);
            }
            finally
            {
                if (created.Succeed)
                    await client.DeleteAsync("res.partner.category", (long)created.Value["id"]);
            }
        }

        [Fact]
        public async Task Copy_duplicates_record_with_default_values()
        {
            var client = new OdooClient(JsonApiTestConfig);
            var source = (await client.SearchIdsAsync("res.partner", OdooQuery<IntegrationPartnerOdooModel>.Create()
                .Where(x => x.Name, OdooOperator.EqualsTo, "Test Partner PL"))).Value.Single();
            var name = $"Copy {Guid.NewGuid():N}";

            var copy = await client.CopyAsync<IntegrationPartnerOdooModel>(source, OdooDictionaryModel.Create(() => new IntegrationPartnerOdooModel { Name = name }));
            try
            {
                copy.Succeed.Should().BeTrue(copy.Message);
                copy.Value.Should().NotBe(source);
                var copied = (await client.ReadAsync<IntegrationPartnerOdooModel>(new[] { copy.Value })).Value.Single();
                copied.Name.Should().Be(name);
                copied.CountryId.Should().BePositive("other values are copied from the source");
            }
            finally
            {
                if (copy.Succeed)
                    await client.DeleteAsync("res.partner", copy.Value);
            }
        }

        [Fact]
        public async Task GetDatabases_lists_test_database()
        {
            var instance = await new OdooClient(JsonApiTestConfig).GetDatabasesAsync();
            var shared = await OdooClient.GetDatabasesAsync(JsonApiTestConfig);

            instance.Succeed.Should().BeTrue(instance.Message);
            instance.Value.Should().Contain(JsonApiTestConfig.DbName);
            shared.Value.Should().Equal(instance.Value);
        }

        [Fact]
        public async Task GetDatabases_does_not_need_valid_credentials()
        {
            var config = new OdooConfig(JsonApiTestConfig.ApiUrl, "not-existing-db", "nobody", "wrong", protocol: OdooApiProtocol.JsonApi);

            var result = await new OdooClient(config).GetDatabasesAsync();

            result.Succeed.Should().BeTrue(result.Message);
            result.Value.Should().Contain(JsonApiTestConfig.DbName);
        }

        [Fact]
        public async Task CallMethod_posts_note_with_named_arguments()
        {
            var client = new OdooClient(JsonApiTestConfig);
            var partnerId = (await client.SearchIdsAsync("res.partner", OdooQuery<IntegrationPartnerOdooModel>.Create()
                .Where(x => x.Name, OdooOperator.EqualsTo, "Test Partner PL"))).Value.Single();
            var body = $"Note {Guid.NewGuid():N}";
            var kwargs = new System.Collections.Generic.Dictionary<string, object> { ["body"] = body, ["message_type"] = "comment", ["subtype_xmlid"] = "mail.mt_note" };

            var posted = await client.CallMethodAsync("res.partner", "message_post", new object[] { new[] { partnerId } }, kwargs);
            var messageId = Convert.ToInt64(posted.Value);
            try
            {
                posted.Succeed.Should().BeTrue(posted.Message);
                var message = (await client.ReadAsync("mail.message", new[] { messageId }, new OdooQuery { ReturnFields = { "body", "model", "res_id" } })).Value.Single();
                ((string)message["body"]).Should().Contain(body);
                message["model"].Should().Be("res.partner");
                message["res_id"].Should().Be(partnerId);
            }
            finally
            {
                if (posted.Succeed)
                    await client.DeleteAsync("mail.message", messageId);
            }
        }

        [Fact]
        public async Task CallMethod_with_wrong_named_argument_returns_failed_result()
        {
            var client = new OdooClient(JsonApiTestConfig);

            var result = await client.CallMethodAsync("res.partner", "name_search", null, new System.Collections.Generic.Dictionary<string, object> { ["not_existing_argument"] = 1 });

            result.Failed.Should().BeTrue();
        }
    }
}
