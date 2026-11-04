using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    /// <summary>
    /// search / read / get_metadata / check_access_rights / default_get / fields_get(attributes) /
    /// read_group / name_search / name_create / copy - request shapes and answer mapping.
    /// Answers are copied from Odoo 15.
    /// </summary>
    public class OdooClientOrmMethodsTests
    {
        private readonly FakeOdooServer _server = new FakeOdooServer();

        [Fact]
        public async Task SearchIds_sends_search_with_domain_and_paging_but_no_fields()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new[] { 1, 9, 10 });
            var client = _server.CreateClient();
            var query = OdooQuery<TestPartnerOdooModel>.Create()
                .Where(x => x.IsCompany, OdooOperator.EqualsTo, true)
                .Select(x => new { x.Name })
                .OrderBy(x => x.Id).Skip(2).Take(3);

            var result = await client.SearchIdsAsync<TestPartnerOdooModel>(query);

            result.Value.Should().Equal(1, 9, 10);
            var call = _server.ObjectCalls.Single();
            call.Model.Should().Be("test.partner");
            call.ModelMethod.Should().Be("search");
            call.ModelArgs.ToString(Formatting.None).Should().Be("[[[\"is_company\",\"=\",true]]]");
            call.Kwargs.Properties().Select(x => x.Name).Should().BeEquivalentTo("offset", "limit", "order");
        }

        [Fact]
        public async Task SearchIds_without_query_searches_all()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new int[0]);
            var client = _server.CreateClient();

            var result = await client.SearchIdsAsync("res.partner");

            result.Value.Should().BeEmpty();
            _server.ObjectCalls.Single().ModelArgs.ToString(Formatting.None).Should().Be("[[]]");
            _server.ObjectCalls.Single().Kwargs.Should().BeNull();
        }

        [Fact]
        public async Task Read_sends_ids_and_fields_only()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new object[] { new { id = 1, name = "My Company", country_id = new object[] { 233, "United States" } } });
            var client = _server.CreateClient();
            var query = OdooQuery<TestPartnerOdooModel>.Create().Select(x => new { x.Name, x.CountryId }).Take(1);

            var typed = await client.ReadAsync<TestPartnerOdooModel>(new long[] { 1 }, query);
            var dictionaries = await client.ReadAsync("res.partner", new long[] { 1 });

            typed.Value.Single().CountryId.Should().Be(233);
            dictionaries.Value.Single()["name"].Should().Be("My Company");
            var typedCall = _server.ObjectCalls[0];
            typedCall.ModelMethod.Should().Be("read");
            typedCall.ModelArgs.ToString(Formatting.None).Should().Be("[[1]]");
            typedCall.Kwargs.Properties().Select(x => x.Name).Should().Equal("fields");
            _server.ObjectCalls[1].Kwargs.Should().BeNull("no fields means all fields");
        }

        [Fact]
        public async Task GetMetadata_returns_all_values_from_odoo()
        {
            // "xmlids" exists only in newer Odoo versions - dictionary keeps whatever the version returns
            _server.OnObjectCall = r => FakeOdooServer.Json(JObject.Parse(@"{""jsonrpc"":""2.0"",""id"":1,""result"":[
                {""id"":1,""create_uid"":false,""create_date"":""2026-11-04 12:51:40"",""write_uid"":[1,""OdooBot""],
                 ""write_date"":""2026-11-04 12:53:14"",""xmlid"":""base.main_company"",""xmlids"":[{""xmlid"":""base.main_company"",""noupdate"":true}],""noupdate"":true}]}"));
            var client = _server.CreateClient();

            var result = await client.GetMetadataAsync<TestPartnerOdooModel>(new long[] { 1 });

            var metadata = result.Value.Single();
            metadata["xmlid"].Should().Be("base.main_company");
            metadata["noupdate"].Should().Be(true);
            metadata.Should().ContainKeys("create_uid", "create_date", "write_uid", "write_date", "xmlids");
            _server.ObjectCalls.Single().ModelMethod.Should().Be("get_metadata");
            _server.ObjectCalls.Single().ModelArgs.ToString(Formatting.None).Should().Be("[[1]]");
        }

        [Theory]
        [InlineData(OdooAccessOperation.Read, "read")]
        [InlineData(OdooAccessOperation.Write, "write")]
        [InlineData(OdooAccessOperation.Create, "create")]
        [InlineData(OdooAccessOperation.Unlink, "unlink")]
        public async Task CheckAccessRights_asks_without_raising_exception(OdooAccessOperation operation, string expected)
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(false);
            var client = _server.CreateClient();

            var result = await client.CheckAccessRightsAsync<TestPartnerOdooModel>(operation);

            result.Succeed.Should().BeTrue();
            result.Value.Should().BeFalse();
            var call = _server.ObjectCalls.Single();
            call.ModelMethod.Should().Be("check_access_rights");
            call.ModelArgs.ToString(Formatting.None).Should().Be($"[\"{expected}\",false]");
        }

        [Fact]
        public async Task DefaultGet_returns_defaults_of_requested_fields()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new { type = "contact", active = true, lang = "en_US" });
            var client = _server.CreateClient(context: new OdooContext("pl_PL"));

            var result = await client.DefaultGetAsync("res.partner", new[] { "type", "active", "lang" });

            result.Value["type"].Should().Be("contact");
            result.Value["active"].Should().Be(true);
            var call = _server.ObjectCalls.Single();
            call.ModelMethod.Should().Be("default_get");
            call.ModelArgs.ToString(Formatting.None).Should().Be("[[\"type\",\"active\",\"lang\"]]");
            ((string)call.Kwargs["context"]["lang"]).Should().Be("pl_PL");
        }

        [Fact]
        public async Task GetModel_with_attributes_sends_them_as_named_argument()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new { name = new { type = "char", @string = "Name" } });
            var client = _server.CreateClient();

            var result = await client.GetModelAsync("res.partner", new[] { "type", "string" });

            result.Value["name"].PropertyValueType.Should().Be(OdooValueTypeEnum.Char);
            result.Value["name"].String.Should().Be("Name");
            var call = _server.ObjectCalls.Single();
            call.Method.Should().Be("execute_kw");
            call.ModelMethod.Should().Be("fields_get");
            call.Kwargs["attributes"].ToObject<string[]>().Should().Equal("type", "string");
        }

        [Fact]
        public async Task ReadGroup_sends_domain_fields_groupby_and_lazy()
        {
            _server.OnObjectCall = r => FakeOdooServer.Json(JObject.Parse(@"{""jsonrpc"":""2.0"",""id"":1,""result"":[
                {""partner_id_count"":2,""amount_total"":2145.0,""partner_id"":[10,""Deco Addict""],""__domain"":[[""partner_id"",""="",10]]}]}"));
            var client = _server.CreateClient();
            var filter = OdooFilter.Create().EqualTo("state", "sale");

            var result = await client.ReadGroupAsync("sale.order", filter, new[] { "amount_total:sum" }, new[] { "partner_id" }, lazy: false);

            var group = result.Value.Single();
            group["partner_id_count"].Should().Be(2L);
            group["amount_total"].Should().Be(2145.0);
            var call = _server.ObjectCalls.Single();
            call.ModelMethod.Should().Be("read_group");
            call.ModelArgs.ToString(Formatting.None).Should().Be("[[[\"state\",\"=\",\"sale\"]],[\"amount_total:sum\"],[\"partner_id\"]]");
            ((bool)call.Kwargs["lazy"]).Should().BeFalse();
        }

        [Fact]
        public async Task NameSearch_sends_positional_arguments_and_maps_pairs()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new object[] { new object[] { 48, "Test Partner BE" }, new object[] { 47, "Test Partner PL" } });
            var client = _server.CreateClient();

            var result = await client.NameSearchAsync("res.partner", "Test Partner", OdooFilter.Create().EqualTo("is_company", true), limit: 5);

            result.Value.Select(x => x["id"]).Should().Equal(48L, 47L);
            result.Value[1]["display_name"].Should().Be("Test Partner PL");
            result.Value.Should().OnlyContain(x => x.TableName == "res.partner");
            var call = _server.ObjectCalls.Single();
            call.ModelMethod.Should().Be("name_search");
            call.ModelArgs.ToString(Formatting.None).Should().Be("[\"Test Partner\",[[\"is_company\",\"=\",true]],\"ilike\",5]");
        }

        [Fact]
        public async Task NameCreate_returns_id_and_name()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new object[] { 8, "New Tag" });
            var client = _server.CreateClient();

            var result = await client.NameCreateAsync("res.partner.category", "New Tag");

            result.Value["id"].Should().Be(8L);
            result.Value["display_name"].Should().Be("New Tag");
            result.Value.TableName.Should().Be("res.partner.category");
            _server.ObjectCalls.Single().ModelArgs.ToString(Formatting.None).Should().Be("[\"New Tag\"]");
        }

        [Fact]
        public async Task Copy_returns_new_id_from_number_or_list()
        {
            var answers = new Queue<object>(new object[] { 9, new[] { 10 } });
            _server.OnObjectCall = r => FakeOdooServer.Result(answers.Dequeue());
            var client = _server.CreateClient();

            var fromNumber = await client.CopyAsync<TestPartnerOdooModel>(1);
            var fromList = await client.CopyAsync("res.partner", 1, OdooDictionaryModel.Create<TestPartnerOdooModel>(x => x.Name, "Copy"));

            fromNumber.Value.Should().Be(9);
            fromList.Value.Should().Be(10);
            _server.ObjectCalls[0].ModelArgs.ToString(Formatting.None).Should().Be("[[1]]");
            _server.ObjectCalls[1].ModelArgs.ToString(Formatting.None).Should().Be("[[1],{\"name\":\"Copy\"}]");
        }

        [Fact]
        public async Task Copy_with_unexpected_answer_returns_failed_result()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(false);
            var client = _server.CreateClient();

            var result = await client.CopyAsync("res.partner", 1);

            result.Failed.Should().BeTrue();
            result.Message.Should().Contain("copy");
        }

        [Fact]
        public async Task Odoo_error_is_returned_not_thrown()
        {
            _server.OnObjectCall = r => FakeOdooServer.Error("odoo.exceptions.UserError", "Cannot duplicate");
            var client = _server.CreateClient();

            var copy = await client.CopyAsync("res.partner", 1);
            var group = await client.ReadGroupAsync("sale.order", null, null, null);

            copy.Error.Data.Message.Should().Be("Cannot duplicate");
            group.Failed.Should().BeTrue();
        }

        [Fact]
        public void Call_request_merges_context_into_named_arguments()
        {
            var config = new OdooConfig("http://odoo.test", "db", "u", "p");
            var kwargs = new Dictionary<string, object> { ["limit"] = 5 };

            var withContext = JObject.Parse(JsonConvert.SerializeObject(OdooRequestModel.Call(config, 2, "res.partner", "search", new object[] { new object[0] }, kwargs, new OdooContext("pl_PL"))));
            var withoutAnything = OdooRequestModel.Call(config, 2, "res.partner", "check_access_rights", null);

            withContext["params"]["args"][6]["limit"].Value<int>().Should().Be(5);
            withContext["params"]["args"][6]["context"]["lang"].Value<string>().Should().Be("pl_PL");
            kwargs.Should().NotContainKey("context", "given dictionary is not modified");
            withoutAnything.Params.Args.Should().HaveCount(6, "empty positional args are sent, empty kwargs are omitted");
        }

        [Fact]
        public async Task GetDatabases_calls_db_service_without_login()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new[] { "db-test", "other" });
            var client = _server.CreateClient();

            var result = await client.GetDatabasesAsync();

            result.Value.Should().Equal("db-test", "other");
            _server.LoginCount.Should().Be(0);
            var call = _server.Requests.Single();
            call.Service.Should().Be("db");
            call.Method.Should().Be("list");
            call.Args.Should().BeEmpty();
        }

        [Fact]
        public async Task GetDatabases_when_listing_is_disabled_returns_failed_result()
        {
            _server.OnObjectCall = r => FakeOdooServer.Error(OdooExceptionName.AccessDenied, "Access Denied");
            var client = _server.CreateClient();

            var result = await client.GetDatabasesAsync();

            result.Failed.Should().BeTrue();
            result.Error.Data.Name.Should().Be(OdooExceptionName.AccessDenied);
            _server.LoginCount.Should().Be(0, "listing databases is not retried with login");
        }
    }
}
