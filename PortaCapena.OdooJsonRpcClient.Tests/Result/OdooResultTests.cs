using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Result;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Result
{
    public class OdooResultTests
    {
        [Fact]
        public void Succeed_result_has_value_and_no_error()
        {
            var result = OdooResult<int>.SucceedResult(5);

            result.Succeed.Should().BeTrue();
            result.Failed.Should().BeFalse();
            result.Value.Should().Be(5);
            result.Error.Should().BeNull();
            result.Message.Should().BeNull();
        }

        [Fact]
        public void Failed_result_has_error_details()
        {
            var result = OdooResult<int>.FailedResult("boom", 404, "404");

            result.Failed.Should().BeTrue();
            result.Message.Should().Be("boom");
            result.Error.Code.Should().Be(404);
            result.Error.HttpStatus.Should().Be("404");
        }

        [Fact]
        public void Failed_result_can_be_converted_to_other_type()
        {
            var source = OdooResult<string>.FailedResult("boom");
            source.Id = 3;

            var result = OdooResult<long[]>.FailedResult(source);

            result.Failed.Should().BeTrue();
            result.Id.Should().Be(3);
            result.Message.Should().Be("boom");
            result.Value.Should().BeNull();
        }

        [Fact]
        public void Odoo_error_answer_is_deserialized()
        {
            var json = @"{""jsonrpc"":""2.0"",""id"":7,""error"":{""code"":200,""message"":""Odoo Server Error"",""http_status"":""500"",
                ""data"":{""name"":""odoo.exceptions.ValidationError"",""message"":""Name is required"",""debug"":""Traceback"",""arguments"":[""Name is required""],""context"":{}}}}";

            var result = JsonConvert.DeserializeObject<OdooResult<long>>(json);

            result.Failed.Should().BeTrue();
            result.Id.Should().Be(7);
            result.Error.Code.Should().Be(200);
            result.Error.HttpStatus.Should().Be("500");
            result.Error.Data.Name.Should().Be("odoo.exceptions.ValidationError");
            result.Error.Data.Message.Should().Be("Name is required");
            result.Error.Data.Debug.Should().Be("Traceback");
            result.Error.Data.Arguments.Should().Equal("Name is required");
            result.Error.ToString().Should().Contain("Odoo Server Error").And.Contain("Name is required");
        }

        [Fact]
        public void Odoo_success_answer_is_deserialized()
        {
            var result = JsonConvert.DeserializeObject<OdooResult<long[]>>(@"{""jsonrpc"":""2.0"",""id"":1,""result"":[1,2]}");

            result.Succeed.Should().BeTrue();
            result.Value.Should().Equal(1, 2);
        }
    }
}
