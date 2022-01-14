using System;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example
{
    public class ProductTemplateOdooModelRepositoryTests : RequestTestBase
    {
        private class ProductTemplateRepository : OdooRepository<ProductTemplateOdooModel>
        {
            public ProductTemplateRepository() : base(TestConfig) { }
        }

        [Fact]
        public async Task Get_All()
        {
            var repo = new ProductTemplateRepository();

            var result = await repo.Query().ToListAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
            result.Value.Should().NotBeNull().And.NotBeEmpty();
        }

        [Fact]
        public async Task Get_By_Id()
        {
            var repo = new ProductTemplateRepository();

            var result = await repo.Query().ById(1).FirstOrDefaultAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_By_Ids()
        {
            var repo = new ProductTemplateRepository();

            var result = await repo.Query().ByIds(1, 2).ToListAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
            result.Value.Should().NotBeNull().And.NotBeEmpty();
            result.Value.Length.Should().Be(2);
        }



        [Fact]
        public async Task Get_name_in_nl()
        {
            var repo = new ProductTemplateRepository();

            var result = await repo.Query().ById(23).FirstOrDefaultAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Name.Should().Be("Acoustic Bloc Screens");
            result.Value.Name.Should().NotBe("Akoestische blokschermen");


            repo.Config.Context.Language = "nl_NL";

            var resultInNL = await repo.Query().ById(result.Value.Id).Select(x => new { x.Name}).FirstOrDefaultAsync();

            resultInNL.Error.Should().BeNull();
            resultInNL.Succeed.Should().BeTrue();
            resultInNL.Value.Should().NotBeNull();

            resultInNL.Value.Name.Should().NotBe("Acoustic Bloc Screens");
            resultInNL.Value.Name.Should().Be("Akoestische blokschermen");

            repo.Config.Context.Language = "nl_NL";

            var resultInEn = await repo.Query().ById(result.Value.Id).FirstOrDefaultAsync();

            resultInEn.Error.Should().BeNull();
            resultInEn.Succeed.Should().BeTrue();
            resultInEn.Value.Should().NotBeNull();

            result.Value.Name.Should().Be("Acoustic Bloc Screens");
            result.Value.Name.Should().NotBe("Akoestische blokschermen");
        }

        //[Fact]
        [Fact(Skip = "Test for working on Odoo")]
        public async Task Update_name_in_nl()
        {
            var repo = new ProductTemplateRepository();

            var result = await repo.Query().ById(15).FirstOrDefaultAsync();

            result.Error.Should().BeNull();
            result.Succeed.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Name.Should().Be("Acoustic Bloc Screens");
            result.Value.Name.Should().NotBe("Akoestische blokschermen");

            var model = OdooDictionaryModel.Create<ProductProductOdooModel>().Add(x => x.Name, "Product new name NL");

            repo.Config.Context.Language = "nl_BE";

            var updateResult = await repo.UpdateAsync(model, result.Value.Id);

            updateResult.Error.Should().BeNull();
            updateResult.Succeed.Should().BeTrue();
            updateResult.Value.Should().BeTrue();

            var resultInNL = await repo.Query().ById(result.Value.Id).FirstOrDefaultAsync();

            resultInNL.Error.Should().BeNull();
            resultInNL.Succeed.Should().BeTrue();
            resultInNL.Value.Should().NotBeNull();

            result.Value.Name.Should().NotBe("Acoustic Bloc Screens");
            result.Value.Name.Should().Be("Akoestische blokschermen");


            repo.Config.Context.Language = "en_US";

            var resultInEn = await repo.Query().ById(result.Value.Id).FirstOrDefaultAsync();

            resultInEn.Error.Should().BeNull();
            resultInEn.Succeed.Should().BeTrue();
            resultInEn.Value.Should().NotBeNull();

            result.Value.Name.Should().Be("Acoustic Bloc Screens");
            result.Value.Name.Should().NotBe("Akoestische blokschermen");
        }
    }
}