using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Result;
using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient
{
    public class OdooRepository<T> : IOdooRepository<T> where T : IOdooModel, new()
    {
        public string TableName { get; }
        protected readonly OdooClient OdooClient;

        public OdooRepository(OdooConfig config)
        {
            OdooClient = new OdooClient(config);
            TableName = OdooExtensions.GetOdooTableName<T>();
        }

        public OdooQueryBuilder<T> Query()
        {
            return new OdooQueryBuilder<T>(OdooClient);
        }

        public async Task<OdooResult<long>> CreateAsync(IOdooCreateModel model)
        {
            return await OdooClient.CreateAsync(model);
        }

        public async Task<OdooResult<bool>> UpdateAsync(long id, IOdooCreateModel model)
        {
            return await OdooClient.UpdateAsync(model, id);
        }

        public async Task<OdooResult<bool>> DeleteAsync(long id)
        {
            return await OdooClient.DeleteAsync(TableName, id);
        }
    }
}