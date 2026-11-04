using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example.Rpc
{
    /// <summary>
    /// many2many commands (issue #35) on Odoo 15 (docker/odoo15) with the JSON-RPC API.
    /// </summary>
    public class OdooCommandIntegrationTestsJsonRpc : RequestTestBase
    {
        [Fact]
        public async Task Many2many_field_can_be_changed_with_commands()
        {
            var repository = new OdooRepository<IntegrationPartnerOdooModel>(TestConfig);
            var partnerId = (await repository.CreateAsync(OdooDictionaryModel.Create(() => new IntegrationPartnerOdooModel
            {
                Name = $"Command test {Guid.NewGuid():N}"
            }).Add(x => x.CategoryId, OdooCommand.Create(new OdooDictionaryModel { { "name", $"Command A {Guid.NewGuid():N}" } })))).Value;
            var categoryA = (await repository.Query().ById(partnerId).FirstAsync()).Value.CategoryId.Single();
            var categoryB = 0L;
            try
            {
                await repository.UpdateAsync(OdooDictionaryModel.Create<IntegrationPartnerOdooModel>(x => x.CategoryId, OdooCommand.Create(new OdooDictionaryModel { { "name", $"Command B {Guid.NewGuid():N}" } })), partnerId);
                var afterCreate = (await repository.Query().ById(partnerId).FirstAsync()).Value.CategoryId;
                categoryB = afterCreate.Single(x => x != categoryA);

                await repository.UpdateAsync(OdooDictionaryModel.Create<IntegrationPartnerOdooModel>(x => x.CategoryId, OdooCommand.Unlink(categoryA)), partnerId);
                var afterUnlink = (await repository.Query().ById(partnerId).FirstAsync()).Value.CategoryId;

                await repository.UpdateAsync(OdooDictionaryModel.Create<IntegrationPartnerOdooModel>(x => x.CategoryId, OdooCommand.Link(categoryA)), partnerId);
                var afterLink = (await repository.Query().ById(partnerId).FirstAsync()).Value.CategoryId;

                await repository.UpdateAsync(OdooDictionaryModel.Create<IntegrationPartnerOdooModel>(x => x.CategoryId, OdooCommand.Set(categoryA)), partnerId);
                var afterSet = (await repository.Query().ById(partnerId).FirstAsync()).Value.CategoryId;

                await repository.UpdateAsync(OdooDictionaryModel.Create<IntegrationPartnerOdooModel>(x => x.CategoryId, OdooCommand.Clear()), partnerId);
                var afterClear = (await repository.Query().ById(partnerId).FirstAsync()).Value.CategoryId;

                afterCreate.Should().BeEquivalentTo(new[] { categoryA, categoryB });
                afterUnlink.Should().BeEquivalentTo(new[] { categoryB });
                afterLink.Should().BeEquivalentTo(new[] { categoryA, categoryB });
                afterSet.Should().BeEquivalentTo(new[] { categoryA });
                afterClear.Should().BeEmpty();
            }
            finally
            {
                await repository.DeleteAsync(partnerId);
                await new OdooClient(TestConfig).DeleteRangeAsync("res.partner.category", new[] { categoryA, categoryB });
            }
        }
    }
}
