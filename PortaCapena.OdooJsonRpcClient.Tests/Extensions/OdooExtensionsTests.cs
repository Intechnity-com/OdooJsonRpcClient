using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Result;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Extensions
{
    public class OdooExtensionsTests
    {
        public enum EnumWithoutAttributes
        {
            [Description("described")]
            Described,
            Plain
        }

        [Fact]
        public void Description_reads_description_attribute_or_falls_back_to_name()
        {
            EnumWithoutAttributes.Described.Description().Should().Be("described");
            EnumWithoutAttributes.Plain.Description().Should().Be("Plain");
            OdooOperator.NotIn.Description().Should().Be("not in");
        }

        [Fact]
        public void OdooValue_reads_enum_member()
        {
            TestPartnerTypeOdooEnum.InvoiceAddress.OdooValue().Should().Be("invoice");
        }

        [Fact]
        public void OdooValue_without_enum_member_throws()
        {
            Action act = () => EnumWithoutAttributes.Plain.OdooValue();

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Table_name_is_read_from_attribute()
        {
            new TestPartnerOdooModel().OdooTableName().Should().Be("test.partner");
            OdooExtensions.GetOdooTableName<TestCountryOdooModel>().Should().Be("test.country");
        }

        [Fact]
        public void Missing_table_name_attribute_throws()
        {
            Action instance = () => new ModelWithoutJsonProperty().OdooTableName();
            Action generic = () => OdooExtensions.GetOdooTableName<ModelWithoutJsonProperty>();

            instance.Should().Throw<ArgumentException>().WithMessage("*OdooTableNameAttribute*");
            generic.Should().Throw<ArgumentException>().WithMessage("*OdooTableNameAttribute*");
        }

        [Fact]
        public void Odoo_property_name_by_name_and_expression()
        {
            var model = new TestPartnerOdooModel();

            model.OdooPropertyName(nameof(TestPartnerOdooModel.CountryId)).Should().Be("country_id");
            model.OdooPropertyName(x => x.CreditLimit).Should().Be("credit_limit");
            OdooExtensions.GetOdooPropertyName<TestPartnerOdooModel>(nameof(TestPartnerOdooModel.Computed)).Should().BeNull();
        }

        [Fact]
        public void Odoo_property_name_of_not_existing_property_throws()
        {
            Action act = () => OdooExtensions.GetOdooPropertyName<TestPartnerOdooModel>("NotExisting");
            Action nullName = () => OdooExtensions.GetOdooPropertyName<TestPartnerOdooModel>(null);

            act.Should().Throw<ArgumentException>().WithMessage("*NotExisting*");
            nullName.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Dates_are_formatted_for_odoo_regardless_of_culture()
        {
            var original = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("fa-IR");
                var date = new DateTime(2024, 3, 4, 5, 6, 7);

                date.ToOdooDateTimeString().Should().Be("2024-03-04 05:06:07");
                date.ToOdooDateString().Should().Be("2024-03-04");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = original;
            }
        }

        [Fact]
        public void ToResult_keeps_id_and_changes_value()
        {
            var source = new OdooResult<long[]> { Id = 5, Jsonrpc = "2.0", Value = new long[] { 1 } };

            var result = source.ToResult("x");

            result.Id.Should().Be(5);
            result.Jsonrpc.Should().Be("2.0");
            result.Value.Should().Be("x");
            result.Succeed.Should().BeTrue();
        }
    }
}
