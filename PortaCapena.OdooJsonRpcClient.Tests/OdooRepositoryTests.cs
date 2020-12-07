using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Tests.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooRepositoryTests : OdooTestBase
    {
        [Fact]
        public async Task Get_All_products()
        {
            var repository = new OdooRepository<OdooProductProduct>(Config);
            var products = await repository.Query().ToListAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().BeGreaterThan(0);
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Get_All_products_with_where()
        {
            var repository = new OdooRepository<OdooProductProduct>(Config);
            var products = await repository.Query().ById(66).FirstOrDefaultAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Succeed.Should().BeTrue();
        }
    }
}