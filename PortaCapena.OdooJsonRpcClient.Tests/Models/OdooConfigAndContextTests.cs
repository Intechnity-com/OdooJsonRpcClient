using System;
using System.Collections.Generic;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Models
{
    public class OdooConfigAndContextTests
    {
        [Fact]
        public void Config_with_language_and_timezone()
        {
            var language = new OdooConfig("http://odoo.test", "db", "u", "p", "pl_PL");
            var languageTimezone = new OdooConfig("http://odoo.test", "db", "u", "p", "pl_PL", "Europe/Warsaw", TimeSpan.FromSeconds(3));

            language.Context.Language.Should().Be("pl_PL");
            languageTimezone.Context.Language.Should().Be("pl_PL");
            languageTimezone.Context.Timezone.Should().Be("Europe/Warsaw");
            languageTimezone.Timeout.Should().Be(TimeSpan.FromSeconds(3));
        }

        [Fact]
        public void Config_copies_given_context()
        {
            var context = new OdooContext("nl_NL");

            var config = new OdooConfig("http://odoo.test/", "db", "u", "p", context);
            context.Language = "pl_PL";

            config.Context.Language.Should().Be("nl_NL");
            config.ApiUrl.Should().Be("http://odoo.test");
            config.ApiUrlJson.Should().Be("http://odoo.test/jsonrpc");
            config.DbName.Should().Be("db");
            config.UserName.Should().Be("u");
            config.Password.Should().Be("p");
        }

        [Fact]
        public void Config_without_url_throws()
        {
            Action nullUrl = () => new OdooConfig(null, "db", "u", "p");

            nullUrl.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Context_properties_use_odoo_keys()
        {
            var context = new OdooContext
            {
                Tag = "tag",
                ActiveModel = "res.partner",
                DefaultIsCompany = true,
                ForceCompany = 2,
                AllowedCompanyId = 3
            };

            context["tag"].Should().Be("tag");
            context["active_model"].Should().Be("res.partner");
            context["default_is_company"].Should().Be(true);
            context["force_company"].Should().Be(2L);
            context["allowed_company_ids"].Should().BeEquivalentTo(new long[] { 3 });
            context.Tag.Should().Be("tag");
            context.ActiveModel.Should().Be("res.partner");
            context.DefaultIsCompany.Should().BeTrue();
            context.ForceCompany.Should().Be(2);
            context.AllowedCompanyId.Should().Be(3);
        }

        [Fact]
        public void Context_missing_or_invalid_values_are_null()
        {
            var context = new OdooContext { ["force_company"] = "not a number" };

            context.ForceCompany.Should().BeNull();
            context.AllowedCompanyId.Should().BeNull();
            context.DefaultIsCompany.Should().BeNull();
            context.Timezone.Should().BeNull();
        }

        [Fact]
        public void Context_from_dictionary_copies_all_values()
        {
            var context = new OdooContext(new Dictionary<string, object> { ["lang"] = "pl_PL", ["tz"] = null });

            context.Should().ContainKey("lang").And.ContainKey("tz");
            context.Timezone.Should().BeNull();
        }

        [Fact]
        public void Company_id_from_string_is_converted()
        {
            var context = new OdooContext { ["force_company"] = "5", ["allowed_company_ids"] = new List<object> { 6L } };

            context.ForceCompany.Should().Be(5);
            context.AllowedCompanyId.Should().Be(6);
        }
    }
}
