using System;
using System.Linq;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooDictionaryModelTests
    {
        [Fact]
        public void Can_create_simple_dictionary()
        {
            var model = OdooDictionaryModel.Create(() => new PurchaseOrderOdooModel()
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
            var model = OdooDictionaryModel.Create(() => new ProductProductOdooDto()
            {
                Name = "test name",
            });
            model.Add(x => x.CreateDate, new DateTime());

            model.TableName.Should().NotBeEmpty();
            model.Should().NotBeEmpty();
            model.Count.Should().Be(2);

            model.First().Key.Should().Be("name");
            model.First().Value.Should().Be("test name");

            model.Skip(1).First().Key.Should().Be("create_date");
            model.Skip(1).First().Value.Should().Be(new DateTime());
        }

        [Fact]
        public void Can_create_dictionary_with_call_method()
        {
            var model = OdooDictionaryModel.Create(() => new PurchaseOrderLineOdooModel()
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
            var model = OdooDictionaryModel.Create(() => new PurchaseOrderLineOdooModel()
            {
                Name = TestString("123"),
            });
            model.Add(x => x.DisplayName, TestString("456"));

            model.TableName.Should().NotBeEmpty();
            model.Should().NotBeEmpty();
            model.Count.Should().Be(2);

            model.First().Key.Should().Be("name");
            model.First().Value.Should().BeOfType<string>();
            model.First().Value.Should().Be("123test");

            model.Skip(1).First().Key.Should().Be("display_name");
            model.Skip(1).First().Value.Should().BeOfType<string>();
            model.Skip(1).First().Value.Should().Be("456test");
        }

        [Fact]
        public void Can_create_dictionary_with_array()
        {
            var model = OdooDictionaryModel.Create(() => new ProductProductOdooDto()
            {
                ActivityIds = new long[] { 1, 2, 3 }
            });
            model.Add(x => x.MessageFollowerIds, new long[] { 4, 5, 6 });

            model.TableName.Should().NotBeEmpty();
            model.Should().NotBeEmpty();
            model.Count.Should().Be(2);

            model.First().Key.Should().Be("activity_ids");
            model.First().Value.Should().BeOfType<long[]>().And.NotBeNull();
            model.First().Value.Should().BeEquivalentTo(new long[] { 1, 2, 3 });

            model.Skip(1).First().Key.Should().Be("message_follower_ids");
            model.Skip(1).First().Value.Should().BeOfType<long[]>().And.NotBeNull();
            model.Skip(1).First().Value.Should().BeEquivalentTo(new long[] { 4, 5, 6 });
        }


        private string TestString() => new Guid().ToString();
        private string TestString(string x) => x + "test";
    }
}