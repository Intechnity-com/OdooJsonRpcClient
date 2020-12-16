using System;
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
                .Where(x => x.Id, OdooOperator.EqualsTo, 10)
                ;

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
                .Where(OdooFilter.Create().EqualTo("name", "Bioboxen 610l"))
                ;

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
    }
}