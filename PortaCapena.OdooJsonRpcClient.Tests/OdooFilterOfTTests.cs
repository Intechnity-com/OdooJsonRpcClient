using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooFilterOfTTests
    {
        public static IEnumerable<object[]> Operators()
        {
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.EqualTo(x => x.Name, "a")), "[[\"name\",\"=\",\"a\"]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.NotEqual(x => x.Name, "a")), "[[\"name\",\"!=\",\"a\"]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.Like(x => x.Name, "a")), "[[\"name\",\"like\",\"a\"]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.NotLike(x => x.Name, "a")), "[[\"name\",\"not like\",\"a\"]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.ILike(x => x.Name, "a")), "[[\"name\",\"ilike\",\"a\"]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.GreaterThan(x => x.Color, 1)), "[[\"color\",\">\",1]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.GreaterThanOrEqual(x => x.Color, 1)), "[[\"color\",\">=\",1]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.LessThan(x => x.Color, 1)), "[[\"color\",\"<\",1]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.LessThanOrEqual(x => x.Color, 1)), "[[\"color\",\"<=\",1]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.In(x => x.Id, new long[] { 1, 2 })), "[[\"id\",\"in\",[1,2]]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.In(x => x.Name, new object[] { "a", "b" })), "[[\"name\",\"in\",[\"a\",\"b\"]]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.In(x => x.Name, new List<object> { "a" })), "[[\"name\",\"in\",[\"a\"]]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.In(x => x.Id, new OdooFilterValue().AddValue(3))), "[[\"id\",\"in\",[3]]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.NotIn(x => x.Id, new long[] { 1 })), "[[\"id\",\"not in\",[1]]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.NotIn(x => x.Name, new object[] { "a" })), "[[\"name\",\"not in\",[\"a\"]]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.NotIn(x => x.Name, new List<object> { "a" })), "[[\"name\",\"not in\",[\"a\"]]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.NotIn(x => x.Id, new OdooFilterValue().AddValue(4))), "[[\"id\",\"not in\",[4]]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.BeNull(x => x.ParentId)), "[[\"parent_id\",\"=\",false]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.NotBeNull(x => x.ParentId)), "[[\"parent_id\",\"!=\",false]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.ChildOf(x => x.ParentId, "Main")), "[[\"parent_id\",\"child_of\",\"Main\"]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.ChildOf(x => x.ParentId, 7)), "[[\"parent_id\",\"child_of\",7]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.ParentOf(x => x.ParentId, 7)), "[[\"parent_id\",\"parent_of\",7]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.Between(x => x.CreditLimit, 1.5, 2.5)), "[\"&\",[\"credit_limit\",\">=\",1.5],[\"credit_limit\",\"<=\",2.5]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.Not().EqualTo(x => x.IsCompany, true)), "[\"!\",[\"is_company\",\"=\",true]]" };
            yield return new object[] { (Func<OdooFilter<TestPartnerOdooModel>, OdooFilter>)(f => f.Or().And().EqualTo(x => x.Name, "a").EqualTo(x => x.Color, 1).EqualTo(x => x.Id, 2)), "[\"|\",\"&\",[\"name\",\"=\",\"a\"],[\"color\",\"=\",1],[\"id\",\"=\",2]]" };
        }

        [Theory]
        [MemberData(nameof(Operators))]
        public void Typed_operators_produce_odoo_domain_with_field_names(Func<OdooFilter<TestPartnerOdooModel>, OdooFilter> build, string expected)
        {
            var filter = build(OdooFilter<TestPartnerOdooModel>.Create());

            JsonConvert.SerializeObject(filter).Should().Be(expected);
        }

        [Fact]
        public void Typed_and_untyped_filters_give_same_domain()
        {
            var typed = OdooFilter<TestPartnerOdooModel>.Create().Or().ILike(x => x.Name, "a").In(x => x.CountryId, new long[] { 1, 2 });
            var untyped = OdooFilter.Create().Or().ILike("name", "a").In("country_id", new long[] { 1, 2 });

            JsonConvert.SerializeObject(typed).Should().Be(JsonConvert.SerializeObject(untyped));
        }

        [Fact]
        public void Build_returns_same_filter()
        {
            var filter = OdooFilter.Create().EqualTo("a", 1);

            filter.Build().Should().BeSameAs(filter);
        }

        [Fact]
        public void Typed_filter_can_be_used_where_untyped_is_expected()
        {
            OdooFilter filter = OdooFilter<TestPartnerOdooModel>.Create().EqualTo(x => x.Name, "a");

            var query = new OdooQuery { Filters = filter };

            JsonConvert.SerializeObject(query.Filters).Should().Be("[[\"name\",\"=\",\"a\"]]");
        }
    }
}
