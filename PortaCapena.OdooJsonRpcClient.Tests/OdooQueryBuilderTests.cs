using System;
using System.Collections;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooQueryTests
    {
        [Fact]
        public void Can_create_builder_with_static_method()
        {
            var filters = OdooQuery<ProductProductOdooDto>.Create();

            filters.Should().NotBeNull();
            filters.Filters.Count.Should().Be(0);
            filters.ReturnFields.Count.Should().Be(0);
            filters.Limit.Should().BeNull();
            filters.Offset.Should().BeNull();
        }


        [Fact]
        public void When_use_select_shoud_return_filds_with_odoo_prop_names()
        {
            var filters = OdooQuery<ProductProductOdooDto>.Create()
                .Select(x => new
                {
                    x.Name,
                    x.Description
                });

            filters.ReturnFields.Count.Should().Be(2);
            filters.ReturnFields.First().Should().Be("name");
            filters.ReturnFields.ElementAt(1).Should().Be("description");

            filters.Filters.Count.Should().Be(0);
            filters.Limit.Should().BeNull();
            filters.Offset.Should().BeNull();
        }

        [Fact]
        public void When_use_pridicate_where_shoud_return_correct_filters_model()
        {
            var filters = OdooQuery<ProductProductOdooDto>.Create()
                .Where(x => x.Id, OdooOperator.EqualsTo, 10);

            filters.Filters.Count.Should().Be(1);
            var json = JsonConvert.SerializeObject(filters.Filters);

            json.Should().Be("[[\"id\",\"=\",10]]");

            filters.ReturnFields.Count.Should().Be(0);
            filters.Limit.Should().BeNull();
            filters.Offset.Should().BeNull();
        }


        [Fact]
        public void When_use_pridicate_where_with_two_times_shoud_return_correct_filters_model()
        {
            var filters = OdooQuery<ProductProductOdooDto>.Create()
                .Where(x => x.WriteDate, OdooOperator.GreaterThanOrEqualTo, new DateTime(2020, 12, 2))
                .Where(x => x.Name, OdooOperator.EqualsTo, "Bioboxen 610l")
                ;

            filters.Filters.Count.Should().Be(2);
            var json = JsonConvert.SerializeObject(filters.Filters);

            json.Should().Be("[[\"write_date\",\">=\",\"2020-12-02T00:00:00\"],[\"name\",\"=\",\"Bioboxen 610l\"]]");

            filters.ReturnFields.Count.Should().Be(0);
            filters.Limit.Should().BeNull();
            filters.Offset.Should().BeNull();
        }


        [Fact]
        public void When_use_where_with_OdooFilter_param_shoud_return_correct_filters_model()
        {

            var filters = OdooQuery<ProductProductOdooDto>.Create()
                .Where(OdooFilter.Create().GreaterThanOrEqual("write_date", new DateTime(2020, 12, 2)))
                .Where(OdooFilter.Create().EqualTo("name", "Bioboxen 610l"));

            filters.Filters.Count.Should().Be(2);
            var json = JsonConvert.SerializeObject(filters.Filters);

            json.Should().Be("[[\"write_date\",\">=\",\"2020-12-02T00:00:00\"],[\"name\",\"=\",\"Bioboxen 610l\"]]");

            filters.ReturnFields.Count.Should().Be(0);
            filters.Limit.Should().BeNull();
            filters.Offset.Should().BeNull();
        }


        [Fact]
        public void When_use_where_with_complex_OdooFilter_param_shoud_return_correct_filters_model()
        {
            var filters = OdooQuery<ProductProductOdooDto>.Create()
                .Where(OdooFilter.Create()
                    .GreaterThanOrEqual("write_date", new DateTime(2020, 12, 2))
                    .And()
                    .EqualTo("name", "Bioboxen 610l"));

            var json = JsonConvert.SerializeObject(filters.Filters);
            json.Should().Be("[[\"write_date\",\">=\",\"2020-12-02T00:00:00\"],\"&\",[\"name\",\"=\",\"Bioboxen 610l\"]]");

            filters.ReturnFields.Count.Should().Be(0);
            filters.Limit.Should().BeNull();
            filters.Offset.Should().BeNull();
        }


        [Fact]
        public void When_use_select_with_anymous_model_shoud_return_correct_filters_model()
        {
            var filters = OdooQuery<ProductProductOdooDto>.Create()
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.WriteDate
                });

            filters.Limit.Should().BeNull();
            filters.Offset.Should().BeNull();
            filters.Filters.Count.Should().Be(0);

            filters.ReturnFields.Count.Should().Be(3);

            filters.ReturnFields.First().Should().Be("name");
            filters.ReturnFields.Skip(1).First().Should().Be("description");
            filters.ReturnFields.Skip(2).First().Should().Be("write_date");
        }

        [Fact]
        public void When_use_pridicate_where_with_one_forgin_key_shoud_return_correct_filters_model()
        {
            var filters = OdooQuery<ProductProductOdooDto>.Create()
                    .Where<ResCompanyOdooModel>(x => x.CompanyId, x => x.CountryCode, OdooOperator.EqualsTo, "BE");

            filters.Filters.Count.Should().Be(1);
            filters.Filters[0].Should().NotBeNull();

            var arr = filters.Filters[0] as ArrayList;
            arr[0].Should().Be("company_id.country_code");
            arr[1].Should().Be("=");
            arr[2].Should().Be("BE");

            var json = JsonConvert.SerializeObject(filters.Filters);
            json.Should().Be("[[\"company_id.country_code\",\"=\",\"BE\"]]");

            filters.ReturnFields.Count.Should().Be(0);
            filters.Limit.Should().BeNull();
            filters.Offset.Should().BeNull();
        }

        [Fact]
        public void When_use_pridicate_where_with_two_forgin_key_shoud_return_correct_filters_model()
        {
            var filters = OdooQuery<ProductProductOdooDto>.Create()
                .Where<ResCompanyOdooModel, AccountTaxOdooModel>(x => x.PropertyAccountExpenseId, x => x.AccountSaleTaxId, x => x.CountryCode, OdooOperator.EqualsTo, "BE");

            filters.Filters.Count.Should().Be(1);
            filters.Filters[0].Should().NotBeNull();

            var arr = filters.Filters[0] as ArrayList;
            arr[0].Should().Be("property_account_expense_id.account_sale_tax_id.country_code");
            arr[1].Should().Be("=");
            arr[2].Should().Be("BE");

            var json = JsonConvert.SerializeObject(filters.Filters);
            json.Should().Be("[[\"property_account_expense_id.account_sale_tax_id.country_code\",\"=\",\"BE\"]]");

            filters.ReturnFields.Count.Should().Be(0);
            filters.Limit.Should().BeNull();
            filters.Offset.Should().BeNull();
        }
    }
}