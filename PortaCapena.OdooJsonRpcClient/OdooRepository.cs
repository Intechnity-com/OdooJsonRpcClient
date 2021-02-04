using System.Collections.Generic;
using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Result;

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
        public async Task<OdooResult<long>> CreateAsync(OdooCreateDictionary model)
        {
            return await OdooClient.CreateAsync(model);
        }

        public async Task<OdooResult<bool>> UpdateAsync(IOdooCreateModel model, long id)
        {
            return await OdooClient.UpdateAsync(model, id);
        }
        public async Task<OdooResult<bool>> UpdateRangeAsync(IOdooCreateModel model, long[] ids)
        {
            return await OdooClient.UpdateRangeAsync(model, ids);
        }

        public async Task<OdooResult<bool>> UpdateAsync(OdooCreateDictionary model, long id)
        {
            return await OdooClient.UpdateAsync(model, id);
        }
        public async Task<OdooResult<bool>> UpdateRangeAsync(OdooCreateDictionary model, long[] ids)
        {
            model.TableName = TableName;
            return await OdooClient.UpdateRangeAsync(model, ids);
        }

        public async Task<OdooResult<bool>> DeleteAsync(T model)
        {
            return await OdooClient.DeleteAsync(model);
        }
        public async Task<OdooResult<bool>> DeleteAsync(long id)
        {
            return await OdooClient.DeleteAsync(TableName, id);
        }
        public async Task<OdooResult<bool>> DeleteRangeAsync(T[] models)
        {
            return await OdooClient.DeleteRangeAsync(models as IOdooModel[]);
        }
    }
}