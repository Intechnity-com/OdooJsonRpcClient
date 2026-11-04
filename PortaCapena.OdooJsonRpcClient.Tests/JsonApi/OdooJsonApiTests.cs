using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.JsonApi
{
    /// <summary>
    /// JSON API (Odoo 19+): requests are translated from the JSON-RPC calls of the client - url, headers, named arguments, answers and errors.
    /// </summary>
    public class OdooJsonApiTests
    {
        private readonly FakeOdooJsonApiServer _server = new FakeOdooJsonApiServer();

        [Fact]
        public void Default_protocol_is_json_rpc()
        {
            new OdooConfig("http://odoo.test", "db", "u", "p").Protocol.Should().Be(OdooApiProtocol.JsonRpc);
        }

        [Fact]
        public async Task Query_is_sent_as_search_read_with_api_key_database_and_named_arguments()
        {
            _server.OnModelCall = r => FakeOdooJsonApiServer.Value(new[] { new { id = 46, name = "Test Partner BE", country_id = new object[] { 20, "Belgium" } } });
            var repository = new OdooRepository<TestPartnerOdooModel>(_server.CreateClient());

            var result = await repository.Query()
                .Where(x => x.Name, OdooOperator.CaseInsensitiveLike, "test")
                .Select(x => new { x.Name, x.CountryId })
                .OrderBy(x => x.Name).Skip(1).Take(2)
                .WithContext("lang", "pl_PL")
                .ToListAsync();

            result.Value.Single().CountryId.Should().Be(20);
            var call = _server.ModelCalls.Single();
            call.Message.Method.Should().Be(HttpMethod.Post);
            call.Message.RequestUri.AbsolutePath.Should().Be("/json/2/test.partner/search_read");
            call.Message.Headers.Authorization.Scheme.Should().Be("bearer");
            call.Message.Headers.Authorization.Parameter.Should().Be(FakeOdooJsonApiServer.ApiKey);
            call.Message.Headers.GetValues("X-Odoo-Database").Should().Equal("db");
            call.Body["domain"].ToString(Formatting.None).Should().Be("[[\"name\",\"ilike\",\"test\"]]");
            call.Body["fields"].ToObject<string[]>().Should().BeEquivalentTo("name", "country_id");
            ((string)call.Body["order"]).Should().Be("name ASC");
            ((int)call.Body["offset"]).Should().Be(1);
            ((int)call.Body["limit"]).Should().Be(2);
            ((string)call.Body["context"]["lang"]).Should().Be("pl_PL");
        }

        [Fact]
        public async Task Login_uses_context_get_of_api_key_user()
        {
            var client = _server.CreateClient();

            var result = await client.LoginAsync();

            result.Value.Should().Be(FakeOdooJsonApiServer.UserId);
            _server.Requests.Single().Message.RequestUri.AbsolutePath.Should().Be("/json/2/res.users/context_get");
        }

        [Fact]
        public async Task Version_is_read_from_web_version()
        {
            var result = await _server.CreateClient().GetVersionAsync();

            result.Value.ServerSerie.Should().Be("19.0");
            result.Value.ServerVersion.Should().Be("19.0-20261102");
            _server.Requests.Single().Message.Method.Should().Be(HttpMethod.Get);
        }

        [Fact]
        public async Task Databases_are_listed_with_web_controller()
        {
            var result = await _server.CreateClient().GetDatabasesAsync();

            result.Value.Should().Equal("db", "other");
            _server.Requests.Single().Message.RequestUri.AbsolutePath.Should().Be("/web/database/list");
        }

        [Fact]
        public async Task Create_of_one_record_sends_list_and_returns_first_id()
        {
            _server.OnModelCall = r => FakeOdooJsonApiServer.Value(new[] { 42 });
            var client = _server.CreateClient();

            var result = await client.CreateAsync(new TestPartnerCreateModel { Name = "New" });

            result.Value.Should().Be(42);
            var call = _server.ModelCalls.Single();
            call.Method.Should().Be("create");
            call.Body["vals_list"].ToString(Formatting.None).Should().Be("[{\"name\":\"New\"}]");
        }

        [Fact]
        public async Task Create_of_many_records_returns_all_ids()
        {
            _server.OnModelCall = r => FakeOdooJsonApiServer.Value(new[] { 1, 2 });
            var client = _server.CreateClient();

            var result = await client.CreateAsync(new[] { new TestPartnerCreateModel { Name = "A" }, new TestPartnerCreateModel { Name = "B" } });

            result.Value.Should().Equal(1, 2);
            _server.ModelCalls.Single().Body["vals_list"].ToString(Formatting.None).Should().Be("[{\"name\":\"A\"},{\"name\":\"B\"}]");
        }

        [Fact]
        public async Task Record_methods_send_ids_and_named_arguments()
        {
            _server.OnModelCall = r => r.Method == "read" ? FakeOdooJsonApiServer.Value(new object[0]) : FakeOdooJsonApiServer.Value(true);
            var client = _server.CreateClient();

            await client.UpdateRangeAsync(OdooDictionaryModel.Create<TestPartnerOdooModel>(x => x.Name, "B"), new long[] { 3, 4 });
            await client.DeleteAsync("test.partner", 5);
            await client.ReadAsync("test.partner", new long[] { 6 }, new OdooQuery { ReturnFields = { "name" } });
            await client.ActionAsync("sale.order", "action_confirm", 15);
            await client.CopyAsync("test.partner", 7, OdooDictionaryModel.Create<TestPartnerOdooModel>(x => x.Name, "Copy"));

            _server.ModelCalls.Select(x => $"{x.Method} {x.Body.ToString(Formatting.None)}").Should().Equal(
                "write {\"ids\":[3,4],\"vals\":{\"name\":\"B\"}}",
                "unlink {\"ids\":[5]}",
                "read {\"fields\":[\"name\"],\"ids\":[6]}",
                "action_confirm {\"ids\":[15]}",
                "copy {\"ids\":[7],\"default\":{\"name\":\"Copy\"}}");
        }

        [Fact]
        public async Task Model_methods_send_positional_arguments_by_name()
        {
            _server.OnModelCall = r => FakeOdooJsonApiServer.Value(r.Method == "name_search" ? (object)new[] { new object[] { 1, "A" } } : true);
            var client = _server.CreateClient();

            await client.NameSearchAsync("res.partner", "abc", limit: 5);
            await client.CheckAccessRightsAsync("res.partner", OdooAccessOperation.Write);
            await client.GetModelAsync("res.partner", new[] { "type" });

            _server.ModelCalls.Select(x => $"{x.Method} {x.Body.ToString(Formatting.None)}").Should().Equal(
                "name_search {\"name\":\"abc\",\"domain\":[],\"operator\":\"ilike\",\"limit\":5}",
                "check_access_rights {\"operation\":\"write\",\"raise_exception\":false}",
                "fields_get {\"attributes\":[\"type\"]}");
        }

        [Fact]
        public async Task Call_method_sends_ids_and_named_arguments()
        {
            _server.OnModelCall = r => FakeOdooJsonApiServer.Value(new[] { 1085 });
            var client = _server.CreateClient();

            await client.CallMethodAsync("res.partner", "message_post", new object[] { new long[] { 1 } }, new Dictionary<string, object> { ["body"] = "Note" });

            var call = _server.ModelCalls.Single();
            call.Body.ToString(Formatting.None).Should().Be("{\"body\":\"Note\",\"ids\":[1]}");
        }

        [Fact]
        public async Task Positional_arguments_return_failed_result_without_request()
        {
            var client = _server.CreateClient();

            var collection = await client.ActionCollectionAsync("res.partner", "message_post", new object[] { new long[] { 1 }, "positional" });
            var call = await client.CallMethodAsync("res.partner", "message_post", new object[] { new long[] { 1 }, "positional" });

            collection.Failed.Should().BeTrue();
            collection.Message.Should().Contain("positional arguments").And.Contain(nameof(OdooClient.CallMethodAsync));
            call.Failed.Should().BeTrue();
            _server.ModelCalls.Should().BeEmpty();
        }

        [Fact]
        public async Task Odoo_error_is_returned_with_exception_data()
        {
            _server.OnModelCall = r => FakeOdooJsonApiServer.Error(HttpStatusCode.UnprocessableEntity, "odoo.exceptions.ValidationError", "Name is required");
            var client = _server.CreateClient();

            var result = await client.CreateAsync(new TestPartnerCreateModel());

            result.Failed.Should().BeTrue();
            result.Message.Should().Be("Name is required");
            result.Error.Code.Should().Be(422);
            result.Error.HttpStatus.Should().Be("422");
            result.Error.Data.Name.Should().Be("odoo.exceptions.ValidationError");
            result.Error.Data.Arguments.Should().Equal("Name is required", "422");
        }

        [Fact]
        public async Task Non_json_error_returns_http_status()
        {
            _server.OnModelCall = r => new HttpResponseMessage(HttpStatusCode.BadGateway) { Content = new StringContent("<html>Bad gateway</html>") };
            var client = _server.CreateClient();

            var result = await client.GetAsync<TestPartnerOdooModel>();

            result.Failed.Should().BeTrue();
            result.Error.Code.Should().Be(502);
            result.Message.Should().Contain("HTTP 502");
            result.Error.Data.Should().BeNull();
        }

        [Fact]
        public async Task Timeout_returns_failed_result()
        {
            _server.Delay = TimeSpan.FromSeconds(5);
            var client = _server.CreateClient(TimeSpan.FromMilliseconds(100));

            var result = await client.GetVersionAsync();

            result.Failed.Should().BeTrue();
            result.Message.Should().Contain("timed out");
        }

        [Fact]
        public async Task Named_argument_wins_over_the_same_positional_argument()
        {
            _server.OnModelCall = r => FakeOdooJsonApiServer.Value(new[] { 1 });
            var client = _server.CreateClient();

            await client.CallMethodAsync("res.partner", "search", new object[] { new object[0] },
                new Dictionary<string, object> { ["domain"] = new object[] { new object[] { "id", "=", 1 } } });

            var call = _server.ModelCalls.Single();
            call.Message.RequestUri.AbsolutePath.Should().Be("/json/2/res.partner/search");
            call.Body["domain"].ToString(Formatting.None).Should().Be("[[\"id\",\"=\",1]]");
        }
    }
}
