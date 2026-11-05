using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Example.Api
{
    /// <summary>
    /// Account codes on Odoo 19 (docker/odoo19) with the JSON API (issue #121).
    /// Since Odoo 18 the code is company dependent - it is returned only for the company set in the context.
    /// </summary>
    public class AccountIntegrationTestsJsonApi : RequestTestBase
    {
        [Fact]
        public async Task Account_codes_are_returned_for_company_in_context()
        {
            var client = new OdooClient(JsonApiTestConfig);
            var companies = await client.GetAsync("res.company");
            companies.Succeed.Should().BeTrue(companies.Message);

            var accountsRead = 0;
            foreach (var companyId in companies.Value.Select(x => (long)x["id"]))
            {
                var query = new OdooQuery { Filters = OdooFilter.Create().EqualTo("company_ids", companyId) };
                var accounts = await client.GetAsync<IntegrationAccountOdooModel>(query, new OdooContext { AllowedCompanyId = companyId });

                accounts.Succeed.Should().BeTrue(accounts.Message);
                accounts.Value.Where(x => string.IsNullOrEmpty(x.Code)).Should().BeEmpty();
                accountsRead += accounts.Value.Length;
            }

            accountsRead.Should().BePositive();
        }
    }
}
