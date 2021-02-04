using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Result;

namespace PortaCapena.OdooJsonRpcClient
{
    public interface IOdooRepository<T> where T : IOdooModel, new()
    {
        OdooQueryBuilder<T> Query();

        Task<OdooResult<long>> CreateAsync(IOdooCreateModel model);
        Task<OdooResult<long>> CreateAsync(OdooCreateDictionary model);

        Task<OdooResult<bool>> UpdateAsync(IOdooCreateModel model, long id);
        Task<OdooResult<bool>> UpdateRangeAsync(IOdooCreateModel model, long[] ids);
        Task<OdooResult<bool>> UpdateAsync(OdooCreateDictionary model, long id);
        Task<OdooResult<bool>> UpdateRangeAsync(OdooCreateDictionary model, long[] ids);

        Task<OdooResult<bool>> DeleteAsync(T model);
        Task<OdooResult<bool>> DeleteAsync(long id);
        Task<OdooResult<bool>> DeleteRangeAsync(T[] models);
    }
}