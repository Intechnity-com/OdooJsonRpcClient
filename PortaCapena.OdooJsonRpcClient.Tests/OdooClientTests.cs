using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Tests.Models;
using PortaCapena.OdooJsonRpcClient.Tests.Models.Create;
using PortaCapena.OdooJsonRpcClient.Utils;
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

            var products = await odooClient.GetAsync<OdooProductProduct>();

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().BeGreaterThan(0);
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Get_product_by_Id_test()
        {
            var odooClient = new OdooClient(Config);

            var query = OdooQuery<OdooProductProduct>.Create()
                .Where(x => x.Id, OdooOperator.EqualsTo, 67);

            var products = await odooClient.GetAsync<OdooProductProduct>(query);

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().Be(1);
            products.Succeed.Should().BeTrue();
        }


        [Fact]
        public async Task Get_products_with_take_query_test()
        {
            var odooClient = new OdooClient(Config);
            var query = OdooQuery<OdooProductProduct>.Create()
                .Take(10);

            var products = await odooClient.GetAsync<OdooProductProduct>(query);

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().Be(10);
            products.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task Get_products_with_skip_query_test()
        {
            var odooClient = new OdooClient(Config);
            var query = OdooQuery<OdooProductProduct>.Create()
                .Skip(5);

            var products = await odooClient.GetAsync<OdooProductProduct>(query);

            var allProducts = await odooClient.GetAsync<OdooProductProduct>();

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
            var query = OdooQuery<OdooProductProduct>.Create()
                .OrderBy(x => x.Id);

            var products = await odooClient.GetAsync<OdooProductProduct>(query);

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
            var query = OdooQuery<OdooProductProduct>.Create()
                .OrderByDescending(x => x.Id);

            var products = await odooClient.GetAsync<OdooProductProduct>(query);

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

            var filters = OdooQuery<OdooProductProduct>.Create()
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.WriteDate
                })
                .Where(x => x.Name, OdooOperator.EqualsTo, "Bioboxen 610l")
                .Where(x => x.WriteDate, OdooOperator.GreaterThanOrEqualTo, new DateTime(2020, 12, 2));

            var products = await odooClient.GetAsync<OdooProductProduct>(filters);

            products.Error.Should().BeNull();
            products.Value.Should().NotBeNull();
            products.Value.Length.Should().Be(1);
            products.Succeed.Should().BeTrue();
        }


        [Fact]
        public async Task Get_DotNet_model_should_return_string()
        {
            var odooClient = new OdooClient(Config);
            var tableName = "product.product";
            var modelResult = await odooClient.GetModelAsync(tableName);

            var model = OdooModelMapper.GetDotNetModel(tableName, modelResult.Value);
        }


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

            var query = OdooQuery<OdooProductProduct>.Create()
                .Where(x => x.Id, OdooOperator.EqualsTo, createdProductId);

            var products = await odooClient.GetAsync<OdooProductProduct>(query);
            products.Succeed.Should().BeTrue();
            products.Value.Length.Should().Be(1);
            products.Value.First().Name.Should().Be(model.Name);


            model.Name += " update";

            var updateProductResult = await odooClient.UpdateAsync(model, createdProductId);
            updateProductResult.Succeed.Should().BeTrue();

            var query2 = OdooQuery<OdooProductProduct>.Create()
                .Where(x => x.Id, OdooOperator.EqualsTo, createdProductId);

            var products2 = await odooClient.GetAsync<OdooProductProduct>(query2);
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

            var dictModel = OdooCreateDictionary.Create(() => new OdooProductProduct
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

            var query = OdooQuery<OdooProductProduct>.Create().ById(createdProductId);

            var product = await odooClient.GetAsync<OdooProductProduct>(query);
            product.Succeed.Should().BeTrue();
            product.Value.First().Name.Should().Be(model.Name);

            var deleteProductResult = await odooClient.DeleteAsync(product.Value.First());
            deleteProductResult.Succeed.Should().BeTrue();
            deleteProductResult.Value.Should().BeTrue();
        }


        // [Fact(Skip = "Test for working on Odoo")]
        //[Fact]
        //public async Task Create_product_and_delete_object_test()
        //{
        //    var odooClient = new OdooClient(_config);

        //    var model = new OdooCreateProduct()
        //    {
        //        Name = "Prod test Kg",
        //        UomId = 3,
        //        UomPoId = 3
        //    };

        //    var createResult = await odooClient.CreateAsync(model);
        //    createResult.Succeed.Should().BeTrue();
        //    var createdProductId = createResult.Value;

        //    var query = OdooQuery<OdooProductProduct>.Create().ById(createdProductId).Build();

        //    var product = await odooClient.GetAsync<OdooProductProduct>(query);
        //    product.Succeed.Should().BeTrue();
        //    product.Value.First().Name.Should().Be(model.Name);

        //    var deleteProductResult = await odooClient.DeleteAsync(product.Value.First());
        //    deleteProductResult.Succeed.Should().BeTrue();
        //    deleteProductResult.Value.Should().BeTrue();
        //}


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
    }
}