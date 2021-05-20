using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Result;

namespace PortaCapena.OdooJsonRpcClient
{
    public interface IOdooRepository<T> where T : IOdooModel, new()
    {
        OdooQueryBuilder<T> Query();

        Task<OdooResult<long>> CreateAsync(IOdooCreateModel model, OdooContext context = null);
        Task<OdooResult<long>> CreateAsync(OdooDictionaryModel model, OdooContext context = null);

        Task<OdooResult<bool>> UpdateAsync(IOdooCreateModel model, long id, OdooContext context = null);
        Task<OdooResult<bool>> UpdateRangeAsync(IOdooCreateModel model, long[] ids, OdooContext context = null);
        Task<OdooResult<bool>> UpdateAsync(OdooDictionaryModel model, long id, OdooContext context = null);
        Task<OdooResult<bool>> UpdateRangeAsync(OdooDictionaryModel model, long[] ids, OdooContext context = null);

        Task<OdooResult<bool>> DeleteAsync(T model, OdooContext context = null);
        Task<OdooResult<bool>> DeleteAsync(long id, OdooContext context = null);
        Task<OdooResult<bool>> DeleteRangeAsync(T[] models, OdooContext context = null);
    }
}