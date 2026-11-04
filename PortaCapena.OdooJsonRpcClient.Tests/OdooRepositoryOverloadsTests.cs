using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooRepositoryOverloadsTests
    {
        private readonly FakeOdooServer _server = new FakeOdooServer { OnObjectCall = r => FakeOdooServer.Result(1) };

        [Fact]
        public async Task Cancellation_token_overloads_send_same_requests()
        {
            var repository = new OdooRepository<TestPartnerOdooModel>(_server.CreateClient());
            var token = CancellationToken.None;

            await repository.CreateAsync(new TestPartnerCreateModel { Name = "a" }, null, token);
            await repository.CreateAsync(new OdooDictionaryModel { ["name"] = "b" }, null, token);
            await repository.CreateAsync(new[] { new TestPartnerCreateModel { Name = "c" } }, null, token);
            await repository.CreateAsync(new[] { new OdooDictionaryModel { ["name"] = "d" } }, null, token);
            await repository.UpdateAsync(new TestPartnerCreateModel { Name = "e" }, 1, null, token);
            await repository.UpdateRangeAsync(new TestPartnerCreateModel { Name = "f" }, new long[] { 1 }, null, token);
            await repository.UpdateAsync(new OdooDictionaryModel { ["name"] = "g" }, 1, null, token);
            await repository.UpdateRangeAsync(new OdooDictionaryModel { ["name"] = "h" }, new long[] { 1 }, null, token);
            await repository.DeleteAsync(new TestPartnerOdooModel { Id = 1 }, null, token);
            await repository.DeleteAsync(2, null, token);
            await repository.DeleteRangeAsync(new[] { new TestPartnerOdooModel { Id = 3 } }, null, token);
            await repository.ActionAsync("action_archive", new long[] { 4 }, null, token);

            _server.ObjectCalls.Should().HaveCount(12).And.OnlyContain(x => x.Model == "test.partner");
            _server.ObjectCalls.Select(x => x.ModelMethod).Should().Equal(
                "create", "create", "create", "create", "write", "write", "write", "write", "unlink", "unlink", "unlink", "action_archive");
        }

        [Fact]
        public async Task Overloads_without_token_send_model_values()
        {
            var repository = new OdooRepository<TestPartnerOdooModel>(_server.CreateClient());

            await repository.CreateAsync(new TestPartnerCreateModel { Name = "a" });
            await repository.CreateAsync(new[] { new TestPartnerCreateModel { Name = "b" } });
            await repository.UpdateAsync(new TestPartnerCreateModel { Name = "c" }, 5);
            await repository.UpdateRangeAsync(new TestPartnerCreateModel { Name = "d" }, new long[] { 6, 7 });
            await repository.DeleteAsync(new TestPartnerOdooModel { Id = 8 });

            _server.ObjectCalls.Select(x => x.ModelArgs.ToString(Formatting.None)).Should().Equal(
                "[{\"name\":\"a\"}]",
                "[[{\"name\":\"b\"}]]",
                "[[5],{\"name\":\"c\"}]",
                "[[6,7],{\"name\":\"d\"}]",
                "[[8]]");
        }

        [Fact]
        public async Task Update_range_always_targets_repository_table()
        {
            var repository = new OdooRepository<TestPartnerOdooModel>(_server.CreateClient());

            await repository.UpdateRangeAsync(OdooDictionaryModel.Create("other.table"), new long[] { 1 });

            _server.ObjectCalls.Single().Model.Should().Be("test.partner");
        }

        [Fact]
        public async Task Interface_can_be_used_for_repository()
        {
            IOdooRepository<TestPartnerOdooModel> repository = new OdooRepository<TestPartnerOdooModel>(_server.CreateClient());

            var result = await repository.DeleteAsync(9);

            result.Succeed.Should().BeTrue();
        }
    }
}
