using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Configurations;
using PortaCapena.OdooJsonRpcClient.Shared;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example;

public class OdooClientHttpTest : RequestTestBase, IDisposable
{
    // teardown
    public void Dispose()
    {
        OdooClientHttp.ClearHttpMessageHandlers();
        InitializeHttpClient();
    }
    
    [Fact]
    public async Task Can_call_custom_http_message_handler()
    {
        var customHandler = new TestMessageHandler();
        OdooClientHttp.Configure(f =>
        {
            f.AddHttpMessageHandler(customHandler);
        });
        InitializeHttpClient();
        var client = new OdooClient(TestConfig);
        
        await client.GetVersionAsync();
            
        client.Should().NotBeNull();
        customHandler.IsCalled.Should().BeTrue();
    }

    //FIFO: First In First Out
    [Fact]
    public async Task Can_call_chain_of_handlers_in_FIFO_order()
    {
        var expectedOrderOfExecution = new List<int> {1, 2};
        var uotExecutionOrder = new List<int>();
        var firstHandler = new TestMessageHandler(1, uotExecutionOrder);
        var secondHandler = new TestMessageHandler(2, uotExecutionOrder);
        OdooClientHttp.Configure(f =>
        {
            f.AddHttpMessageHandler(firstHandler);
            f.AddHttpMessageHandler(secondHandler);
        });
        
        InitializeHttpClient();
        var client = new OdooClient(TestConfig);

        await client.GetVersionAsync();
            
        client.Should().NotBeNull();
        uotExecutionOrder.Should().BeEquivalentTo(expectedOrderOfExecution);
    }

    // making the internal httpclient private is affecting
    // all the tests in the whole project.
    // Since, it's only instantiated once and keeping old states.
    private static void InitializeHttpClient()
    {
        // this internally resets the private httpclient
        OdooClient.BasicAuthenticationUsernamePassword = "admin";
    }
}

public class TestMessageHandler : DelegatingHandler
{
    private readonly List<int> _orderOfExecution = new();
    private int Order { get; }
        
    public bool IsCalled { get; private set; }
    
    public TestMessageHandler()
    {
    }

    public TestMessageHandler(int order, List<int> orderOfExecution)
    {
        Order = order;
        _orderOfExecution = orderOfExecution;
    }
        
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        IsCalled = true;
        _orderOfExecution.Add(Order);
        Console.WriteLine($"Order: {Order}");
        var response = await base.SendAsync(request, cancellationToken);
        return response;
    }
}