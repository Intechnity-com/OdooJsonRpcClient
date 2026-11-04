using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Client
{
    public class OdooRepositoryTests
    {
        private readonly FakeOdooServer _server = new FakeOdooServer { OnObjectCall = r => FakeOdooServer.Result(1) };

        private OdooRepository<TestPartnerOdooModel> CreateRepository() => new OdooRepository<TestPartnerOdooModel>(_server.CreateClient());

        [Fact]
        public void Table_name_comes_from_attribute()
        {
            CreateRepository().TableName.Should().Be("test.partner");
        }

        [Fact]
        public async Task Dictionary_without_table_name_goes_to_repository_table()
        {
            var repository = CreateRepository();

            await repository.CreateAsync(new OdooDictionaryModel { ["name"] = "a" });
            await repository.UpdateAsync(new OdooDictionaryModel { ["name"] = "b" }, 1);
            await repository.UpdateRangeAsync(new OdooDictionaryModel { ["name"] = "c" }, new long[] { 1 });
            await repository.CreateAsync(new[] { new OdooDictionaryModel { ["name"] = "d" } });

            _server.ObjectCalls.Should().HaveCount(4).And.OnlyContain(x => x.Model == "test.partner");
        }

        [Fact]
        public async Task Dictionary_with_table_name_keeps_it_on_create()
        {
            var repository = CreateRepository();

            await repository.CreateAsync(OdooDictionaryModel.Create("other.model"));

            _server.ObjectCalls.Single().Model.Should().Be("other.model");
        }

        [Fact]
        public async Task Delete_by_id_and_models()
        {
            _server.OnObjectCall = r => FakeOdooServer.Result(true);
            var repository = CreateRepository();

            await repository.DeleteAsync(3);
            await repository.DeleteRangeAsync(new[] { new TestPartnerOdooModel { Id = 4 }, new TestPartnerOdooModel { Id = 5 } });

            _server.ObjectCalls.Select(x => x.ModelArgs.ToString(Newtonsoft.Json.Formatting.None)).Should().Equal(new[] { "[[3]]", "[[4,5]]" }, "ids must be one positional argument - odoo spreads args into unlink(*args)");
        }

        [Fact]
        public async Task Action_is_called_on_repository_table()
        {
            var repository = CreateRepository();

            await repository.ActionAsync("action_archive", new long[] { 1 });

            var call = _server.ObjectCalls.Single();
            call.Model.Should().Be("test.partner");
            call.ModelMethod.Should().Be("action_archive");
        }
    }
}
