using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example.Rpc
{
    /// <summary>
    /// Account codes on Odoo 15 (docker/odoo15) with the JSON-RPC API (issue #121).
    /// </summary>
    public class AccountIntegrationTestsJsonRpc : RequestTestBase
    {
        [Fact]
        public async Task Account_codes_are_returned()
        {
            var accounts = await new OdooRepository<IntegrationAccountOdooModel>(TestConfig).Query().ToListAsync();

            accounts.Succeed.Should().BeTrue(accounts.Message);
            accounts.Value.Should().NotBeEmpty();
            accounts.Value.Should().OnlyContain(x => !string.IsNullOrEmpty(x.Code));
        }
    }
}
