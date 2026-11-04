using System;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooDictionaryModelBehaviourTests
    {
        [Fact]
        public void Add_with_expression_sets_table_name_when_missing()
        {
            var model = new OdooDictionaryModel().Add<TestPartnerOdooModel>(x => x.Name, "a");

            model.TableName.Should().Be("test.partner");
            model["name"].Should().Be("a");
        }

        [Fact]
        public void Add_with_expression_keeps_existing_table_name()
        {
            var model = OdooDictionaryModel.Create("custom.table").Add<TestPartnerOdooModel>(x => x.Name, "a");

            model.TableName.Should().Be("custom.table");
        }

        [Fact]
        public void Create_from_initializer_evaluates_any_expression()
        {
            var suffix = "x";
            var model = OdooDictionaryModel.Create(() => new TestPartnerOdooModel
            {
                Name = "a" + suffix,                                  // binary expression
                Color = DateTime.Now.Year > 2000 ? 1 : 2,             // conditional
                CountryId = null,
                Type = TestPartnerTypeOdooEnum.Contact,
                CategoryId = new long[] { 1, 2 },
            });

            model.TableName.Should().Be("test.partner");
            model["name"].Should().Be("ax");
            model["color"].Should().Be(1);
            model["country_id"].Should().BeNull();
            model["type"].Should().Be(TestPartnerTypeOdooEnum.Contact);
            model["category_id"].Should().BeEquivalentTo(new long[] { 1, 2 });
        }

        [Fact]
        public void Create_from_non_initializer_throws()
        {
            var existing = new TestPartnerOdooModel();
            Action variable = () => OdooDictionaryModel.Create(() => existing);
            Action constructor = () => OdooDictionaryModel.Create(() => new TestPartnerOdooModel());

            variable.Should().Throw<ArgumentException>().WithMessage("*object initializer*");
            constructor.Should().Throw<ArgumentException>().WithMessage("*object initializer*");
        }

        [Fact]
        public void Property_without_json_property_throws_meaningful_error()
        {
            Action act = () => OdooDictionaryModel.Create(() => new TableModelWithoutJsonProperty { NotMapped = "a" });

            act.Should().Throw<ArgumentException>().WithMessage("*NotMapped*JsonProperty*");
        }
    }
}
