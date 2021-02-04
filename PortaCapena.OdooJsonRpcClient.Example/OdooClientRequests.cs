using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using PortaCapena.OdooJsonRpcClient.Shared.Models.Create;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example
{
    public class OdooClientRequests : TestBase
    {

        [Fact]
        public async Task Can_get_odoo_version()
        {
            var odooClient = new OdooClient(Config);

            var result = await odooClient.GetVersionAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
        }


        [Fact]
        public async Task Can_get_all_products()
        {
            var odooClient = new OdooClient(Config);

            var products = await odooClient.GetAsync<SaleOrderOdooModel>();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().BeGreaterThan(0);
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Get_product_by_Id_test()
        {
            var odooClient = new OdooClient(Config);

            var query = OdooQuery<ProductProductOdooDto>.Create()
                .Where(x => x.Barcode, OdooOperator.EqualsTo, 1);

            var products = await odooClient.GetAsync<ProductProductOdooDto>(query);

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().Be(1);
            products.Succeed.Should().BeTrue();
        }


        [Fact]
        public async Task Shoud_get_products_using_query_take()
        {
            var odooClient = new OdooClient(Config);
            var query = OdooQuery<ProductProductOdooDto>.Create()
                .Take(10);

            var products = await odooClient.GetAsync<ProductProductOdooDto>(query);

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().Be(10);
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Shoud_get_products_using_query_skip()
        {
            var odooClient = new OdooClient(Config);
            var query = OdooQuery<ProductProductOdooDto>.Create()
                .Skip(5);

            var products = await odooClient.GetAsync<ProductProductOdooDto>(query);

            var allProducts = await odooClient.GetAsync<ProductProductOdooDto>();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();

            allProducts.Error.Should().BeNull();
            allProducts.Value.Should().NotBeNull();

            products.Value.Length.Should().Be(allProducts.Value.Length - 5);
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Get_products_with_order_query()
        {
            var odooClient = new OdooClient(Config);
            var query = OdooQuery<ProductProductOdooDto>.Create()
                .OrderBy(x => x.Id);

            var products = await odooClient.GetAsync<ProductProductOdooDto>(query);

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().BeGreaterThan(0);
            products.Succeed.Should().BeTrue();


            var orderedByAsc = products.Value.OrderBy(p => p.Id);
            products.Value.SequenceEqual(orderedByAsc).Should().BeTrue();
        }

        [Fact]
        public async Task Get_products_with_order_desc_query()
        {
            var odooClient = new OdooClient(Config);
            var query = OdooQuery<ProductProductOdooDto>.Create()
                .OrderByDescending(x => x.Id);

            var products = await odooClient.GetAsync<ProductProductOdooDto>(query);

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().BeGreaterThan(0);
            products.Succeed.Should().BeTrue();


            var orderedByAsc = products.Value.OrderByDescending(p => p.Id);
            products.Value.SequenceEqual(orderedByAsc).Should().BeTrue();
        }



        [Fact]
        public async Task Should_get_products_with_selected_properties_using_query()
        {
            var odooClient = new OdooClient(Config);

            var filters = OdooQuery<ProductProductOdooDto>.Create()
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.WriteDate
                })
                .Where(x => x.Name, OdooOperator.EqualsTo, "Bioboxen 610l")
                .Where(x => x.WriteDate, OdooOperator.GreaterThanOrEqualTo, new DateTime(2020, 12, 2));

            var products = await odooClient.GetAsync<ProductProductOdooDto>(filters);

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().Be(1);
            products.Succeed.Should().BeTrue();
        }


        [Fact]
        public async Task Get_DotNet_model_should_return_string()
        {
            var dupa = InvoicingPolicyOdooEnum.DeliveredQuantities.OdooValue();
            var odooClient = new OdooClient(Config);
            var tableName = "sale.order";
            var modelResult = await odooClient.GetModelAsync(tableName);

            modelResult.Succeed.Should().BeTrue();

            var model = OdooModelMapper.GetDotNetModel(tableName, modelResult.Value);
        }


        #region Create


        //  [Fact(Skip = "Test for working on Odoo")]
        [Fact]
        public async Task Can_create_product()
        {
            var odooClient = new OdooClient(Config);

            var model = new OdooCreateProduct()
            {
                Name = "Prod test Kg",
                UomId = 3,
                UomPoId = 3,
                InvoicePolicy = InvoicingPolicyOdooEnum.DeliveredQuantities
            };

            var products = await odooClient.CreateAsync(model);

            products.Error.Should().BeNull();
            products.Succeed.Should().BeTrue();
        }

        [Fact(Skip = "Test for working on Odoo")]
        // [Fact]
        public async Task Can_create_update_delete_product()
        {
            var odooClient = new OdooClient(Config);

            var model = new OdooCreateProduct()
            {
                Name = "Prod test Kg",
                UomId = 3,
                UomPoId = 3
            };

            var createResult = await odooClient.CreateAsync(model);
            createResult.Succeed.Should().BeTrue();
            var createdProductId = createResult.Value;

            var query = OdooQuery<ProductProductOdooDto>.Create()
                .Where(x => x.Id, OdooOperator.EqualsTo, createdProductId);

            var products = await odooClient.GetAsync<ProductProductOdooDto>(query);
            products.Succeed.Should().BeTrue();
            products.Value.Length.Should().Be(1);
            products.Value.First().Name.Should().Be(model.Name);

            model.Name += " update";

            var updateProductResult = await odooClient.UpdateAsync(model, createdProductId);
            updateProductResult.Succeed.Should().BeTrue();

            var query2 = OdooQuery<ProductProductOdooDto>.Create()
                .Where(x => x.Id, OdooOperator.EqualsTo, createdProductId);

            var products2 = await odooClient.GetAsync<ProductProductOdooDto>(query2);
            products2.Succeed.Should().BeTrue();
            products2.Value.Length.Should().Be(1);
            products2.Value.First().Name.Should().Be(model.Name);


            var deleteProductResult = await odooClient.DeleteAsync(model.OdooTableName(), createdProductId);
            deleteProductResult.Succeed.Should().BeTrue();
            deleteProductResult.Value.Should().BeTrue();
        }

        [Fact(Skip = "Test for working on Odoo")]
        //  [Fact]
        public async Task Can_create_product_from_dictionary_model()
        {
            var odooClient = new OdooClient(Config);

            var dictModel = OdooCreateDictionary.Create(() => new ProductProductOdooDto
            {
                Name = "test OdooCreateDictionary",
                CombinationIndices = "sadasd"
            });

            var createResult = await odooClient.CreateAsync(dictModel);
            createResult.Succeed.Should().BeTrue();
            createResult.Value.Should().BeGreaterThan(0);

            var deleteProductResult = await odooClient.DeleteAsync("product.product", createResult.Value);
            deleteProductResult.Succeed.Should().BeTrue();
            deleteProductResult.Value.Should().BeTrue();
        }

        [Fact(Skip = "Test for working on Odoo")]
        // [Fact]
        public async Task Can_create_product_and_delete_using_object()
        {
            var odooClient = new OdooClient(Config);

            var model = new OdooCreateProduct()
            {
                Name = "Prod test Kg",
                UomId = 3,
                UomPoId = 3
            };

            var createResult = await odooClient.CreateAsync(model);
            createResult.Succeed.Should().BeTrue();
            var createdProductId = createResult.Value;

            var query = OdooQuery<ProductProductOdooDto>.Create().ById(createdProductId);

            var product = await odooClient.GetAsync<ProductProductOdooDto>(query);
            product.Succeed.Should().BeTrue();
            product.Value.First().Name.Should().Be(model.Name);

            var deleteProductResult = await odooClient.DeleteAsync(product.Value.First());
            deleteProductResult.Succeed.Should().BeTrue();
            deleteProductResult.Value.Should().BeTrue();
        }

        [Fact(Skip = "Test for working on Odoo")]
        //[Fact]
        public async Task Can_create_voucher()
        {
            var odooClient = new OdooClient(Config);

            var model = new OdooVoucherCreateOrUpdate()
            {
                Active = true,
                Name = $"GiftCard 123E",
                PromoCode = "codetest1",
                RuleDateTo = new DateTime(2021, 1, 1),
                DiscountFixedAmount = 2d,
                DiscountType = "fixed_amount",
                ProgramType = "promotion_program"
            };

            var createResult = await odooClient.CreateAsync(model);
            createResult.Succeed.Should().BeTrue();
        }

        [Fact(Skip = "Test for working on Odoo")]
        // [Fact]
        public async Task Can_create_sale_order()
        {
            var odooClient = new OdooClient(Config);

            var companyResult = await odooClient.GetAsync<CompanyOdooDto>(OdooQuery<CompanyOdooDto>.Create().ById(1));
            companyResult.Succeed.Should().BeTrue();
            var company = companyResult.Value.First();

            var partnerResult = await odooClient.GetAsync<PartnerOdooDto>(OdooQuery<PartnerOdooDto>.Create().ById(9));
            partnerResult.Succeed.Should().BeTrue();
            var partner = partnerResult.Value.First();

            var productQuery = OdooQuery<ProductProductOdooDto>.Create().ById(41);
            var productsResult = await odooClient.GetAsync<ProductProductOdooDto>(productQuery);
            productsResult.Succeed.Should().BeTrue();
            var product = productsResult.Value.First();



            var dictModel = OdooCreateDictionary.Create(() => new SaleOrderOdooModel
            {

                PricelistId = 17,

                PartnerId = partner.Id,
                PartnerInvoiceId = partner.Id,
                PartnerShippingId = partner.Id,

                CompanyId = company.Id,

                DateOrder = DateTime.Now,
            });

            var createResult = await odooClient.CreateAsync(dictModel);
            createResult.Message.Should().BeNullOrEmpty();
            createResult.Succeed.Should().BeTrue();


            var lineModel = OdooCreateDictionary.Create(() => new SaleOrderLineOdooDto()
            {
                OrderId = createResult.Value,
                Name = "test line",
                ProductId = product.Id,
                ProductUomQty = 24,
                PriceUnit = 15
            });

            var createLineResult = await odooClient.CreateAsync(lineModel);
            createLineResult.Succeed.Should().BeTrue();
        }

        [Fact(Skip = "Test for working on Odoo")]
        //[Fact]
        public async Task OnChange_test()
        {
            try
            {
                var loginResult = await OdooClient.LoginAsync(Config);

                var param = new OdooRequestParams(Config.ApiUrlJson, "object", "execute", Config.DbName, loginResult.Value, Config.Password, "sale.order", OdooOperation.OnChage,
                    new Dictionary<string, int>() { { "onchange_pricelist_id", 17 } }, 135);
                var request = new OdooRequestModel(param);

                var result = OdooClient.CallAsync(request);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // [Fact(Skip = "Test for working on Odoo")]
        [Fact]
        public async Task Can_create_purchase_order()
        {

            var odooClient = new OdooClient(Config);

            var partnerResult = await odooClient.GetAsync<StockPickingTypeOdooDto>();


            var dupa = new PurchaseOrderOdooDto
            {
                DateOrder = DateTime.Now,
                PartnerId = 9,
                CurrencyId = 15,
                CompanyId = 1,
                PickingTypeId = 1,
                Name = "test purchase"
            };

            //var createResult = await odooClient.CreateAsync(dupa);
            //createResult.Message.Should().BeNullOrEmpty();
            //createResult.Succeed.Should().BeTrue();
        }


        #endregion


        [Fact(Skip = "Test for working on Odoo")]
        //[Fact]
        public void OnChange_test1()
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(Config.UserName, Config.Password);
                    var json = "{\"jsonrpc\":\"2.0\",\"method\":\"GUI.ShowNotification\",\"params\":{\"title\":\"This is the title of the message\",\"message\":\"This is the body of the message\"},\"id\":1}";
                    var response = client.UploadString(Config.ApiUrlJson, "POST", json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}