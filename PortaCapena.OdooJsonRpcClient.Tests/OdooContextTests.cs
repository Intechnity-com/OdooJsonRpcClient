using System.Linq;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooContextTests
    {
        [Fact]
        public void Can_create_with_lang_param()
        {
            var context = new OdooContext("pl_PL");

            context.Count.Should().Be(1);
            context.First().Key.Should().Be("lang");
            context.First().Value.Should().Be("pl_PL");
        }

        [Fact]
        public void Can_create_with_lang_and_timezone_param()
        {
            var context = new OdooContext("pl_PL", "time1");

            context.Count.Should().Be(2);
            context.First().Key.Should().Be("lang");
            context.First().Value.Should().Be("pl_PL");

            context.Skip(1).First().Key.Should().Be("tz");
            context.Skip(1).First().Value.Should().Be("time1");
        }

        [Fact]
        public void Can_update_one_prop()
        {
            var context = new OdooContext("pl_PL", "time1");

            context.Language = "new Language";

            context.Count.Should().Be(2);
            context.First().Key.Should().Be("lang");
            context.First().Value.Should().Be("new Language");

            context.Skip(1).First().Key.Should().Be("tz");
            context.Skip(1).First().Value.Should().Be("time1");
        }

        [Fact]
        public void Can_create_with_lang_param_ctor_and_with_dict()
        {
            var context = new OdooContext("pl_PL")
            {
                {"test_prop", "test value"}
            };

            context.Count.Should().Be(2);
            context.First().Key.Should().Be("lang");
            context.First().Value.Should().Be("pl_PL");

            context.Skip(1).First().Key.Should().Be("test_prop");
            context.Skip(1).First().Value.Should().Be("test value");
        }

        [Fact]
        public void Can_set_null_to_remove_value()
        {
            var context = new OdooContext("pl_PL", "time1");

            context.Timezone = null;

            context.Count.Should().Be(1);
            context.First().Key.Should().Be("lang");
            context.First().Value.Should().Be("pl_PL");
        }
    }
}