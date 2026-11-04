using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Example.Infrastructure;
using PortaCapena.OdooJsonRpcClient.Shared;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example.Rpc
{
    /// <summary>
    /// Models generated from Odoo 15 (docker/odoo15) with the JSON-RPC API must compile (issues #50, #122).
    /// </summary>
    public class GeneratedModelCompilationTestsJsonRpc : RequestTestBase
    {
        [Theory]
        [InlineData("res.partner")]
        [InlineData("res.users")]
        [InlineData("res.company")]
        [InlineData("product.template")]
        [InlineData("product.product")]
        [InlineData("sale.order")]
        [InlineData("sale.order.line")]
        [InlineData("purchase.order")]
        [InlineData("account.move")]
        [InlineData("account.move.line")]
        [InlineData("account.payment")]
        [InlineData("stock.picking")]
        [InlineData("project.task")]
        public async Task Generated_model_compiles(string tableName)
        {
            var model = await new OdooClient(TestConfig).GetModelAsync(tableName);
            model.Succeed.Should().BeTrue(model.Message);

            var code = OdooModelMapper.GetDotNetModel(tableName, model.Value);
            var errors = GeneratedModelCompiler.Compile(code);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Records with JSON / object fields (tax_totals, invoice_payments_widget, analytic_distribution...) are read
        /// into the generated model with GetAsync&lt;T&gt; (issues #76, #94, #95, #98, #127).
        /// </summary>
        [Theory]
        [InlineData("res.partner")]
        [InlineData("res.users")]
        [InlineData("product.template")]
        [InlineData("sale.order")]
        [InlineData("purchase.order")]
        [InlineData("account.move")]
        [InlineData("account.move.line")]
        [InlineData("crm.team")]
        public async Task Generated_model_reads_records(string tableName)
        {
            var client = new OdooClient(TestConfig);
            var model = await client.GetModelAsync(tableName);
            var modelType = GeneratedModelCompiler.CompileModelType(tableName, OdooModelMapper.GetDotNetModel(tableName, model.Value));

            var getAsync = typeof(OdooClient).GetMethods().Single(x => x.Name == nameof(OdooClient.GetAsync) && x.IsGenericMethodDefinition && !x.IsStatic && x.GetParameters()[0].ParameterType == typeof(Request.OdooQuery));
            dynamic result = await (dynamic)getAsync.MakeGenericMethod(modelType).Invoke(client, new object[] { new Request.OdooQuery { Limit = 20 }, null, System.Threading.CancellationToken.None });

            ((bool)result.Succeed).Should().BeTrue((string)result.Message);
            ((System.Array)result.Value).Length.Should().BeGreaterThan(0);
        }
    }
}
