using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Request
{
    public class OdooFilterTests
    {
        private static string Json(OdooFilter filter) => JsonConvert.SerializeObject(filter);

        [Theory]
        [MemberData(nameof(SimpleOperators))]
        public void Simple_operators_produce_odoo_domain(Func<OdooFilter, OdooFilter> build, string expected)
        {
            Json(build(OdooFilter.Create())).Should().Be(expected);
        }

        public static IEnumerable<object[]> SimpleOperators()
        {
            object[] Case(Func<OdooFilter, OdooFilter> f, string expected) => new object[] { f, expected };

            yield return Case(f => f.EqualTo("name", "a"), "[[\"name\",\"=\",\"a\"]]");
            yield return Case(f => f.NotEqual("name", "a"), "[[\"name\",\"!=\",\"a\"]]");
            yield return Case(f => f.Like("name", "a"), "[[\"name\",\"like\",\"a\"]]");
            yield return Case(f => f.NotLike("name", "a"), "[[\"name\",\"not like\",\"a\"]]");
            yield return Case(f => f.ILike("name", "a"), "[[\"name\",\"ilike\",\"a\"]]");
            yield return Case(f => f.GreaterThan("qty", 1), "[[\"qty\",\">\",1]]");
            yield return Case(f => f.GreaterThanOrEqual("qty", 1), "[[\"qty\",\">=\",1]]");
            yield return Case(f => f.LessThan("qty", 1), "[[\"qty\",\"<\",1]]");
            yield return Case(f => f.LessThanOrEqual("qty", 1), "[[\"qty\",\"<=\",1]]");
            yield return Case(f => f.In("id", new long[] { 1, 2 }), "[[\"id\",\"in\",[1,2]]]");
            yield return Case(f => f.In("id", new object[] { 1, "x" }), "[[\"id\",\"in\",[1,\"x\"]]]");
            yield return Case(f => f.In("id", new List<object> { 1 }), "[[\"id\",\"in\",[1]]]");
            yield return Case(f => f.In("id", new OdooFilterValue().AddValue(1).AddValue(2)), "[[\"id\",\"in\",[1,2]]]");
            yield return Case(f => f.NotIn("id", new long[] { 1 }), "[[\"id\",\"not in\",[1]]]");
            yield return Case(f => f.NotBeNull("parent_id"), "[[\"parent_id\",\"!=\",false]]");
            yield return Case(f => f.BeNull("parent_id"), "[[\"parent_id\",\"=\",false]]");
        }

        [Fact]
        public void Between_produces_two_inclusive_conditions_joined_with_and()
        {
            var filter = OdooFilter.Create().Between("amount", 10, 20);

            Json(filter).Should().Be("[\"&\",[\"amount\",\">=\",10],[\"amount\",\"<=\",20]]");
        }

        [Fact]
        public void Between_can_be_combined_with_other_conditions()
        {
            var filter = OdooFilter.Create().Or().EqualTo("state", "draft").Between("amount", 1, 2);

            Json(filter).Should().Be("[\"|\",[\"state\",\"=\",\"draft\"],\"&\",[\"amount\",\">=\",1],[\"amount\",\"<=\",2]]");
        }

        [Fact]
        public void ChildOf_and_ParentOf_produce_valid_domain()
        {
            Json(OdooFilter.Create().ChildOf("categ_id", "All")).Should().Be("[[\"categ_id\",\"child_of\",\"All\"]]");
            Json(OdooFilter.Create().ChildOf("categ_id", 5)).Should().Be("[[\"categ_id\",\"child_of\",5]]");
            Json(OdooFilter.Create().ParentOf("categ_id", new[] { 5, 6 })).Should().Be("[[\"categ_id\",\"parent_of\",[5,6]]]");
        }

        [Fact]
        public void Logical_operators_are_prefix()
        {
            var filter = OdooFilter.Create().Or().EqualTo("a", 1).And().Not().EqualTo("b", 2).EqualTo("c", 3);

            Json(filter).Should().Be("[\"|\",[\"a\",\"=\",1],\"&\",\"!\",[\"b\",\"=\",2],[\"c\",\"=\",3]]");
        }

        [Fact]
        public void Generic_filter_maps_property_names()
        {
            var filter = OdooFilter<TestPartnerOdooModel>.Create()
                .Or()
                .EqualTo(x => x.Name, "a")
                .In(x => x.CountryId, new long[] { 1 })
                .Between(x => x.CreditLimit, 1, 2)
                .ChildOf(x => x.ParentId, 3);

            Json(filter).Should().Be("[\"|\",[\"name\",\"=\",\"a\"],[\"country_id\",\"in\",[1]],\"&\",[\"credit_limit\",\">=\",1],[\"credit_limit\",\"<=\",2],[\"parent_id\",\"child_of\",3]]");
        }

        [Fact]
        public void Generic_logical_operators_keep_generic_type_for_chaining()
        {
            OdooFilter<TestPartnerOdooModel> filter = OdooFilter<TestPartnerOdooModel>.Create().Or().Not().And();

            filter.Count.Should().Be(3);
        }

        [Fact]
        public void Generic_filter_on_property_without_json_property_throws_meaningful_error()
        {
            Action act = () => OdooFilter<TestPartnerOdooModel>.Create().EqualTo(x => x.Computed, "a");

            act.Should().Throw<ArgumentException>().WithMessage("*Computed*JsonProperty*");
        }

        [Fact]
        public void Generic_filter_with_non_property_expression_throws_meaningful_error()
        {
            Action act = () => OdooFilter<TestPartnerOdooModel>.Create().EqualTo(x => x.Name.Length, 1);

            act.Should().Throw<ArgumentException>();
        }
    }
}
