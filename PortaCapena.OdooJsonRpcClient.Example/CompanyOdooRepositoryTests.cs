using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example
{
    public class CompanyOdooRepositoryTests : RequestTestBase
    {
        private class CompanyRepository : OdooRepository<ResCompanyOdooModel>
        {
            public CompanyRepository() : base(TestConfig) { }
        }

        [Fact]
        public async Task Get_All()
        {
            var repo = new CompanyRepository();

            var result = await repo.Query().ToListAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
            result.Value.Should().NotBeNull().And.NotBeEmpty();
        }

        [Fact]
        public async Task Get_By_Id()
        {
            var repo = new CompanyRepository();

            var result = await repo.Query().ById(1).FirstOrDefaultAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_By_Ids()
        {
            var repo = new CompanyRepository();

            var result = await repo.Query().ByIds(1, 2).ToListAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
            result.Value.Should().NotBeNull().And.NotBeEmpty();
            result.Value.Length.Should().Be(2);
        }

        [Fact]
        public async Task Get_By_Name()
        {
            var repo = new CompanyRepository();

            var result = await repo.Query().Where(x => x.Name, OdooOperator.EqualsTo, "My Company").FirstOrDefaultAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_By_Country()
        {
            var repo = new CompanyRepository();

            var result = await repo.Query().Where(x => x.CountryCode, OdooOperator.EqualsTo, "BE").ToListAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
            result.Value.Should().NotBeNull().And.NotBeEmpty();
        }

        [Fact]
        public async Task Get_By_Country_name()
        {
            var repo = new CompanyRepository();

            var result = await repo.Query().Where<ResCountryOdooModel>(x => x.CountryId, x => x.Name, OdooOperator.EqualsTo, "Belgium").ToListAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
            result.Value.Should().NotBeNull().And.NotBeEmpty();
        }
    }
}