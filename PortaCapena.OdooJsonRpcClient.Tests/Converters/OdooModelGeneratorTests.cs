using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared.Models.Fake;
using PortaCapena.OdooJsonRpcClient.Shared.Models.Odoo15;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Converters
{
    public class OdooModelGeneratorTests
    {
        [Theory]
        [InlineData("char", false, "string")]
        [InlineData("text", false, "string")]
        [InlineData("html", false, "string")]
        [InlineData("binary", false, "string")]
        [InlineData("properties", false, "string")]
        [InlineData("properties_definition", false, "string")]
        [InlineData("boolean", true, "bool")]
        [InlineData("boolean", false, "bool?")]
        [InlineData("integer", true, "int")]
        [InlineData("integer", false, "int?")]
        [InlineData("float", true, "double")]
        [InlineData("float", false, "double?")]
        [InlineData("monetary", true, "decimal")]
        [InlineData("monetary", false, "decimal?")]
        [InlineData("date", true, "DateTime")]
        [InlineData("datetime", false, "DateTime?")]
        [InlineData("many2one", true, "long")]
        [InlineData("many2one", false, "long?")]
        [InlineData("many2one_reference", false, "long?")]
        [InlineData("one2one", true, "long")]
        [InlineData("many2many", false, "long[]")]
        [InlineData("one2many", false, "long[]")]
        public void Odoo_field_type_maps_to_dotnet_type(string odooType, bool required, string expected)
        {
            var property = new KeyValuePair<string, OdooPropertyInfo>("field", new OdooPropertyInfo { Type = odooType, ResultRequired = required });

            OdooModelMapper.ConvertToDotNetPropertyTypeName(property, "res.partner").Should().Be(expected);
        }

        [Fact]
        public void Id_field_is_always_long()
        {
            var property = new KeyValuePair<string, OdooPropertyInfo>("id", new OdooPropertyInfo { Type = "integer" });

            OdooModelMapper.ConvertToDotNetPropertyTypeName(property, "res.partner").Should().Be("long");
        }

        [Fact]
        public void Selection_maps_to_generated_enum_name()
        {
            var optional = new KeyValuePair<string, OdooPropertyInfo>("state", new OdooPropertyInfo { Type = "selection", String = "Status" });
            var required = new KeyValuePair<string, OdooPropertyInfo>("state", new OdooPropertyInfo { Type = "selection", String = "Status", ResultRequired = true });

            OdooModelMapper.ConvertToDotNetPropertyTypeName(optional, "sale.order").Should().Be("StatusSaleOrderOdooEnum?");
            OdooModelMapper.ConvertToDotNetPropertyTypeName(required, "sale.order").Should().Be("StatusSaleOrderOdooEnum");
        }

        [Fact]
        public void Related_maps_to_model_of_relation_field()
        {
            var property = new KeyValuePair<string, OdooPropertyInfo>("x", new OdooPropertyInfo { Type = "related", RelationField = "res.country" });

            OdooModelMapper.ConvertToDotNetPropertyTypeName(property, "res.partner").Should().Be("ResCountryOdooModel");
        }

        [Fact]
        public void Generated_model_contains_attributes_properties_and_enums()
        {
            var fields = new Dictionary<string, OdooPropertyInfo>
            {
                ["id"] = new OdooPropertyInfo { Type = "integer", String = "ID" },
                ["name"] = new OdooPropertyInfo { Type = "char", String = "Name", ResultRequired = true, Help = "Line 1\nLine 2" },
                ["partner_id"] = new OdooPropertyInfo { Type = "many2one", String = "Customer", Relation = "res.partner", RelationField = "x" },
                ["state"] = new OdooPropertyInfo { Type = "selection", String = "Status", Selection = new[] { new[] { "draft", "Quotation" }, new[] { "sale", "Sales Order" } } },
            };

            var code = OdooModelMapper.GetDotNetModel("sale.order", fields);

            code.Should().Contain("[OdooTableName(\"sale.order\")]");
            code.Should().Contain("[JsonConverter(typeof(OdooModelConverter))]");
            code.Should().Contain("public class SaleOrderOdooModel : IOdooModel");
            code.Should().Contain("[JsonProperty(\"id\")]").And.Contain("public long Id { get; set; }");
            code.Should().Contain("public string Name { get; set; }");
            code.Should().Contain("public long? PartnerId { get; set; }");
            code.Should().Contain("public StatusSaleOrderOdooEnum? State { get; set; }");
            code.Should().Contain("/// partner_id - many2one - res.partner (x) <br />");
            code.Should().Contain("Help: Line 1; Line 2");
            code.Should().Contain("public enum StatusSaleOrderOdooEnum");
            code.Should().Contain("[EnumMember(Value = \"draft\")]").And.Contain("Quotation = 1,");
            code.Should().Contain("[EnumMember(Value = \"sale\")]").And.Contain("SalesOrder = 2,");
        }

        [Fact]
        public void Generated_model_without_summary_has_no_comments()
        {
            var fields = new Dictionary<string, OdooPropertyInfo> { ["name"] = new OdooPropertyInfo { Type = "char", String = "Name", Help = "help" } };

            var code = OdooModelMapper.GetDotNetModel("res.partner", fields, addSummary: false);

            code.Should().NotContain("///");
            code.Should().Contain("public string Name { get; set; }");
        }

        [Fact]
        public void Unknown_field_type_throws()
        {
            var property = new KeyValuePair<string, OdooPropertyInfo>("x", new OdooPropertyInfo { Type = "char" });
            property.Value.Type = "not_existing";

            Action act = () => OdooModelMapper.ConvertToDotNetPropertyTypeName(property, "res.partner");

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void Enum_value_is_read_from_enum_member()
        {
            var field = typeof(TestPartnerTypeOdooEnum).GetField(nameof(TestPartnerTypeOdooEnum.InvoiceAddress));

            OdooModelMapper.GetOdooEnumName(field).Should().Be("invoice");
            OdooModelMapper.ConvertToDotNetEnum(typeof(TestPartnerTypeOdooEnum), "contact").Should().Be(TestPartnerTypeOdooEnum.Contact);
        }

        [Fact]
        public void Enum_without_enum_member_throws()
        {
            var field = typeof(BindingFlags).GetField(nameof(BindingFlags.Public));

            Action act = () => OdooModelMapper.GetOdooEnumName(field);

            act.Should().Throw<ArgumentException>().WithMessage("*EnumMemberAttribute*");
        }

        [Fact]
        public void Enum_members_get_unique_valid_names()
        {
            var fields = new Dictionary<string, OdooPropertyInfo>
            {
                // issue #50: labels giving the same name, issue #122: labels starting with a digit
                ["tz"] = new OdooPropertyInfo { Type = "selection", String = "Timezone", Selection = new[] { new[] { "Etc/GMT-0", "Etc/GMT-0" }, new[] { "Etc/GMT0", "Etc/GMT0" }, new[] { "Etc/GMT+0", "Etc/GMT+0" } } },
                ["repeat_day"] = new OdooPropertyInfo { Type = "selection", String = "Repeat Day", Selection = new[] { new[] { "1", "1" }, new[] { "2", "2" } } },
            };

            var code = OdooModelMapper.GetDotNetModel("project.task", fields);

            code.Should().Contain("EtcGMT0 = 1,").And.Contain("EtcGMT02 = 2,").And.Contain("EtcGmtplus0 = 3,");
            code.Should().Contain("[EnumMember(Value = \"Etc/GMT0\")]");
            code.Should().Contain("Value1 = 1,").And.Contain("Value2 = 2,");
            code.Should().Contain("[EnumMember(Value = \"1\")]");
        }
    }
}
