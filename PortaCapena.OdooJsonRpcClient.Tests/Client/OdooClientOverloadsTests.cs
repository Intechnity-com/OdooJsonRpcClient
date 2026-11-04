using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Client
{
    public class OdooClientOverloadsTests
    {
        private readonly FakeOdooServer _server = new FakeOdooServer { OnObjectCall = r => FakeOdooServer.Result(true) };

        [Fact]
        public async Task User_id_overloads_do_not_log_in()
        {
            _server.OnObjectCall = r => r.ModelMethod == "search_count" ? FakeOdooServer.Result(4) : FakeOdooServer.Result(new[] { new { id = 1, name = "A" } });
            var client = _server.CreateClient(context: new OdooContext("pl_PL"));

            var typed = await client.GetAsync<TestPartnerOdooModel>(99);
            var dictionaries = await client.GetAsync("res.partner", 99);
            var count = await client.GetCountAsync<TestPartnerOdooModel>(99);

            typed.Value.Single().Name.Should().Be("A");
            dictionaries.Value.Single()["name"].Should().Be("A");
            count.Value.Should().Be(4);
            _server.LoginCount.Should().Be(0);
            _server.ObjectCalls.Should().OnlyContain(x => x.Uid == 99 && (string)x.Kwargs["context"]["lang"] == "pl_PL");
        }

        [Fact]
        public async Task Get_current_user_uid_logs_in_once()
        {
            var client = _server.CreateClient();

            var first = await client.GetCurrentUserUidOrLoginAsync();
            var second = await client.GetCurrentUserUidOrLoginAsync();

            first.Value.Should().Be(FakeOdooServer.UserId);
            second.Value.Should().Be(FakeOdooServer.UserId);
            _server.LoginCount.Should().Be(1);
        }

        [Fact]
        public async Task Explicit_login_refreshes_cached_user_id()
        {
            var uid = 10;
            _server.OnLogin = r => FakeOdooServer.Result(Interlocked.Increment(ref uid));
            var client = _server.CreateClient();

            await client.LoginAsync();
            await client.LoginAsync();
            var current = await client.GetCurrentUserUidOrLoginAsync();

            current.Value.Should().Be(12);
            _server.LoginCount.Should().Be(2);
        }

        [Fact]
        public async Task Update_with_model_sends_all_properties()
        {
            var client = _server.CreateClient();

            await client.UpdateAsync(new TestPartnerCreateModel { Name = "New" }, 5);
            await client.UpdateRangeAsync(new TestPartnerCreateModel { Name = "All" }, new long[] { 1, 2 });

            _server.ObjectCalls.Select(x => x.ModelArgs.ToString(Formatting.None)).Should().Equal(
                "[[5],{\"name\":\"New\"}]",
                "[[1,2],{\"name\":\"All\"}]");
            _server.ObjectCalls.Should().OnlyContain(x => x.Model == "test.partner" && x.ModelMethod == "write");
        }

        [Fact]
        public async Task Update_dictionary_by_id()
        {
            var client = _server.CreateClient();

            var result = await client.UpdateAsync(OdooDictionaryModel.Create<TestPartnerOdooModel>(x => x.Name, "B"), 8);

            result.Value.Should().BeTrue();
            _server.ObjectCalls.Single().ModelArgs.ToString(Formatting.None).Should().Be("[[8],{\"name\":\"B\"}]");
        }

        [Fact]
        public async Task Delete_by_table_name_and_ids()
        {
            var client = _server.CreateClient();

            await client.DeleteAsync("res.partner", 3);
            await client.DeleteRangeAsync("res.partner", new long[] { 4, 5 });

            _server.ObjectCalls.Select(x => x.ModelArgs.ToString(Formatting.None)).Should().Equal("[[3]]", "[[4,5]]");
        }

        [Fact]
        public async Task Delete_null_model_throws()
        {
            var client = _server.CreateClient();

            Func<Task> act = () => client.DeleteAsync((IOdooModel)null);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Create_many_dictionaries_of_same_table()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new[] { 11, 12 });
            var client = _server.CreateClient();
            var models = new[]
            {
                OdooDictionaryModel.Create<TestPartnerOdooModel>(x => x.Name, "A"),
                OdooDictionaryModel.Create<TestPartnerOdooModel>(x => x.Name, "B"),
            };

            var result = await client.CreateAsync(models);

            result.Value.Should().Equal(11, 12);
            _server.ObjectCalls.Single().ModelArgs.ToString(Formatting.None).Should().Be("[[{\"name\":\"A\"},{\"name\":\"B\"}]]");
        }

        [Fact]
        public async Task Action_collection_sends_arguments_list()
        {
            var client = _server.CreateClient();

            await client.ActionCollectionAsync("mail.thread", "message_post", new object[] { new long[] { 1 }, "hello" });

            var call = _server.ObjectCalls.Single();
            call.ModelMethod.Should().Be("message_post");
            call.ModelArgs.ToString(Formatting.None).Should().Be("[[1],\"hello\"]");
        }

        [Fact]
        public async Task Get_model_with_cancellation_token()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new { name = new { type = "char" } });
            var client = _server.CreateClient();

            var result = await client.GetModelAsync("res.partner", CancellationToken.None);

            result.Value.Keys.Should().Equal("name");
        }

        [Fact]
        public async Task Count_uses_only_filters()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(3);
            var client = _server.CreateClient();
            var query = OdooQuery<TestPartnerOdooModel>.Create().Where(x => x.IsCompany, OdooOperator.EqualsTo, true);

            var result = await client.GetCountAsync<TestPartnerOdooModel>(query);

            result.Value.Should().Be(3);
            var call = _server.ObjectCalls.Single();
            call.ModelMethod.Should().Be("search_count");
            call.ModelArgs.ToString(Formatting.None).Should().Be("[[[\"is_company\",\"=\",true]]]");
        }

        [Fact]
        public async Task Model_without_table_name_attribute_throws()
        {
            var client = _server.CreateClient();

            Func<Task> act = () => client.GetAsync<ModelWithoutJsonProperty>();

            await act.Should().ThrowAsync<ArgumentException>().WithMessage("*OdooTableNameAttribute*");
        }

        [Fact]
        public void Repository_shares_given_client()
        {
            var client = _server.CreateClient();

            var repository = new OdooRepository<TestPartnerOdooModel>(client);

            repository.Config.Should().BeSameAs(client.Config);
        }

        [Fact]
        public void Repository_requires_client()
        {
            Action act = () => new OdooRepository<TestPartnerOdooModel>((OdooClient)null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Repository_with_http_client_uses_it()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(new object[0]);
            var repository = new OdooRepository<TestPartnerOdooModel>(_server.CreateConfig(), new HttpClient(_server));

            var result = await repository.Query().ToListAsync();

            result.Succeed.Should().BeTrue();
            _server.ObjectCalls.Should().ContainSingle();
        }
    }
}
