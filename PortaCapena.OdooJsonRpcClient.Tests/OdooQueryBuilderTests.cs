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
        public void Builder_shoud_create_and_build()
        {
            var filters = OdooQuery<OdooProductProduct>.Create();

            filters.Should().NotBeNull();
            filters.Filters.Count.Should().Be(0);
            filters.ReturnFields.Count.Should().Be(0);
            filters.Limit.Should().BeNull();
            filters.Offset.Should().BeNull();
        }


        [Fact]
        public void When_use_select_shoud_return_filds()
        {
            var filters = OdooQuery<OdooProductProduct>.Create()
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
        public void When_use_pridicate_where_shoud_return_filters()
        {
            var filters = OdooQuery<OdooProductProduct>.Create()
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
        public void When_use_pridicate_where_shoud_return_filters2()
        {
            var filters = OdooQuery<OdooProductProduct>.Create()
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
        public void When_use_where_shoud_return_filters()
        {

            var filters = OdooQuery<OdooProductProduct>.Create()
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
        public void When_use_where_shoud_return_filters2()
        {
            var filters = OdooQuery<OdooProductProduct>.Create()
                .Where(OdooFilter.Create().GreaterThanOrEqual("write_date", new DateTime(2020, 12, 2)).And().EqualTo("name", "Bioboxen 610l"))
                ;

            var json = JsonConvert.SerializeObject(filters.Filters);
            json.Should().Be("[[\"write_date\",\">=\",\"2020-12-02T00:00:00\"],\"&\",[\"name\",\"=\",\"Bioboxen 610l\"]]");

            filters.ReturnFields.Count.Should().Be(0);
            filters.Limit.Should().BeNull();
            filters.Offset.Should().BeNull();
        }
    }
}