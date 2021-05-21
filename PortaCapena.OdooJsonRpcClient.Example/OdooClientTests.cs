using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class OdooClientTests : RequestTestBase
    {

        [Fact]
        public async Task Can_get_odoo_version()
        {
            var odooClient = new OdooClient(TestConfig);

            var result = await odooClient.GetVersionAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
        }


        [Fact]
        public async Task Get_DotNet_model_should_return_string()
        {
            var odooClient = new OdooClient(TestConfig);
            var tableName = "product.product";
            var modelResult = await odooClient.GetModelAsync(tableName);

            modelResult.Succeed.Should().BeTrue();

            var model = OdooModelMapper.GetDotNetModel(tableName, modelResult.Value);
        }


        [Fact]
        public async Task Can_get_all_products()
        {
            var odooClient = new OdooClient(TestConfig);

            var products = await odooClient.GetAsync<ResCountryOdooModel>();


            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().BeGreaterThan(0);
            products.Succeed.Should().BeTrue();
        }


        [Fact]
        public async Task Can_get_all_countries()
        {
            var odoorepository = new OdooRepository<ResCountryOdooModel>(TestConfig);

            var products = await odoorepository.Query().Where(x => x.XStudioIsInEu, OdooOperator.EqualsTo, true).ToListAsync();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().BeGreaterThan(0);
            products.Succeed.Should().BeTrue();

            var dupa = products.Value.Where(x => x.XStudioIsInEu == true).ToList();
        }

        [Fact]
        public async Task Get_product_by_Id_test()
        {
            var odooClient = new OdooClient(TestConfig);

            var query = OdooQuery<ProductProductOdooDto>.Create()
                .Where(x => x.Id, OdooOperator.EqualsTo, 303);

            var products = await odooClient.GetAsync<ProductProductOdooDto>(query);

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().Be(1);
            products.Succeed.Should().BeTrue();
        }


        [Fact]
        public async Task Shoud_get_products_using_query_take()
        {
            var odooClient = new OdooClient(TestConfig);
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
            var odooClient = new OdooClient(TestConfig);
            var query = OdooQuery<ProductProductOdooDto>.Create().Skip(5);

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
            var odooClient = new OdooClient(TestConfig);
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
            var odooClient = new OdooClient(TestConfig);
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
            var odooClient = new OdooClient(TestConfig);

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
        //    products.Value.Length.Should().Be(1);
            products.Succeed.Should().BeTrue();
        }




        #region Create

        //  [Fact(Skip = "Test for working on Odoo")]
        [Fact]
        public async Task Can_Create_customer()
        {
            var name = "dupa";
            var city = "dupa";
            var postalCode = "dupa";
            var vatEu = "test";
            var address = "dupa";
            var isCompany = true;

            CompanyTypeResPartnerOdooEnum? test = CompanyTypeResPartnerOdooEnum.Company;

            var model = OdooDictionaryModel.Create(() => new ResPartnerOdooModel()
            {
                Name = name,
                CountryId = 20,
                City = city,
                Zip = postalCode,
                //Vat = vatEu,
                Street = address,
                CompanyType = test ?? CompanyTypeResPartnerOdooEnum.Individual
            });

            var odooClient = new OdooClient(TestConfig);

            var products = await odooClient.CreateAsync(model);

            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Can_create_update_get_and_delete_customer()
        {
            var model = OdooDictionaryModel.Create(() => new ResPartnerOdooModel()
            {
                Name = "dupa",
                CountryId = 20,
                City = "dupa",
                Zip = "dupa",
                Street = "dupa",
                CompanyType = CompanyTypeResPartnerOdooEnum.Individual
            });

            var odooClient = new OdooClient(TestConfig);
            var products = await odooClient.CreateAsync(model);

            products.Succeed.Should().BeTrue();
            products.Value.Should().BePositive();

            model.Add(x => x.Name, "new name");

            var editedCustomer = await odooClient.UpdateAsync(model, products.Value);
            editedCustomer.Succeed.Should().BeTrue();

            var query = new OdooQuery();
            query.Filters.EqualTo("id", products.Value);
            var customers = await odooClient.GetAsync<ResPartnerOdooModel>(query);

            customers.Succeed.Should().BeTrue();
            customers.Value.Length.Should().Be(1);
            customers.Value.First().Name.Should().Be("new name");

            var deleteResult = await odooClient.DeleteAsync(customers.Value.First());

            deleteResult.Succeed.Should().BeTrue();
        }


        [Fact(Skip = "Test for working on Odoo")]
        //  [Fact]
        public async Task Can_create_product()
        {
            var odooClient = new OdooClient(TestConfig);

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
        //[Fact]
        public async Task Can_create_update_delete_product()
        {
            var odooClient = new OdooClient(TestConfig);

            var model = new OdooCreateProduct()
            {
                Name = "Prod test Kg",
                UomId = 3,
                UomPoId = 3,
                InvoicePolicy = InvoicingPolicyOdooEnum.DeliveredQuantities
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
        // [Fact]
        public async Task Can_create_product_from_dictionary_model()
        {
            var odooClient = new OdooClient(TestConfig);

            var dictModel = OdooDictionaryModel.Create(() => new ProductProductOdooDto
            {
                Name = "test OdooCreateDictionary",
            });

            var dictModel2 = OdooDictionaryModel.Create<ProductProductOdooDto>(x => x.CombinationIndices, "create test");

            var dictModel3 = OdooDictionaryModel.Create<ProductProductOdooDto>(x => x.InvoicePolicy, InvoicingPolicyOdooEnum.DeliveredQuantities);

            dictModel.Add<ProductProductOdooDto>(x => x.CombinationIndices, "sadasd");
            dictModel.Add<ProductProductOdooDto>(x => x.InvoicePolicy, InvoicingPolicyOdooEnum.DeliveredQuantities);

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
            var odooClient = new OdooClient(TestConfig);

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
            var odooClient = new OdooClient(TestConfig);

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
            var odooClient = new OdooClient(TestConfig);

            var companyResult = await odooClient.GetAsync<CompanyOdooDto>(OdooQuery<CompanyOdooDto>.Create().ById(1));
            companyResult.Succeed.Should().BeTrue();
            var company = companyResult.Value.First();

            var partnerResult = await odooClient.GetAsync<ResPartnerOdooModel>(OdooQuery<ResPartnerOdooModel>.Create().ById(9));
            partnerResult.Succeed.Should().BeTrue();
            var partner = partnerResult.Value.First();

            var productQuery = OdooQuery<ProductProductOdooDto>.Create().ById(41);
            var productsResult = await odooClient.GetAsync<ProductProductOdooDto>(productQuery);
            productsResult.Succeed.Should().BeTrue();
            var product = productsResult.Value.First();



            var dictModel = OdooDictionaryModel.Create(() => new SaleOrderOdooModel
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


            var lineModel = OdooDictionaryModel.Create(() => new SaleOrderLineOdooDto()
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
                var loginResult = await OdooClient.LoginAsync(TestConfig);

                var param = new OdooRequestParams(TestConfig.ApiUrlJson, "object", "execute", TestConfig.DbName, loginResult.Value, TestConfig.Password, "sale.order", OdooOperation.OnChage,
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

        [Fact(Skip = "Test for working on Odoo")]
        //[Fact]
        public async Task Can_create_purchase_order()
        {

            var odooClient = new OdooClient(TestConfig);

            var partnerResult = await odooClient.GetAsync<StockPickingTypeOdooDto>();


            var dupa = new PurchaseOrderOdooModel()
            {
                DateOrder = DateTime.Now,
                PartnerId = 9,
                CurrencyId = 15,
                CompanyId = 1,
                //      PickingTypeId = 1,
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
                    client.Credentials = new NetworkCredential(TestConfig.UserName, TestConfig.Password);
                    var json = "{\"jsonrpc\":\"2.0\",\"method\":\"GUI.ShowNotification\",\"params\":{\"title\":\"This is the title of the message\",\"message\":\"This is the body of the message\"},\"id\":1}";
                    var response = client.UploadString(TestConfig.ApiUrlJson, "POST", json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public DateTime GetTestdate()
        {
            return DateTime.Now;
        }

        [Fact]
        // [Fact(Skip = "Test for working on Odoo")]
        public async Task CreatePurchaseOrderAsync()
        {
            var odooCompanyId = 32;

            var accountTaxOdooRepository = new OdooRepository<AccountTaxOdooModel>(TestConfig);
            var purchaseOrderOdooRepository = new OdooRepository<PurchaseOrderOdooModel>(TestConfig);
            var purchaseOrderLineOdooRepository = new OdooRepository<PurchaseOrderLineOdooModel>(TestConfig);

            var odooCompanyTaxes = await accountTaxOdooRepository.Query()
                .Where(x => x.CompanyId, OdooOperator.EqualsTo, odooCompanyId)
                .Where(x => x.AmountType, OdooOperator.EqualsTo, TaxComputationAccountTaxOdooEnum.PercentageOfPrice)
                .ToListAsync();

            //  odooCompanyTaxes.Succeed.Should().BeTrue();
            //   odooCompanyTaxes.Value.Should().NotBeNull().And.NotBeEmpty();

            var purchaseOrderModel = OdooDictionaryModel.Create(() => new PurchaseOrderOdooModel()
            {
                CompanyId = odooCompanyId,
                DateOrder = GetTestdate(),
                PartnerId = 1,
                CurrencyId = 1,
                XStudioPickupAddress = "pickupAddress",
                XStudioPickupDate = GetTestdate(),
                State = StatusPurchaseOrderOdooEnum.PurchaseOrder
            });

            //        var orderResult = await purchaseOrderOdooRepository.CreateAsync(purchaseOrderModel);
            //        orderResult.Succeed.Should().BeTrue();


            //    var taxId = odooCompanyTaxes.Value.FirstOrDefault(x => x.Amount == 6);

            //    taxId.Should().NotBeNull();

            var model = OdooDictionaryModel.Create(() => new PurchaseOrderLineOdooModel()
            {
                PriceUnit = 10,
                ProductId = 12,
                ProductQty = 11,
                //  OrderId = orderResult.Value,
                State = StatusPurchaseOrderLineOdooEnum.PurchaseOrder,
                TaxesId = new long[] { 12, 11 }
            });

            //       var createPurchaseOrderLineResult = await purchaseOrderLineOdooRepository.CreateAsync(model);

            //        createPurchaseOrderLineResult.Succeed.Should().BeTrue();
        }
    }
}