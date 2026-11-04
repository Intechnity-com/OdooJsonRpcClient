using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Tests.Infrastructure;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    /// <summary>
    /// Static methods use the built-in HttpClient, so they are called against a closed local port:
    /// they must return failed results instead of throwing. Short timeout keeps the tests fast on Windows,
    /// where a refused local connection is retried for ~2 seconds.
    /// </summary>
    public class OdooClientStaticApiTests
    {
        private static readonly OdooConfig Unreachable = new OdooConfig("http://127.0.0.1:1", "db", "user", "pass", TimeSpan.FromMilliseconds(200));

        [Fact]
        public async Task Static_methods_return_failed_result_when_server_is_unreachable()
        {
            var dictionary = OdooDictionaryModel.Create<TestPartnerOdooModel>(x => x.Name, "a");
            var model = new TestPartnerCreateModel { Name = "a" };

            var results = new[]
            {
                (await OdooClient.GetVersionAsync(Unreachable)).Failed,
                (await OdooClient.LoginAsync(Unreachable)).Failed,
                (await OdooClient.GetAsync<TestPartnerOdooModel>(Unreachable, 1)).Failed,
                (await OdooClient.GetAsync("res.partner", Unreachable, 1)).Failed,
                (await OdooClient.GetCountAsync<TestPartnerOdooModel>(Unreachable, 1)).Failed,
                (await OdooClient.CreateAsync(Unreachable, 1, model)).Failed,
                (await OdooClient.CreateAsync(Unreachable, 1, dictionary)).Failed,
                (await OdooClient.CreateAsync(Unreachable, 1, new IOdooCreateModel[] { model })).Failed,
                (await OdooClient.CreateAsync(Unreachable, 1, new[] { dictionary })).Failed,
                (await OdooClient.UpdateAsync(Unreachable, 1, model, 1)).Failed,
                (await OdooClient.UpdateAsync(Unreachable, 1, dictionary, 1)).Failed,
                (await OdooClient.UpdateRangeAsync(Unreachable, 1, model, new long[] { 1 })).Failed,
                (await OdooClient.UpdateRangeAsync(Unreachable, 1, dictionary, new long[] { 1 })).Failed,
                (await OdooClient.DeleteAsync(Unreachable, 1, "res.partner", 1)).Failed,
                (await OdooClient.DeleteRangeAsync(Unreachable, 1, "res.partner", new long[] { 1 })).Failed,
                (await OdooClient.ActionAsync(Unreachable, 1, "res.partner", "action_archive", 1)).Failed,
                (await OdooClient.ActionCollectionAsync(Unreachable, 1, "res.partner", "action_archive", new object[] { 1 })).Failed,
                (await OdooClient.GetModelAsync(Unreachable, 1, "res.partner")).Failed,
                (await OdooClient.CallAndDeserializeAsync<object>(OdooRequestModel.Version(Unreachable))).Failed,
                (await OdooClient.GetDatabasesAsync(Unreachable)).Failed,
            };

            results.Should().OnlyContain(failed => failed);
        }

        [Fact]
        public async Task Static_create_many_validates_arguments()
        {
            Func<Task> empty = () => OdooClient.CreateAsync(Unreachable, 1, new IOdooCreateModel[0]);
            Func<Task> nullModels = () => OdooClient.CreateAsync(Unreachable, 1, (OdooDictionaryModel[])null);

            await empty.Should().ThrowAsync<ArgumentException>();
            await nullModels.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Call_async_throws_transport_errors()
        {
            Func<Task> act = () => OdooClient.CallAsync(OdooRequestModel.Version(Unreachable));

            await act.Should().ThrowAsync<HttpRequestException>();
        }
    }
}
