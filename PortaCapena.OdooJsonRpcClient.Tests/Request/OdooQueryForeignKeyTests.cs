using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Request
{
    /// <summary>
    /// Where overloads on related fields exist for every combination of long / long? keys.
    /// </summary>
    public class OdooQueryForeignKeyTests
    {
        [Fact]
        public void Query_builds_paths_for_all_key_type_combinations()
        {
            var query = OdooQuery<TestPartnerOdooModel>.Create()
                .Where<TestPartnerOdooModel>(x => x.Id, p => p.Name, OdooOperator.EqualsTo, "a")
                .Where<TestPartnerOdooModel>(x => x.ParentId, p => p.Name, OdooOperator.EqualsTo, "b")
                .Where<TestPartnerOdooModel, TestCountryOdooModel>(x => x.Id, p => p.CountryId, c => c.Code, OdooOperator.EqualsTo, "c")
                .Where<TestPartnerOdooModel, TestCountryOdooModel>(x => x.ParentId, p => p.CountryId, c => c.Code, OdooOperator.EqualsTo, "d")
                .Where<TestCountryOdooModel, TestCurrencyOdooModel>(x => x.CountryId, c => c.CurrencyId, cur => cur.Name, OdooOperator.EqualsTo, "e")
                .Where<TestCountryOdooModel, TestCurrencyOdooModel>(x => x.Id, c => c.CurrencyId, cur => cur.Name, OdooOperator.EqualsTo, "f");

            JsonConvert.SerializeObject(query.Filters).Should().Be(
                "[[\"id.name\",\"=\",\"a\"]," +
                "[\"parent_id.name\",\"=\",\"b\"]," +
                "[\"id.country_id.code\",\"=\",\"c\"]," +
                "[\"parent_id.country_id.code\",\"=\",\"d\"]," +
                "[\"country_id.currency_id.name\",\"=\",\"e\"]," +
                "[\"id.currency_id.name\",\"=\",\"f\"]]");
        }

        [Fact]
        public async Task Builder_sends_paths_for_all_key_type_combinations()
        {
            var server = new FakeOdooServer();
            var repository = new OdooRepository<TestPartnerOdooModel>(server.CreateClient());

            await repository.Query()
                .Where<TestPartnerOdooModel>(x => x.Id, p => p.Name, OdooOperator.EqualsTo, "a")
                .Where<TestPartnerOdooModel>(x => x.ParentId, p => p.Name, OdooOperator.EqualsTo, "b")
                .Where<TestPartnerOdooModel, TestCountryOdooModel>(x => x.Id, p => p.CountryId, c => c.Code, OdooOperator.EqualsTo, "c")
                .Where<TestPartnerOdooModel, TestCountryOdooModel>(x => x.ParentId, p => p.CountryId, c => c.Code, OdooOperator.EqualsTo, "d")
                .Where<TestCountryOdooModel, TestCurrencyOdooModel>(x => x.CountryId, c => c.CurrencyId, cur => cur.Name, OdooOperator.EqualsTo, "e")
                .Where<TestCountryOdooModel, TestCurrencyOdooModel>(x => x.Id, c => c.CurrencyId, cur => cur.Name, OdooOperator.EqualsTo, "f")
                .Where(OdooFilter<TestPartnerOdooModel>.Create().EqualTo(x => x.Color, 1))
                .ToListAsync();

            var domain = server.ObjectCalls.Single().ModelArgs[0];
            domain.Select(x => (string)x[0]).Should().Equal(
                "id.name", "parent_id.name", "id.country_id.code", "parent_id.country_id.code", "country_id.currency_id.name", "id.currency_id.name", "color");
        }

        [Fact]
        public async Task Builder_paging_and_ordering()
        {
            var server = new FakeOdooServer();
            var repository = new OdooRepository<TestPartnerOdooModel>(server.CreateClient());

            await repository.Query().OrderByDescending(x => x.Name).ThenOrderBy(x => x.Id).Select("name", "email").Skip(5).Take(10).ToListAsync();

            var kwargs = server.ObjectCalls.Single().Kwargs;
            ((string)kwargs["order"]).Should().Be("name DESC, id ASC");
            ((int)kwargs["offset"]).Should().Be(5);
            ((int)kwargs["limit"]).Should().Be(10);
            kwargs["fields"].ToObject<string[]>().Should().Equal("name", "email");
        }
    }
}
