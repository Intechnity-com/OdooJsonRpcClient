using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests.Request
{
    public class OdooRequestModelFactoryTests
    {
        private static readonly OdooConfig Config = new OdooConfig("http://odoo.test/", "db", "user", "pass");

        [Fact]
        public void Request_envelope_is_json_rpc_call()
        {
            var json = JObject.Parse(JsonConvert.SerializeObject(OdooRequestModel.Version(Config)));

            ((string)json["jsonrpc"]).Should().Be("2.0");
            ((string)json["method"]).Should().Be("call");
            ((string)json["params"]["service"]).Should().Be("common");
            ((string)json["params"]["method"]).Should().Be("version");
            json["params"]["args"].Should().BeEmpty();
            json["params"]["url"].Should().BeNull("url is not serialized");
        }

        [Fact]
        public void Url_is_jsonrpc_endpoint()
        {
            OdooRequestModel.Login(Config).Params.Url.Should().Be("http://odoo.test/jsonrpc");
        }

        [Fact]
        public void Login_sends_credentials()
        {
            OdooRequestModel.Login(Config).Params.Args.Should().Equal("db", "user", "pass");
        }

        [Fact]
        public void Model_fields_uses_execute_fields_get()
        {
            var request = OdooRequestModel.ModelFields(Config, 2, "res.partner");

            request.Params.Method.Should().Be("execute");
            request.Params.Args.Should().Equal("db", 2, "pass", "res.partner", OdooOperation.FieldsGet);
        }

        [Fact]
        public void Search_sends_domain_and_kwargs()
        {
            var query = OdooQuery<TestPartnerOdooModel>.Create().Where(x => x.Name, OdooOperator.EqualsTo, "a").Take(3);

            var json = JObject.Parse(JsonConvert.SerializeObject(OdooRequestModel.Search(Config, 2, "test.partner", query)));
            var args = (JArray)json["params"]["args"];

            ((string)args[4]).Should().Be("search");
            args[5].ToString(Formatting.None).Should().Be("[[[\"name\",\"=\",\"a\"]]]");
            ((int)args[6]["limit"]).Should().Be(3);
        }

        [Fact]
        public void Read_sends_ids_as_one_argument_and_fields()
        {
            var query = new OdooQuery();
            query.ReturnFields.Add("name");

            var json = JObject.Parse(JsonConvert.SerializeObject(OdooRequestModel.Read(Config, 2, "res.partner", new long[] { 1, 2 }, query)));
            var args = (JArray)json["params"]["args"];

            ((string)args[4]).Should().Be("read");
            args[5].ToString(Formatting.None).Should().Be("[[1,2]]");
            args[6]["fields"].ToObject<string[]>().Should().Equal("name");
        }

        [Fact]
        public void Metadata_sends_ids_as_one_argument()
        {
            var json = JObject.Parse(JsonConvert.SerializeObject(OdooRequestModel.Metadata(Config, 2, "res.partner", new long[] { 4 })));
            var args = (JArray)json["params"]["args"];

            ((string)args[4]).Should().Be("get_metadata");
            args[5].ToString(Formatting.None).Should().Be("[[4]]");
        }

        [Fact]
        public void Action_wraps_argument_and_action_collection_does_not()
        {
            var action = JObject.Parse(JsonConvert.SerializeObject(OdooRequestModel.Action(Config, 2, "sale.order", "action_confirm", new long[] { 1, 2 })));
            var collection = JObject.Parse(JsonConvert.SerializeObject(OdooRequestModel.ActionCollection(Config, 2, "sale.order", "message_post", new object[] { new long[] { 1 }, "text" })));

            action["params"]["args"][5].ToString(Formatting.None).Should().Be("[[1,2]]");
            collection["params"]["args"][5].ToString(Formatting.None).Should().Be("[[1],\"text\"]");
        }

        [Fact]
        public void Context_is_sent_in_kwargs_and_empty_context_is_omitted()
        {
            var withContext = OdooRequestModel.Delete(Config, 2, "res.partner", new long[] { 1 }, new OdooContext("pl_PL"));
            var emptyContext = OdooRequestModel.Delete(Config, 2, "res.partner", new long[] { 1 }, new OdooContext());

            JObject.Parse(JsonConvert.SerializeObject(withContext))["params"]["args"][6]["context"]["lang"].Value<string>().Should().Be("pl_PL");
            emptyContext.Params.Args.Should().HaveCount(6, "trailing null kwargs are removed");
        }

        [Fact]
        public void Explicit_id_and_method_are_kept()
        {
            var request = new OdooRequestModel(42, "2.0", "custom", new OdooRequestParams("http://x", "object", "execute_kw", "db", null, null));

            request.Id.Should().Be(42);
            request.Method.Should().Be("custom");
            request.Params.Args.Should().Equal("db");
        }

        [Fact]
        public void Generated_ids_are_unique()
        {
            Enumerable.Range(0, 100).Select(_ => OdooRequestModel.Version(Config).Id).Should().OnlyHaveUniqueItems();
        }
    }
}
