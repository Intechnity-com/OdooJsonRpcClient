using System;
using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Models
{
    public class OdooPropertyInfoTests
    {
        [Theory]
        [InlineData("binary", OdooValueTypeEnum.Binary)]
        [InlineData("boolean", OdooValueTypeEnum.Boolean)]
        [InlineData("char", OdooValueTypeEnum.Char)]
        [InlineData("json", OdooValueTypeEnum.Char)]
        [InlineData("date", OdooValueTypeEnum.Date)]
        [InlineData("datetime", OdooValueTypeEnum.Datetime)]
        [InlineData("float", OdooValueTypeEnum.Float)]
        [InlineData("integer", OdooValueTypeEnum.Integer)]
        [InlineData("many2many", OdooValueTypeEnum.Many2Many)]
        [InlineData("many2one", OdooValueTypeEnum.Many2One)]
        [InlineData("many2one_reference", OdooValueTypeEnum.Many2OneReference)]
        [InlineData("one2many", OdooValueTypeEnum.One2Many)]
        [InlineData("selection", OdooValueTypeEnum.Selection)]
        [InlineData("text", OdooValueTypeEnum.Text)]
        [InlineData("html", OdooValueTypeEnum.Html)]
        [InlineData("related", OdooValueTypeEnum.Reference)]
        [InlineData("one2one", OdooValueTypeEnum.One2One)]
        [InlineData("monetary", OdooValueTypeEnum.Monetary)]
        [InlineData("properties", OdooValueTypeEnum.Properties)]
        [InlineData("properties_definition", OdooValueTypeEnum.PropertiesDefinition)]
        public void Odoo_field_type_is_mapped(string odooType, OdooValueTypeEnum expected)
        {
            OdooPropertyInfo.ToOdooValueTypeEnum(odooType).Should().Be(expected);
            new OdooPropertyInfo { Type = odooType }.PropertyValueType.Should().Be(expected);
        }

        [Fact]
        public void Unknown_field_type_throws()
        {
            Action act = () => OdooPropertyInfo.ToOdooValueTypeEnum("unknown_type");

            act.Should().Throw<Exception>().WithMessage("*unknown_type*");
        }

        [Fact]
        public void Fields_get_answer_is_deserialized()
        {
            var json = @"{
                ""type"": ""selection"", ""string"": ""Status"", ""help"": ""Help text"", ""required"": true, ""readonly"": true,
                ""store"": true, ""sortable"": true, ""searchable"": true, ""manual"": false, ""company_dependent"": false, ""change_default"": false,
                ""selection"": [[""draft"", ""Draft""], [""done"", ""Done""]], ""depends"": [""a"", ""b""], ""groups"": ""base.group_user"",
                ""translate"": false, ""trim"": true, ""relation"": ""res.partner"", ""relation_field"": ""partner_id"", ""related"": ""x.y"",
                ""group_operator"": ""sum"", ""digits"": [16, 2], ""attachment"": false, ""unknown_key"": 1
            }";

            var info = JsonConvert.DeserializeObject<OdooPropertyInfo>(json);

            info.PropertyValueType.Should().Be(OdooValueTypeEnum.Selection);
            info.String.Should().Be("Status");
            info.Help.Should().Be("Help text");
            info.ResultRequired.Should().BeTrue();
            info.Readonly.Should().BeTrue();
            info.Store.Should().BeTrue();
            info.Sortable.Should().BeTrue();
            info.Searchable.Should().BeTrue();
            info.Selection.Should().HaveCount(2);
            info.Selection[1].Should().Equal("done", "Done");
            info.Depends.Should().Equal("a", "b");
            info.Groups.Should().Be("base.group_user");
            info.Trim.Should().BeTrue();
            info.Relation.Should().Be("res.partner");
            info.RelationField.Should().Be("partner_id");
            info.Related.Should().Be("x.y");
            info.GroupOperator.Should().Be("sum");
            info.Digits.Should().Equal(16, 2);
        }
    }
}
