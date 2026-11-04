using FluentAssertions;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Models
{
    public class OdooCommandTests
    {
        [Fact]
        public void Commands_produce_odoo_x2many_command_lists()
        {
            JsonConvert.SerializeObject(OdooCommand.Create(new OdooDictionaryModel { { "name", "a" } })).Should().Be("[[0,0,{\"name\":\"a\"}]]");
            JsonConvert.SerializeObject(OdooCommand.Update(5, new OdooDictionaryModel { { "name", "a" } })).Should().Be("[[1,5,{\"name\":\"a\"}]]");
            JsonConvert.SerializeObject(OdooCommand.Delete(5)).Should().Be("[[2,5,0]]");
            JsonConvert.SerializeObject(OdooCommand.Unlink(5)).Should().Be("[[3,5,0]]");
            JsonConvert.SerializeObject(OdooCommand.Link(5)).Should().Be("[[4,5,0]]");
            JsonConvert.SerializeObject(OdooCommand.Clear()).Should().Be("[[5,0,0]]");
            JsonConvert.SerializeObject(OdooCommand.Set(1, 2)).Should().Be("[[6,0,[1,2]]]");
        }
    }
}
