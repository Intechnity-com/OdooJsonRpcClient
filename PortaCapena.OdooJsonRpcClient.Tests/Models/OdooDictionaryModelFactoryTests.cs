using System;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Models
{
    public class OdooDictionaryModelFactoryTests
    {
        [Fact]
        public void Create_without_table_name()
        {
            var model = OdooDictionaryModel.Create();

            model.TableName.Should().BeNull();
            model.Should().BeEmpty();
        }

        [Fact]
        public void Create_with_table_name()
        {
            OdooDictionaryModel.Create("res.partner").TableName.Should().Be("res.partner");
            new OdooDictionaryModel("res.country").TableName.Should().Be("res.country");
        }

        [Fact]
        public void Create_typed_takes_table_name_from_attribute()
        {
            OdooDictionaryModel.Create<TestPartnerOdooModel>().TableName.Should().Be("test.partner");
            new OdooDictionaryModel<TestCountryOdooModel>().TableName.Should().Be("test.country");
        }

        [Fact]
        public void Typed_model_without_table_attribute_throws()
        {
            Action act = () => new OdooDictionaryModel<ModelWithoutJsonProperty>();

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Create_with_one_property()
        {
            var model = OdooDictionaryModel.Create<TestPartnerOdooModel>(x => x.CountryId, 20L);
            var typed = OdooDictionaryModel<TestPartnerOdooModel>.Create(x => x.Color, 3);

            model.TableName.Should().Be("test.partner");
            model["country_id"].Should().Be(20L);
            typed["color"].Should().Be(3);
        }

        [Fact]
        public void Typed_create_from_initializer()
        {
            var model = OdooDictionaryModel<TestPartnerOdooModel>.Create(() => new TestPartnerOdooModel { Name = "A", IsCompany = true });

            model.TableName.Should().Be("test.partner");
            model.Should().HaveCount(2);
            model["is_company"].Should().Be(true);
        }

        [Fact]
        public void Add_chains_and_overrides_values()
        {
            var model = OdooDictionaryModel<TestPartnerOdooModel>
                .Create(x => x.Name, "first")
                .Add(x => x.Name, "second")
                .Add(() => new TestPartnerOdooModel { Color = 2 });

            model["name"].Should().Be("second");
            model["color"].Should().Be(2);
        }

        [Fact]
        public void Add_initializer_to_untyped_model_sets_table_name()
        {
            var model = new OdooDictionaryModel().Add(() => new TestPartnerOdooModel { Name = "A" });

            model.TableName.Should().Be("test.partner");
            model["name"].Should().Be("A");
        }

        [Fact]
        public void Add_with_property_without_json_property_throws()
        {
            Action act = () => new OdooDictionaryModel().Add<TestPartnerOdooModel>(x => x.Computed, "a");

            act.Should().Throw<ArgumentException>().WithMessage("*Computed*");
        }
    }
}
