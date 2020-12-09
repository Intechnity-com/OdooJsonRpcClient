using System;
using System.Linq;
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

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public class OdooClientTests : OdooTestBase
    {

        [Fact]
        public async Task Get_version_test()
        {
            var odooClient = new OdooClient(Config);

            var result = await odooClient.GetVersionAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
        }


        [Fact]
        public async Task Get_all_products_test()
        {
            var odooClient = new OdooClient(Config);

            var products = await odooClient.GetAsync<ProductProductOdooDto>();

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
                .Where(x => x.Barcode, OdooOperator.EqualsTo, 67);

            var products = await odooClient.GetAsync<ProductProductOdooDto>(query);

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().Be(1);
            products.Succeed.Should().BeTrue();
        }


        [Fact]
        public async Task Get_products_with_take_query_test()
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
        public async Task Get_products_with_skip_query_test()
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
        public async Task Get_products_with_order_query_test()
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
        public async Task Get_products_with_order_desc_query_test()
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
        public async Task Get_with_filters_test2()
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
            var odooClient = new OdooClient(Config);
            var tableName = "sale.order.line";
            var modelResult = await odooClient.GetModelAsync(tableName);

            modelResult.Succeed.Should().BeTrue();

            var model = OdooModelMapper.GetDotNetModel(tableName, modelResult.Value);
        }


        #region Create


        [Fact(Skip = "Test for working on Odoo")]
        // [Fact]
        public async Task Create_product_test()
        {
            var odooClient = new OdooClient(Config);

            var model = new OdooCreateProduct()
            {
                Name = "Prod test Kg",
                UomId = 3,
                UomPoId = 3
            };

            var products = await odooClient.CreateAsync(model);

            products.Error.Should().BeNull();
            products.Succeed.Should().BeTrue();
        }

        [Fact(Skip = "Test for working on Odoo")]
        // [Fact]
        public async Task Create_Update_delete_product_test()
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
        public async Task Create_product_from_dictionary_test()
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
        public async Task Create_product_and_delete_object_test()
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
        public async Task Create_voucher()
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

        //[Fact(Skip = "Test for working on Odoo")]
        [Fact]
        public async Task Create_sale_order()
        {
            var odooClient = new OdooClient(Config);

            var dictModel = OdooCreateDictionary.Create(() => new SaleOrderOdooDto
            {
                CompanyId = 1,
                DateOrder = DateTime.Now,
                PartnerId = 1,
                PricelistId = 1,
                PartnerInvoiceId = 1,
                PartnerShippingId = 1,
                WarehouseId = 1
            });

            var createResult = await odooClient.CreateAsync(dictModel);
            createResult.Succeed.Should().BeTrue();


            var lineModel = OdooCreateDictionary.Create(() => new SaleOrderLineOdooDto()
            {
                OrderId = createResult.Value,
                Name = "test line",
                ProductId = 327,
                ProductUomQty = 1,
                PriceUnit = 15
            });

            var createLineResult = await odooClient.CreateAsync(lineModel);
            createLineResult.Succeed.Should().BeTrue();
        }

        #endregion

    }
}