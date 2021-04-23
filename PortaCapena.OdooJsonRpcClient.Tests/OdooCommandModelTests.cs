using System;
using System.Linq;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooCommandModelTests
    {
        [Fact]
        public void Can_create_simple_dictionary()
        {
            var model = OdooCommandModel.Create(() => new PurchaseOrderOdooModel()
            {
                CompanyId = 1,
                PartnerId = 2,
                CurrencyId = 3,
                XStudioPickupAddress = "pickupAddress",
                State = StatusPurchaseOrderOdooEnum.PurchaseOrder
            });

            model.TableName.Should().NotBeEmpty();
            model.Should().NotBeEmpty();
            model.Count.Should().Be(5);

            model.First().Key.Should().Be("company_id");
            model.First().Value.Should().Be(1);

            model.Skip(1).First().Key.Should().Be("partner_id");
            model.Skip(1).First().Value.Should().Be(2);

            model.Skip(2).First().Key.Should().Be("currency_id");
            model.Skip(2).First().Value.Should().Be(3);

            model.Skip(3).First().Key.Should().Be("x_studio_pickup_address");
            model.Skip(3).First().Value.Should().Be("pickupAddress");

            model.Skip(4).First().Key.Should().Be("state");
            model.Skip(4).First().Value.Should().Be(StatusPurchaseOrderOdooEnum.PurchaseOrder);
        }

        [Fact]
        public void Can_create_dictionary_with_create_instance()
        {
            var model = OdooCommandModel.Create(() => new PurchaseOrderLineOdooModel()
            {
              DateOrder = new DateTime(),
            });
            model.Add(x => x.CreateDate, new DateTime());

            model.TableName.Should().NotBeEmpty();
            model.Should().NotBeEmpty();
            model.Count.Should().Be(2);

            model.First().Key.Should().Be("date_order");
            model.First().Value.Should().Be(new DateTime());

            model.Skip(1).First().Key.Should().Be("create_date");
            model.Skip(1).First().Value.Should().Be(new DateTime());
        }

        [Fact]
        public void Can_create_dictionary_with_call_method()
        {
            var model = OdooCommandModel.Create(() => new PurchaseOrderLineOdooModel()
            {
                Name = TestString(),
            });
            model.Add(x => x.DisplayName, TestString());

            model.TableName.Should().NotBeEmpty();
            model.Should().NotBeEmpty();
            model.Count.Should().Be(2);

            model.First().Key.Should().Be("name");
            model.First().Value.Should().BeOfType<string>();

            model.Skip(1).First().Key.Should().Be("display_name");
            model.Skip(1).First().Value.Should().BeOfType<string>();
        }

        [Fact]
        public void Can_create_dictionary_with_call_method_with_params()
        {
            var model = OdooCommandModel.Create(() => new PurchaseOrderLineOdooModel()
            {
                Name = TestString("123"),
            });
            model.Add(x => x.DisplayName, TestString("123"));

            model.TableName.Should().NotBeEmpty();
            model.Should().NotBeEmpty();
            model.Count.Should().Be(2);

            model.First().Key.Should().Be("name");
            model.First().Value.Should().BeOfType<string>();

            model.Skip(1).First().Key.Should().Be("display_name");
            model.Skip(1).First().Value.Should().BeOfType<string>();
        }

        [Fact]
        public void Can_create_dictionary_with_array()
        {
            var model = OdooCommandModel.Create(() => new PurchaseOrderLineOdooModel()
            {
                AnalyticTagIds = new long[]{1, 2, 3}
            });
            model.Add(x => x.InvoiceLines, new long[] { 4, 5, 6 });

            model.TableName.Should().NotBeEmpty();
            model.Should().NotBeEmpty();
            model.Count.Should().Be(2);

            model.First().Key.Should().Be("analytic_tag_ids");
            model.First().Value.Should().BeOfType<long[]>();
            model.First().Value.Should().BeEquivalentTo(new long[] { 1, 2, 3 });

            model.Skip(1).First().Key.Should().Be("invoice_lines");
            model.Skip(1).First().Value.Should().BeOfType<long[]>();
            model.Skip(1).First().Value.Should().BeEquivalentTo(new long[] { 4, 5, 6 });
        }


        private string TestString() => new Guid().ToString();
        private string TestString(string x) => x;
    }
}