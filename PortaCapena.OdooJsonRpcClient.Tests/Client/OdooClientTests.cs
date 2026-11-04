using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Shared.Models.Fake;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Client
{
    public class OdooClientTests
    {
        private readonly FakeOdooServer _server = new FakeOdooServer();

        #region Login

        [Fact]
        public async Task Logs_in_once_and_reuses_user_id()
        {
            var client = _server.CreateClient();

            await client.GetAsync<TestPartnerOdooModel>();
            await client.GetAsync<TestPartnerOdooModel>();
            await client.GetCountAsync<TestPartnerOdooModel>();

            _server.LoginCount.Should().Be(1);
            _server.ObjectCalls.Should().HaveCount(3).And.OnlyContain(x => x.Uid == FakeOdooServer.UserId);
        }

        [Fact]
        public async Task User_id_is_shared_between_clients_with_same_config_values()
        {
            var config = _server.CreateConfig();

            await new OdooClient(config, new HttpClient(_server)).GetAsync<TestPartnerOdooModel>();
            await new OdooClient(new OdooConfig(config.ApiUrl, config.DbName, config.UserName, config.Password), new HttpClient(_server)).GetAsync<TestPartnerOdooModel>();

            _server.LoginCount.Should().Be(1);
        }

        [Fact]
        public async Task User_id_is_not_shared_between_different_servers()
        {
            var calls = 0;
            _server.OnLogin = r => FakeOdooServer.Result(Interlocked.Increment(ref calls) == 1 ? 10 : 20);

            var first = _server.CreateClient();
            var second = _server.CreateClient();

            await first.GetAsync<TestPartnerOdooModel>();
            await second.GetAsync<TestPartnerOdooModel>();

            _server.ObjectCalls.Select(x => x.Uid).Should().Equal(10, 20);
        }

        [Fact]
        public async Task Login_request_contains_credentials()
        {
            var client = _server.CreateClient();

            var result = await client.LoginAsync();

            result.Succeed.Should().BeTrue();
            result.Value.Should().Be(FakeOdooServer.UserId);
            var login = _server.Requests.Single();
            login.Message.RequestUri.ToString().Should().Be(client.Config.ApiUrl + "/jsonrpc");
            login.Args.Select(x => x.ToString()).Should().Equal("db", "admin", "secret");
        }

        [Fact]
        public async Task Invalid_credentials_return_failed_result()
        {
            // odoo answers `false` instead of an error
            _server.OnLogin = r => FakeOdooServer.Result(false);
            var client = _server.CreateClient();

            var result = await client.GetAsync<TestPartnerOdooModel>();

            result.Failed.Should().BeTrue();
            result.Message.Should().Contain("Login failed").And.Contain("admin");
            _server.ObjectCalls.Should().BeEmpty();
        }

        [Fact]
        public async Task Failed_login_is_not_cached()
        {
            var attempt = 0;
            _server.OnLogin = r => Interlocked.Increment(ref attempt) == 1 ? FakeOdooServer.Result(false) : FakeOdooServer.Result(FakeOdooServer.UserId);
            var client = _server.CreateClient();

            (await client.GetAsync<TestPartnerOdooModel>()).Failed.Should().BeTrue();
            (await client.GetAsync<TestPartnerOdooModel>()).Succeed.Should().BeTrue();

            _server.LoginCount.Should().Be(2);
        }

        [Fact]
        public async Task Access_denied_logs_in_again_and_retries_once()
        {
            var calls = 0;
            _server.OnObjectCall = r => Interlocked.Increment(ref calls) == 1
                ? FakeOdooServer.Error(OdooExceptionName.AccessDenied)
                : FakeOdooServer.Result(new[] { new { id = 1, name = "A" } });
            var client = _server.CreateClient();

            var result = await client.GetAsync<TestPartnerOdooModel>();

            result.Succeed.Should().BeTrue();
            result.Value.Should().ContainSingle(x => x.Name == "A");
            _server.LoginCount.Should().Be(2);
            _server.ObjectCalls.Should().HaveCount(2);
        }

        [Fact]
        public async Task Access_denied_twice_returns_error_without_endless_retry()
        {
            _server.OnObjectCall = r => FakeOdooServer.Error(OdooExceptionName.AccessDenied, "denied");
            var client = _server.CreateClient();

            var result = await client.GetAsync<TestPartnerOdooModel>();

            result.Failed.Should().BeTrue();
            result.Error.Data.Name.Should().Be(OdooExceptionName.AccessDenied);
            _server.ObjectCalls.Should().HaveCount(2);
        }

        [Fact]
        public async Task Other_errors_are_not_retried()
        {
            _server.OnObjectCall = r => FakeOdooServer.Error("odoo.exceptions.ValidationError", "Name is required");
            var client = _server.CreateClient();

            var result = await client.CreateAsync(new TestPartnerCreateModel());

            result.Failed.Should().BeTrue();
            result.Error.Data.Message.Should().Be("Name is required");
            _server.ObjectCalls.Should().HaveCount(1);
            _server.LoginCount.Should().Be(1);
        }

        #endregion

        #region Transport

        [Fact]
        public async Task Non_json_http_error_returns_failed_result_with_status()
        {
            _server.OnLogin = r => FakeOdooServer.Raw("<html>Not Found</html>", HttpStatusCode.NotFound);
            var client = _server.CreateClient();

            var result = await client.LoginAsync();

            result.Failed.Should().BeTrue();
            result.Error.Code.Should().Be(404);
            result.Error.HttpStatus.Should().Be("404");
            result.Message.Should().Contain("HTTP 404");
        }

        [Fact]
        public async Task Json_error_with_http_error_status_is_deserialized()
        {
            _server.OnObjectCall = r => FakeOdooServer.Json(new { jsonrpc = "2.0", id = 1, error = new { code = 500, message = "boom" } }, HttpStatusCode.InternalServerError);
            var client = _server.CreateClient();

            var result = await client.GetAsync<TestPartnerOdooModel>();

            result.Failed.Should().BeTrue();
            result.Message.Should().Be("boom");
        }

        [Fact]
        public async Task Invalid_json_returns_failed_result_instead_of_throwing()
        {
            _server.OnObjectCall = r => FakeOdooServer.Raw("{ not json", HttpStatusCode.OK, "application/json");
            var client = _server.CreateClient();

            var result = await client.GetAsync<TestPartnerOdooModel>();

            result.Failed.Should().BeTrue();
            result.Message.Should().Contain("JsonReaderException");
        }

        [Fact]
        public async Task Timeout_from_config_returns_failed_result()
        {
            _server.Delay = TimeSpan.FromSeconds(5);
            var client = _server.CreateClient(timeout: TimeSpan.FromMilliseconds(100));

            var result = await client.GetVersionAsync();

            result.Failed.Should().BeTrue();
            result.Message.Should().Contain("timed out");
        }

        [Fact]
        public async Task Different_clients_can_have_different_timeouts()
        {
            // 1.0.x mutated a shared HttpClient here, which threw once any request was sent
            _server.Delay = TimeSpan.FromMilliseconds(300);
            var slow = _server.CreateClient(timeout: TimeSpan.FromSeconds(10));
            var fast = _server.CreateClient(timeout: TimeSpan.FromMilliseconds(50));

            var slowResult = await slow.GetVersionAsync();
            var fastResult = await fast.GetVersionAsync();

            slowResult.Succeed.Should().BeTrue();
            fastResult.Failed.Should().BeTrue();
        }

        [Fact]
        public async Task Cancellation_returns_failed_result()
        {
            _server.Delay = TimeSpan.FromSeconds(5);
            var client = _server.CreateClient();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));

            var result = await client.GetVersionAsync(cts.Token);

            result.Failed.Should().BeTrue();
            result.Message.Should().NotContain("timed out");
        }

        [Fact]
        public async Task Cancellation_token_is_passed_to_login()
        {
            _server.Delay = TimeSpan.FromSeconds(5);
            var client = _server.CreateClient();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));

            var started = DateTime.UtcNow;
            var result = await client.GetAsync<TestPartnerOdooModel>(cancellationToken: cts.Token);

            result.Failed.Should().BeTrue();
            (DateTime.UtcNow - started).Should().BeLessThan(TimeSpan.FromSeconds(4));
        }

        [Fact]
        public async Task Basic_authentication_header_is_sent_when_configured()
        {
            var client = _server.CreateClient();
            try
            {
                OdooClient.BasicAuthenticationUsernamePassword = "user:pass";
                await client.GetVersionAsync();
            }
            finally
            {
                OdooClient.BasicAuthenticationUsernamePassword = null;
            }

            var auth = _server.Requests.Single().Message.Headers.Authorization;
            auth.Scheme.Should().Be("Basic");
            auth.Parameter.Should().Be(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("user:pass")));
        }

        [Fact]
        public async Task Request_ids_are_unique()
        {
            var client = _server.CreateClient();

            await Task.WhenAll(Enumerable.Range(0, 20).Select(_ => client.GetVersionAsync()));

            _server.Requests.Select(x => (int)x.Body["id"]).Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Constructor_requires_config()
        {
            Action act = () => new OdooClient(null);
            act.Should().Throw<ArgumentNullException>();
        }

        #endregion

        #region Requests

        [Fact]
        public async Task Get_sends_search_read_with_query_and_context()
        {
            var client = _server.CreateClient(context: new OdooContext("pl_PL"));
            var query = OdooQuery<TestPartnerOdooModel>.Create()
                .Where(x => x.Name, OdooOperator.CaseInsensitiveLike, "abc")
                .Select(x => new { x.Name, x.CountryId })
                .OrderByDescending(x => x.Name)
                .Skip(10)
                .Take(5);

            await client.GetAsync<TestPartnerOdooModel>(query);

            var call = _server.ObjectCalls.Single();
            call.Model.Should().Be("test.partner");
            call.ModelMethod.Should().Be("search_read");
            call.ModelArgs.ToString(Newtonsoft.Json.Formatting.None).Should().Be("[[[\"name\",\"ilike\",\"abc\"]]]");
            call.Kwargs["fields"].ToObject<string[]>().Should().BeEquivalentTo("name", "country_id");
            ((string)call.Kwargs["order"]).Should().Be("name DESC");
            ((int)call.Kwargs["offset"]).Should().Be(10);
            ((int)call.Kwargs["limit"]).Should().Be(5);
            ((string)call.Kwargs["context"]["lang"]).Should().Be("pl_PL");
        }

        [Fact]
        public async Task Context_parameter_overrides_config_context()
        {
            var client = _server.CreateClient(context: new OdooContext("pl_PL"));

            await client.GetAsync<TestPartnerOdooModel>(context: new OdooContext("nl_NL"));

            ((string)_server.ObjectCalls.Single().Kwargs["context"]["lang"]).Should().Be("nl_NL");
        }

        [Fact]
        public async Task Get_without_query_sends_empty_domain()
        {
            var client = _server.CreateClient();

            await client.GetAsync("res.partner");

            var call = _server.ObjectCalls.Single();
            call.Model.Should().Be("res.partner");
            call.ModelArgs.ToString(Newtonsoft.Json.Formatting.None).Should().Be("[[]]");
            call.Kwargs.Should().BeNull();
        }

        [Fact]
        public async Task Dates_are_sent_in_odoo_format()
        {
            var client = _server.CreateClient();
            var query = OdooQuery<TestPartnerOdooModel>.Create().Where(x => x.CreateDate, OdooOperator.GreaterThan, new DateTime(2024, 1, 2, 3, 4, 5));

            await client.GetAsync<TestPartnerOdooModel>(query);

            ((string)_server.ObjectCalls.Single().ModelArgs[0][0][2]).Should().Be("2024-01-02 03:04:05");
        }

        [Fact]
        public async Task Create_sends_model_values()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(42);
            var client = _server.CreateClient();

            var result = await client.CreateAsync(new TestPartnerCreateModel { Name = "New", Type = TestPartnerTypeOdooEnum.InvoiceAddress });

            result.Succeed.Should().BeTrue();
            result.Value.Should().Be(42);
            var call = _server.ObjectCalls.Single();
            call.ModelMethod.Should().Be("create");
            call.ModelArgs.ToString(Newtonsoft.Json.Formatting.None).Should().Be("[{\"name\":\"New\",\"type\":\"invoice\"}]");
        }

        [Fact]
        public async Task Create_many_sends_list_and_returns_ids()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new[] { 1, 2 });
            var client = _server.CreateClient();

            var result = await client.CreateAsync(new[] { new TestPartnerCreateModel { Name = "A" }, new TestPartnerCreateModel { Name = "B" } });

            result.Value.Should().Equal(1, 2);
            _server.ObjectCalls.Single().ModelArgs.ToString(Newtonsoft.Json.Formatting.None).Should().Be("[[{\"name\":\"A\"},{\"name\":\"B\"}]]");
        }

        [Fact]
        public async Task Create_many_from_different_tables_throws()
        {
            var client = _server.CreateClient();
            var models = new[] { OdooDictionaryModel.Create("res.partner"), OdooDictionaryModel.Create("res.country") };

            Func<Task> act = () => client.CreateAsync(models);

            await act.Should().ThrowAsync<ArgumentException>().WithMessage("*same odoo table*");
        }

        [Fact]
        public async Task Create_many_with_empty_list_throws()
        {
            var client = _server.CreateClient();

            Func<Task> act = () => client.CreateAsync(new TestPartnerCreateModel[0]);

            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Create_dictionary_without_table_name_throws()
        {
            var client = _server.CreateClient();

            Func<Task> act = () => client.CreateAsync(new OdooDictionaryModel { ["name"] = "x" });

            await act.Should().ThrowAsync<ArgumentException>().WithMessage("*TableName*");
        }

        [Fact]
        public async Task Update_sends_write_with_ids_and_values()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(true);
            var client = _server.CreateClient();
            var model = OdooDictionaryModel.Create(() => new TestPartnerOdooModel { Name = "Changed", CountryId = null });

            var result = await client.UpdateRangeAsync(model, new long[] { 3, 4 });

            result.Value.Should().BeTrue();
            var call = _server.ObjectCalls.Single();
            call.Model.Should().Be("test.partner");
            call.ModelMethod.Should().Be("write");
            call.ModelArgs.ToString(Newtonsoft.Json.Formatting.None).Should().Be("[[3,4],{\"name\":\"Changed\",\"country_id\":null}]");
        }

        [Fact]
        public async Task Delete_sends_unlink()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(true);
            var client = _server.CreateClient();

            var result = await client.DeleteAsync(new TestPartnerOdooModel { Id = 5 });

            result.Value.Should().BeTrue();
            var call = _server.ObjectCalls.Single();
            call.ModelMethod.Should().Be("unlink");
            call.ModelArgs.ToString(Newtonsoft.Json.Formatting.None).Should().Be("[[5]]");
        }

        [Fact]
        public async Task Delete_range_of_different_tables_throws()
        {
            var client = _server.CreateClient();

            Func<Task> act = () => client.DeleteRangeAsync(new IOdooModel[] { new TestPartnerOdooModel { Id = 1 }, new TestCountryOdooModel { Id = 2 } });

            await act.Should().ThrowAsync<ArgumentException>();
            _server.Requests.Should().BeEmpty();
        }

        [Fact]
        public async Task Action_calls_method_on_model()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(true);
            var client = _server.CreateClient();

            var result = await client.ActionAsync("sale.order", "action_confirm", 15);

            result.Succeed.Should().BeTrue();
            var call = _server.ObjectCalls.Single();
            call.Model.Should().Be("sale.order");
            call.ModelMethod.Should().Be("action_confirm");
            call.ModelArgs.ToString(Newtonsoft.Json.Formatting.None).Should().Be("[15]");
        }

        [Fact]
        public async Task Get_model_sends_fields_get()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new { name = new { type = "char", @string = "Name", required = true } });
            var client = _server.CreateClient();

            var result = await client.GetModelAsync("res.partner");

            result.Succeed.Should().BeTrue();
            result.Value["name"].PropertyValueType.Should().Be(OdooValueTypeEnum.Char);
            result.Value["name"].ResultRequired.Should().BeTrue();
            var call = _server.ObjectCalls.Single();
            call.Method.Should().Be("execute");
            call.ModelMethod.Should().Be("fields_get");
        }

        [Fact]
        public async Task Version_does_not_require_login()
        {
            var client = _server.CreateClient();

            var result = await client.GetVersionAsync();

            result.Succeed.Should().BeTrue();
            result.Value.ServerSerie.Should().Be("15.0");
            _server.LoginCount.Should().Be(0);
        }

        [Fact]
        public async Task Response_is_mapped_to_model()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new object[]
            {
                new { id = 1, name = "Partner", is_company = true, country_id = new object[] { 20, "Poland" }, parent_id = false, category_id = new[] { 1, 2 }, type = "invoice", credit_limit = 0 }
            });
            var client = _server.CreateClient();

            var result = await client.GetAsync<TestPartnerOdooModel>();

            var partner = result.Value.Single();
            partner.Id.Should().Be(1);
            partner.Name.Should().Be("Partner");
            partner.IsCompany.Should().BeTrue();
            partner.CountryId.Should().Be(20);
            partner.ParentId.Should().BeNull();
            partner.CategoryId.Should().Equal(1, 2);
            partner.Type.Should().Be(TestPartnerTypeOdooEnum.InvoiceAddress);
            partner.CreditLimit.Should().Be(0);
        }

        #endregion
    }
}
