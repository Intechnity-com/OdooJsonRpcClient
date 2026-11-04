using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example.Rpc
{
    /// <summary>
    /// API key instead of password with the JSON-RPC API, on Odoo 19 (docker/odoo19) where the key is seeded (issue #103).
    /// </summary>
    public class ApiKeyLoginIntegrationTestsJsonRpc : RequestTestBase
    {
        [Fact]
        public async Task Api_key_can_be_used_as_password()
        {
            var config = new OdooConfig(JsonApiTestConfig.ApiUrl, JsonApiTestConfig.DbName, JsonApiTestConfig.UserName, JsonApiTestConfig.Password);
            var client = new OdooClient(config);

            var login = await client.LoginAsync();
            var partners = await new OdooRepository<IntegrationPartnerOdooModel>(config).Query().Take(1).ToListAsync();

            config.Protocol.Should().Be(Consts.OdooApiProtocol.JsonRpc);
            login.Succeed.Should().BeTrue(login.Message);
            partners.Succeed.Should().BeTrue(partners.Message);
            partners.Value.Should().ContainSingle();
        }
    }
}
