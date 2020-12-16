using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example
{
    public class OdooRepositoryRequests : OdooTestBase
    {
        [Fact]
        public async Task Can_get_all_products()
        {
            var repository = new OdooRepository<ProductProductOdooDto>(Config);
            var products = await repository.Query().ToListAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().BeGreaterThan(0);
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Can_get_product_by_id()
        {
            var repository = new OdooRepository<ProductProductOdooDto>(Config);
            var products = await repository.Query().ById(66).FirstOrDefaultAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Can_get_products_with_where_and_select()
        {
            var repository = new OdooRepository<ProductProductOdooDto>(Config);
            var products = await repository.Query()
                .ById(66)
                .Select(x => new { x.Name })
                .FirstOrDefaultAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Can_get_all_OdooVoucher()
        {
            var repository = new OdooRepository<CouponProgramOdooDto>(Config);

            var products = await repository.Query()
                .SelectSimplifiedModel()
                .FirstOrDefaultAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();
        }
    }
}