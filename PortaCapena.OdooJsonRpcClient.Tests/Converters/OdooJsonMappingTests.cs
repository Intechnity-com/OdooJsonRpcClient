using System;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Converters
{
    public class OdooJsonMappingTests
    {
        private static TestPartnerOdooModel Map(string json) => JsonConvert.DeserializeObject<TestPartnerOdooModel>(json);

        [Fact]
        public void False_means_empty_value()
        {
            var model = Map("{\"name\":false,\"country_id\":false,\"create_date\":false,\"type\":false,\"credit_limit\":false,\"raw\":false}");

            model.Name.Should().BeNull();
            model.CountryId.Should().BeNull();
            model.CreateDate.Should().BeNull();
            model.Type.Should().BeNull();
            model.CreditLimit.Should().BeNull();
            model.Raw.Should().BeNull();
        }

        [Fact]
        public void Null_values_are_ignored()
        {
            var model = Map("{\"name\":null,\"country_id\":null,\"category_id\":null,\"properties\":null}");

            model.Name.Should().BeNull();
            model.CountryId.Should().BeNull();
            model.CategoryId.Should().BeNull();
        }

        [Fact]
        public void Many2one_takes_id()
        {
            Map("{\"country_id\":[20,\"Poland\"]}").CountryId.Should().Be(20);
        }

        [Fact]
        public void Many2many_and_empty_list()
        {
            Map("{\"category_id\":[1,2,3]}").CategoryId.Should().Equal(1, 2, 3);
            Map("{\"category_id\":[]}").CategoryId.Should().BeEmpty();
        }

        [Fact]
        public void Integer_values_map_to_floating_point_properties()
        {
            var model = Map("{\"credit_limit\":5,\"balance\":7,\"color\":3}");

            model.CreditLimit.Should().Be(5d);
            model.Balance.Should().Be(7m);
            model.Color.Should().Be(3);
        }

        [Fact]
        public void Float_values_map_to_numeric_properties()
        {
            var model = Map("{\"credit_limit\":5.25,\"balance\":10.10}");

            model.CreditLimit.Should().Be(5.25d);
            model.Balance.Should().Be(10.10m);
        }

        [Theory]
        [InlineData("pl-PL")]
        [InlineData("en-US")]
        [InlineData("ar-SA")]
        [InlineData("fa-IR")]
        public void Dates_are_parsed_independently_of_current_culture(string culture)
        {
            var original = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

                Map("{\"create_date\":\"2024-03-04 05:06:07\"}").CreateDate.Should().Be(new DateTime(2024, 3, 4, 5, 6, 7));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = original;
            }
        }

        [Fact]
        public void Selection_maps_to_enum_by_enum_member_value()
        {
            Map("{\"type\":\"invoice\"}").Type.Should().Be(TestPartnerTypeOdooEnum.InvoiceAddress);
        }

        [Fact]
        public void Unknown_selection_value_throws_meaningful_error()
        {
            Action act = () => Map("{\"type\":\"unknown\"}");

            act.Should().Throw<ArgumentException>().WithMessage("*unknown*");
        }

        [Fact]
        public void Json_objects_and_lists_map_to_string_property()
        {
            Map("{\"properties\":{\"a\":1}}").Properties.Should().Be("{\"a\":1}");
            Map("{\"properties\":[{\"name\":\"x\"}]}").Properties.Should().Be("[{\"name\":\"x\"}]");
        }

        [Fact]
        public void List_of_strings_maps_to_dotted_string_as_before()
        {
            Map("{\"name\":[\"a\",\"b\"]}").Name.Should().Be("a.b");
        }

        [Fact]
        public void Object_property_gets_raw_json()
        {
            var model = Map("{\"raw\":{\"x\":[1,2]}}");

            model.Raw.Should().BeAssignableTo<JToken>();
            ((JToken)model.Raw)["x"][1].Value<int>().Should().Be(2);
        }

        [Fact]
        public void Unknown_fields_are_ignored()
        {
            Map("{\"id\":1,\"not_in_model\":123}").Id.Should().Be(1);
        }

        [Fact]
        public void Model_without_json_property_attribute_throws()
        {
            Action act = () => JsonConvert.DeserializeObject<ModelWithoutJsonProperty>("{\"id\":1}");

            act.Should().Throw<ArgumentException>().WithMessage("*NotMapped*");
        }

        [Fact]
        public void Null_model_in_array_is_null()
        {
            JsonConvert.DeserializeObject<TestPartnerOdooModel[]>("[null,{\"id\":1}]").Should().HaveCount(2).And.ContainSingle(x => x == null);
        }

        [Theory]
        [InlineData("res.partner", "ResPartner")]
        [InlineData("x_studio_field", "XStudioField")]
        [InlineData("Price +10%", "PricePlus10")]
        [InlineData("___", "None")]
        public void Convert_odoo_name_to_dotnet(string odooName, string expected)
        {
            OdooModelMapper.ConvertOdooNameToDotNet(odooName).Should().Be(expected);
        }

        [Fact]
        public void Many2one_with_text_id_takes_numeric_id()
        {
            // account.root in Odoo 18+ has text ids: account_root_id = ["60", "60"]
            Map("{\"country_id\":[\"60\",\"60\"]}").CountryId.Should().Be(60);
        }
    }
}
