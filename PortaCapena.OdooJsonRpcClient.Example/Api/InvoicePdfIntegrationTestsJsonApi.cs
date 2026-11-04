using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example.Api
{
    /// <summary>
    /// Invoice PDF from Odoo 19 (docker/odoo19) with the JSON API (issue #83).
    /// </summary>
    public class InvoicePdfIntegrationTestsJsonApi : RequestTestBase
    {
        [Fact]
        public async Task Invoice_pdf_can_be_generated_with_send_wizard_without_sending()
        {
            var invoiceRepository = new OdooRepository<IntegrationInvoiceOdooModel>(JsonApiTestConfig);
            var wizardRepository = new OdooRepository<IntegrationInvoiceSendWizardOdooModel>(JsonApiTestConfig);
            var invoice = await invoiceRepository.Query()
                .Where(x => x.MoveType, OdooOperator.EqualsTo, "out_invoice")
                .Where(x => x.State, OdooOperator.EqualsTo, "posted")
                .Select(x => new { x.Name })
                .FirstAsync();

            var wizard = await wizardRepository.CreateAsync(OdooDictionaryModel.Create(() => new IntegrationInvoiceSendWizardOdooModel
            {
                MoveId = invoice.Value.Id,
                SendingMethods = new string[0]
            }));
            var print = await wizardRepository.ActionAsync("action_send_and_print", wizard.Value);
            var pdfInvoice = await invoiceRepository.Query()
                .ById(invoice.Value.Id)
                .Select(x => new { x.InvoicePdfReportFile })
                .FirstAsync();

            wizard.Succeed.Should().BeTrue(wizard.Message);
            print.Succeed.Should().BeTrue(print.Message);
            var pdf = Convert.FromBase64String(pdfInvoice.Value.InvoicePdfReportFile);
            Encoding.ASCII.GetString(pdf, 0, 5).Should().Be("%PDF-");
        }
    }
}
