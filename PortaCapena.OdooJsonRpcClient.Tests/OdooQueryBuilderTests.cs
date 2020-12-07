using System;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Tests.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooQueryBuilderTests
    {
        [Fact]
        public void Builder_shoud_create_and_build()
        {
            var filters = OdooQueryBuilder<OdooProductProduct>.Create().Build();

            filters.Should().NotBeNull();
            filters.Filters.Count.Should().Be(0);
            filters.ReturnFields.Count.Should().Be(0);
            filters.Take.Should().BeNull();
            filters.Skip.Should().BeNull();
        }


        [Fact]
        public void When_use_select_shoud_return_filds()
        {
            var filters = OdooQueryBuilder<OdooProductProduct>.Create()
                .Select(x => new
                {
                    x.Name,
                    x.Description
                })
                .Build();

            filters.ReturnFields.Count.Should().Be(2);
            filters.ReturnFields.First().Should().Be("name");
            filters.ReturnFields.ElementAt(1).Should().Be("description");

            filters.Filters.Count.Should().Be(0);
            filters.Take.Should().BeNull();
            filters.Skip.Should().BeNull();
        }

        [Fact]
        public void When_use_pridicate_where_shoud_return_filters()
        {
            var filters = OdooQueryBuilder<OdooProductProduct>.Create()
                .Where(x => x.Id, OdooOperator.EqualsTo, 10)
                .Build();

            filters.Filters.Count.Should().Be(1);
            var json = JsonConvert.SerializeObject(filters.Filters);

            json.Should().Be("[[\"id\",\"=\",10]]");

            filters.ReturnFields.Count.Should().Be(0);
            filters.Take.Should().BeNull();
            filters.Skip.Should().BeNull();
        }


        [Fact]
        public void When_use_pridicate_where_shoud_return_filters2()
        {
            var filters = OdooQueryBuilder<OdooProductProduct>.Create()
                .Where(x => x.WriteDate, OdooOperator.GreaterThanOrEqualTo, new DateTime(2020, 12, 2))
                .Where(x => x.Name, OdooOperator.EqualsTo, "Bioboxen 610l")
                .Build();

            filters.Filters.Count.Should().Be(2);
            var json = JsonConvert.SerializeObject(filters.Filters);

            json.Should().Be("[[\"write_date\",\">=\",\"2020-12-02T00:00:00\"],[\"name\",\"=\",\"Bioboxen 610l\"]]");

            filters.ReturnFields.Count.Should().Be(0);
            filters.Take.Should().BeNull();
            filters.Skip.Should().BeNull();
        }


        [Fact]
        public void When_use_where_shoud_return_filters()
        {

            var filters = OdooQueryBuilder<OdooProductProduct>.Create()
                .Where(nameof(OdooProductProduct.WriteDate), OdooOperator.GreaterThanOrEqualTo, new DateTime(2020, 12, 2))
                .Where(nameof(OdooProductProduct.Name), OdooOperator.EqualsTo, "Bioboxen 610l")
                .Build();

            filters.Filters.Count.Should().Be(2);
            var json = JsonConvert.SerializeObject(filters.Filters);

            json.Should().Be("[[\"write_date\",\">=\",\"2020-12-02T00:00:00\"],[\"name\",\"=\",\"Bioboxen 610l\"]]");

            filters.ReturnFields.Count.Should().Be(0);
            filters.Take.Should().BeNull();
            filters.Skip.Should().BeNull();
        }


        [Fact]
        public void When_use_where_shoud_return_filters2()
        {
            var filters = OdooQueryBuilder<OdooProductProduct>.Create()
                  .Where(new object[]
                  {
                      nameof(OdooProductProduct.WriteDate),
                      OdooOperator.GreaterThanOrEqualTo,
                      new DateTime(2020, 12, 2)
                  },
                      OdooOperator.And,
                      new object[]
                  {
                      nameof(OdooProductProduct.Name),
                      OdooOperator.EqualsTo,
                      "Bioboxen 610l"
                  })
                .Build();

            filters.Filters.Count.Should().Be(1);
            var json = JsonConvert.SerializeObject(filters.Filters);

            json.Should().Be("[[[\"write_date\",\">=\",\"2020-12-02T00:00:00\"],\"&\",[\"name\",\"=\",\"Bioboxen 610l\"]]]");

            filters.ReturnFields.Count.Should().Be(0);
            filters.Take.Should().BeNull();
            filters.Skip.Should().BeNull();
        }
    }
}