using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared;
using PortaCapena.OdooJsonRpcClient.Shared.Models;

namespace PortaCapena.OdooJsonRpcClient.Example
{
    public class CompanyOdooRepository : TestBase
    {
        public class CompanyRepository : OdooRepository<ResCompanyOdooModel>
        {
            public CompanyRepository(OdooConfig conf) : base(conf)
            {
            }
        }



    }
}