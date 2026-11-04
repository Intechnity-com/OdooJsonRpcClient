using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooQueryBuilderTests
    {
        private readonly FakeOdooServer _server = new FakeOdooServer();

        private OdooRepository<TestPartnerOdooModel> CreateRepository(params object[] records)
        {
            _server.OnObjectCall = r => r.ModelMethod == "search_count"
                ? FakeOdooServer.Result(records.Length)
                : FakeOdooServer.Result(records);
            return new OdooRepository<TestPartnerOdooModel>(_server.CreateClient());
        }

        [Fact]
        public async Task First_does_not_change_builder_limit()
        {
            var repository = CreateRepository(new { id = 1 }, new { id = 2 });
            var query = repository.Query().Where(x => x.IsCompany, OdooOperator.EqualsTo, true);

            await query.FirstOrDefaultAsync();
            await query.ToListAsync();

            _server.ObjectCalls[0].Kwargs["limit"].Value<int>().Should().Be(1);
            (_server.ObjectCalls[1].Kwargs?["limit"]).Should().BeNull("ToListAsync after FirstOrDefaultAsync must not inherit Take(1)");
        }

        [Fact]
        public async Task First_respects_smaller_user_limit()
        {
            var repository = CreateRepository(new { id = 1 });

            await repository.Query().Take(1).SingleAsync();

            _server.ObjectCalls.Single().Kwargs["limit"].Value<int>().Should().Be(1);
        }

        [Fact]
        public async Task First_keeps_filters_order_and_offset()
        {
            var repository = CreateRepository(new { id = 1 });

            await repository.Query()
                .Where(x => x.Name, OdooOperator.EqualsTo, "a")
                .OrderBy(x => x.Name).ThenOrderByDescending(x => x.Id)
                .Skip(3)
                .Select(x => new { x.Name })
                .FirstAsync();

            var call = _server.ObjectCalls.Single();
            JsonConvert.SerializeObject(call.ModelArgs).Should().Be("[[[\"name\",\"=\",\"a\"]]]");
            call.Kwargs["order"].Value<string>().Should().Be("name ASC, id DESC");
            call.Kwargs["offset"].Value<int>().Should().Be(3);
            call.Kwargs["fields"].ToObject<string[]>().Should().Equal("name");
        }

        [Fact]
        public async Task FirstOrDefault_returns_null_value_when_nothing_found()
        {
            var repository = CreateRepository();

            var result = await repository.Query().ById(99).FirstOrDefaultAsync();

            result.Succeed.Should().BeTrue();
            result.Value.Should().BeNull();
        }

        [Fact]
        public async Task First_throws_when_nothing_found_like_linq()
        {
            var repository = CreateRepository();

            Func<Task> act = () => repository.Query().FirstAsync();

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Single_throws_when_more_than_one_found_like_linq()
        {
            var repository = CreateRepository(new { id = 1 }, new { id = 2 });

            Func<Task> act = () => repository.Query().SingleAsync();

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Any_and_count()
        {
            var repository = CreateRepository(new { id = 1 }, new { id = 2 });

            (await repository.Query().AnyAsync()).Value.Should().BeTrue();
            (await repository.Query().CountAsync()).Value.Should().Be(2);
            _server.ObjectCalls.Last().ModelMethod.Should().Be("search_count");
        }

        [Fact]
        public async Task Failed_request_is_returned_not_thrown()
        {
            _server.OnObjectCall = r => FakeOdooServer.Error("odoo.exceptions.UserError", "bad");
            var repository = new OdooRepository<TestPartnerOdooModel>(_server.CreateClient());

            var result = await repository.Query().FirstAsync();

            result.Failed.Should().BeTrue();
            result.Error.Data.Message.Should().Be("bad");
        }

        [Fact]
        public async Task ById_and_ByIds()
        {
            var repository = CreateRepository();

            await repository.Query().ById(5).ToListAsync();
            await repository.Query().ByIds(1, 2).ToListAsync();
            await repository.Query().ByIds().ToListAsync();

            _server.ObjectCalls.Select(x => JsonConvert.SerializeObject(x.ModelArgs)).Should().Equal(
                "[[[\"id\",\"=\",5]]]",
                "[[[\"id\",\"in\",[1,2]]]]",
                "[[]]");
        }

        [Fact]
        public async Task Where_on_related_fields_builds_dotted_path()
        {
            var repository = CreateRepository();

            await repository.Query()
                .Where<TestCountryOdooModel>(x => x.CountryId, x => x.Code, OdooOperator.EqualsTo, "PL")
                .Where<TestCountryOdooModel, TestCurrencyOdooModel>(x => x.CountryId, x => x.CurrencyId, x => x.Name, OdooOperator.EqualsTo, "PLN")
                .ToListAsync();

            JsonConvert.SerializeObject(_server.ObjectCalls.Single().ModelArgs)
                .Should().Be("[[[\"country_id.code\",\"=\",\"PL\"],[\"country_id.currency_id.name\",\"=\",\"PLN\"]]]");
        }

        [Fact]
        public async Task With_context_is_sent()
        {
            var repository = CreateRepository();

            await repository.Query().WithContext("active_test", false).ToListAsync();

            _server.ObjectCalls.Single().Kwargs["context"]["active_test"].Value<bool>().Should().BeFalse();
        }

        [Fact]
        public async Task With_null_context_does_not_throw()
        {
            var repository = CreateRepository();

            var result = await repository.Query().WithContext(null).ToListAsync();

            result.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Select_simplified_model_skips_properties_without_json_property()
        {
            var repository = CreateRepository();

            await repository.Query().SelectSimplifiedModel().ToListAsync();

            var fields = _server.ObjectCalls.Single().Kwargs["fields"].ToObject<string[]>();
            fields.Should().Contain("name").And.NotContainNulls();
        }
    }
}
