using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Result;

namespace PortaCapena.OdooJsonRpcClient
{
    /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="IOdooRepository"]/*'/>
    public interface IOdooRepository<T> where T : IOdooModel, new()
    {
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="Query"]/*'/>
        OdooQueryBuilder<T> Query();

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="CreateAsync.Model"]/*'/>
        Task<OdooResult<long>> CreateAsync(IOdooCreateModel model, OdooContext context = null);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="CreateAsync.Dictionary"]/*'/>
        Task<OdooResult<long>> CreateAsync(OdooDictionaryModel model, OdooContext context = null);

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="UpdateAsync.Model"]/*'/>
        Task<OdooResult<bool>> UpdateAsync(IOdooCreateModel model, long id, OdooContext context = null);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="UpdateRangeAsync.Model"]/*'/>
        Task<OdooResult<bool>> UpdateRangeAsync(IOdooCreateModel model, long[] ids, OdooContext context = null);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="UpdateAsync.Dictionary"]/*'/>
        Task<OdooResult<bool>> UpdateAsync(OdooDictionaryModel model, long id, OdooContext context = null);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="UpdateRangeAsync.Dictionary"]/*'/>
        Task<OdooResult<bool>> UpdateRangeAsync(OdooDictionaryModel model, long[] ids, OdooContext context = null);

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="DeleteAsync.Model"]/*'/>
        Task<OdooResult<bool>> DeleteAsync(T model, OdooContext context = null);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="DeleteAsync.Id"]/*'/>
        Task<OdooResult<bool>> DeleteAsync(long id, OdooContext context = null);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="DeleteRangeAsync"]/*'/>
        Task<OdooResult<bool>> DeleteRangeAsync(T[] models, OdooContext context = null);
    }
}
