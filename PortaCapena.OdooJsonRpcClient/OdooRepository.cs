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
        public OdooConfig Config => OdooClient.Config;

        public OdooQueryBuilder<T> Query() => new OdooQueryBuilder<T>(OdooClient);

        public OdooRepository(OdooConfig config)
        {
            OdooClient = new OdooClient(config);
            TableName = OdooExtensions.GetOdooTableName<T>();
        }

        public async Task<OdooResult<long>> CreateAsync(IOdooCreateModel model, OdooContext context = null)
        {
            return await OdooClient.CreateAsync(model, context);
        }
        public async Task<OdooResult<long>> CreateAsync(OdooDictionaryModel model, OdooContext context = null)
        {
            return await OdooClient.CreateAsync(model, context);
        }

        public async Task<OdooResult<bool>> UpdateAsync(IOdooCreateModel model, long id, OdooContext context = null)
        {
            return await OdooClient.UpdateAsync(model, id, context);
        }
        public async Task<OdooResult<bool>> UpdateRangeAsync(IOdooCreateModel model, long[] ids, OdooContext context = null)
        {
            return await OdooClient.UpdateRangeAsync(model, ids, context);
        }
        public async Task<OdooResult<bool>> UpdateAsync(OdooDictionaryModel model, long id, OdooContext context = null)
        {
            return await OdooClient.UpdateAsync(model, id, context);
        }
        public async Task<OdooResult<bool>> UpdateRangeAsync(OdooDictionaryModel model, long[] ids, OdooContext context = null)
        {
            model.TableName = TableName;
            return await OdooClient.UpdateRangeAsync(model, ids, context);
        }

        public async Task<OdooResult<bool>> DeleteAsync(T model, OdooContext context = null)
        {
            return await OdooClient.DeleteAsync(model, context);
        }
        public async Task<OdooResult<bool>> DeleteAsync(long id, OdooContext context = null)
        {
            return await OdooClient.DeleteAsync(TableName, id, context);
        }
        public async Task<OdooResult<bool>> DeleteRangeAsync(T[] models, OdooContext context = null)
        {
            return await OdooClient.DeleteRangeAsync(models as IOdooModel[], context);
        }
        public async Task<OdooResult<Dictionary<string, object>>> UpdateProductQuantity(long id, OdooContext context = null)
        {
            return await OdooClient.ChangeProductQuantity(id, context);
        }
    }
}