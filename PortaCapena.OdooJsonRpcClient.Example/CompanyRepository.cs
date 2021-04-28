using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example
{
    public class CompanyOdooRepository : TestBase
    {
        public class CompanyRepository : OdooRepository<ResCompanyOdooModel>
        {
            public CompanyRepository() : base(Config) { }
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
    }
}