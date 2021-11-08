using System.Linq;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooConfigTests
    {
        [Fact]
        public void Can_create_OdooConfig()
        {
            var context = new OdooConfig("https://odoo-api-url.com", "odoo-db-name", "admin", "admin");

            context.ApiUrl.Should().Be("https://odoo-api-url.com");

            context.ApiUrlJson.Should().Be("https://odoo-api-url.com/jsonrpc");

            context.DbName.Should().Be("odoo-db-name");
            context.UserName.Should().Be("admin");
            context.Password.Should().Be("admin");

            context.Context.Should().NotBeNull();
            context.Context.Should().BeEmpty();
        }

        [Fact]
        public void Can_create_with_slash_on_end()
        {
            var context = new OdooConfig("https://odoo-api-url.com/", "odoo-db-name", "admin", "admin");

            context.ApiUrl.Should().Be("https://odoo-api-url.com");

            context.ApiUrlJson.Should().Be("https://odoo-api-url.com/jsonrpc");

            context.DbName.Should().Be("odoo-db-name");
            context.UserName.Should().Be("admin");
            context.Password.Should().Be("admin");

            context.Context.Should().NotBeNull();
            context.Context.Should().BeEmpty();
        }
    }
}