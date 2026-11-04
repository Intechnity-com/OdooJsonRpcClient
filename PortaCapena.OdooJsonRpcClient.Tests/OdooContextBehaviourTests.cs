using System;
using System.Collections.Generic;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooContextBehaviourTests
    {
        [Fact]
        public void Null_sources_create_empty_context()
        {
            new OdooContext((Dictionary<string, object>)null).Should().BeEmpty();
            new OdooContext((OdooContext[])null).Should().BeEmpty();
            new OdooContext(new OdooContext("pl_PL"), null).Language.Should().Be("pl_PL");
        }

        [Fact]
        public void Later_contexts_override_earlier()
        {
            var merged = new OdooContext(new OdooContext("pl_PL", "Europe/Warsaw"), new OdooContext("nl_NL"));

            merged.Language.Should().Be("nl_NL");
            merged.Timezone.Should().Be("Europe/Warsaw");
        }

        [Fact]
        public void Company_ids_are_read_regardless_of_numeric_type()
        {
            var context = new OdooContext { ["force_company"] = 3, ["allowed_company_ids"] = new[] { 4 } };

            context.ForceCompany.Should().Be(3);
            context.AllowedCompanyId.Should().Be(4);
        }

        [Fact]
        public void Setting_null_removes_key()
        {
            var context = new OdooContext("pl_PL") { ForceCompany = 1 };

            context.Language = null;
            context.ForceCompany = null;

            context.Should().BeEmpty();
        }

        [Fact]
        public void Config_requires_url()
        {
            Action act = () => new OdooConfig(" ", "db", "u", "p");

            act.Should().Throw<ArgumentException>();
        }
    }
}
