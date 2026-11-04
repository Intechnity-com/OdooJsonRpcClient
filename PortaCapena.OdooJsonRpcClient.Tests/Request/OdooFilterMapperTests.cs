using System;
using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using PortaCapena.OdooJsonRpcClient.Utils;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Request
{
    public class OdooFilterMapperTests
    {
        [Fact]
        public void Property_name_is_mapped_to_odoo_field()
        {
            var filter = OdooFilterMapper.ToOdooExpresion<TestPartnerOdooModel>("CountryId", OdooOperator.EqualsTo, 5);

            JsonConvert.SerializeObject(filter).Should().Be("[\"country_id\",\"=\",5]");
        }

        [Fact]
        public void Member_on_left_side()
        {
            var filter = OdooFilterMapper.ToOdooExpresion<TestPartnerOdooModel>((object)"Name", OdooOperator.CaseInsensitiveLike, "abc");

            JsonConvert.SerializeObject(filter).Should().Be("[\"name\",\"ilike\",\"abc\"]");
        }

        [Fact]
        public void Member_on_right_side_is_swapped()
        {
            var filter = OdooFilterMapper.ToOdooExpresion<TestPartnerOdooModel>((object)5, OdooOperator.EqualsTo, "Color");

            JsonConvert.SerializeObject(filter).Should().Be("[\"color\",\"=\",5]");
        }

        [Fact]
        public void Two_expressions_are_joined_with_operator()
        {
            var left = new object[] { "Name", OdooOperator.EqualsTo, "a" };
            var right = new object[] { "Color", OdooOperator.GreaterThan, 1 };

            var filter = OdooFilterMapper.ToOdooExpresion<TestPartnerOdooModel>(left, OdooOperator.Or, right);

            JsonConvert.SerializeObject(filter).Should().Be("[[\"name\",\"=\",\"a\"],\"|\",[\"color\",\">\",1]]");
        }

        [Fact]
        public void Invalid_array_expressions_throw()
        {
            Action act = () => OdooFilterMapper.ToOdooExpresion<TestPartnerOdooModel>(new object[] { "Name" }, OdooOperator.Or, new object[] { "Color" });

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Two_values_without_member_throw()
        {
            Action act = () => OdooFilterMapper.ToOdooExpresion<TestPartnerOdooModel>((object)1, OdooOperator.EqualsTo, 2);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Path_expression_joins_fields_with_dot()
        {
            var filter = OdooFilterMapper.ToOdooPathExpresion(OdooOperator.EqualsTo, "PL", "partner_id", "country_id", "code");

            JsonConvert.SerializeObject(filter).Should().Be("[\"partner_id.country_id.code\",\"=\",\"PL\"]");
        }

        [Fact]
        public void Path_expression_without_fields_throws()
        {
            Action empty = () => OdooFilterMapper.ToOdooPathExpresion(OdooOperator.EqualsTo, "PL");
            Action nullPath = () => OdooFilterMapper.ToOdooPathExpresion(OdooOperator.EqualsTo, "PL", null);

            empty.Should().Throw<ArgumentException>();
            nullPath.Should().Throw<ArgumentException>();
        }
    }
}
