using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Result;

namespace PortaCapena.OdooJsonRpcClient
{
    /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="OdooRepository"]/*'/>
    public class OdooRepository<T> : IOdooRepository<T> where T : IOdooModel, new()
    {
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="TableName"]/*'/>
        public string TableName { get; }
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="OdooClient"]/*'/>
        protected readonly OdooClient OdooClient;
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="Config"]/*'/>
        public OdooConfig Config => OdooClient.Config;

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="Query"]/*'/>
        public OdooQueryBuilder<T> Query() => new OdooQueryBuilder<T>(OdooClient);

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="ctor"]/*'/>
        public OdooRepository(OdooConfig config) : this(new OdooClient(config))
        {
        }

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="ctor.HttpClient"]/*'/>
        public OdooRepository(OdooConfig config, HttpClient httpClient) : this(new OdooClient(config, httpClient))
        {
        }

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="ctor.OdooClient"]/*'/>
        public OdooRepository(OdooClient odooClient)
        {
            OdooClient = odooClient ?? throw new System.ArgumentNullException(nameof(odooClient));
            TableName = OdooExtensions.GetOdooTableName<T>();
        }

        #region Create

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="CreateAsync.Model"]/*'/>
        public Task<OdooResult<long>> CreateAsync(IOdooCreateModel model, OdooContext context = null)
            => CreateAsync(model, context, CancellationToken.None);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="CreateAsync.Model"]/*'/>
        public Task<OdooResult<long>> CreateAsync(IOdooCreateModel model, OdooContext context, CancellationToken cancellationToken)
            => OdooClient.CreateAsync(model, context, cancellationToken);

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="CreateAsync.Dictionary"]/*'/>
        public Task<OdooResult<long>> CreateAsync(OdooDictionaryModel model, OdooContext context = null)
            => CreateAsync(model, context, CancellationToken.None);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="CreateAsync.Dictionary"]/*'/>
        public Task<OdooResult<long>> CreateAsync(OdooDictionaryModel model, OdooContext context, CancellationToken cancellationToken)
            => OdooClient.CreateAsync(WithTableName(model), context, cancellationToken);

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="CreateAsync.Models"]/*'/>
        public Task<OdooResult<long[]>> CreateAsync(IEnumerable<IOdooCreateModel> models, OdooContext context = null)
            => CreateAsync(models, context, CancellationToken.None);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="CreateAsync.Models"]/*'/>
        public Task<OdooResult<long[]>> CreateAsync(IEnumerable<IOdooCreateModel> models, OdooContext context, CancellationToken cancellationToken)
            => OdooClient.CreateAsync(models, context, cancellationToken);

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="CreateAsync.Dictionaries"]/*'/>
        public Task<OdooResult<long[]>> CreateAsync(IEnumerable<OdooDictionaryModel> models, OdooContext context = null)
            => CreateAsync(models, context, CancellationToken.None);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="CreateAsync.Dictionaries"]/*'/>
        public Task<OdooResult<long[]>> CreateAsync(IEnumerable<OdooDictionaryModel> models, OdooContext context, CancellationToken cancellationToken)
            => OdooClient.CreateAsync(models?.Select(WithTableName).ToList(), context, cancellationToken);

        #endregion

        #region Update

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="UpdateAsync.Model"]/*'/>
        public Task<OdooResult<bool>> UpdateAsync(IOdooCreateModel model, long id, OdooContext context = null)
            => UpdateAsync(model, id, context, CancellationToken.None);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="UpdateAsync.Model"]/*'/>
        public Task<OdooResult<bool>> UpdateAsync(IOdooCreateModel model, long id, OdooContext context, CancellationToken cancellationToken)
            => OdooClient.UpdateAsync(model, id, context, cancellationToken);

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="UpdateRangeAsync.Model"]/*'/>
        public Task<OdooResult<bool>> UpdateRangeAsync(IOdooCreateModel model, long[] ids, OdooContext context = null)
            => UpdateRangeAsync(model, ids, context, CancellationToken.None);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="UpdateRangeAsync.Model"]/*'/>
        public Task<OdooResult<bool>> UpdateRangeAsync(IOdooCreateModel model, long[] ids, OdooContext context, CancellationToken cancellationToken)
            => OdooClient.UpdateRangeAsync(model, ids, context, cancellationToken);

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="UpdateAsync.Dictionary"]/*'/>
        public Task<OdooResult<bool>> UpdateAsync(OdooDictionaryModel model, long id, OdooContext context = null)
            => UpdateAsync(model, id, context, CancellationToken.None);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="UpdateAsync.Dictionary"]/*'/>
        public Task<OdooResult<bool>> UpdateAsync(OdooDictionaryModel model, long id, OdooContext context, CancellationToken cancellationToken)
            => OdooClient.UpdateAsync(WithTableName(model), id, context, cancellationToken);

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="UpdateRangeAsync.Dictionary"]/*'/>
        public Task<OdooResult<bool>> UpdateRangeAsync(OdooDictionaryModel model, long[] ids, OdooContext context = null)
            => UpdateRangeAsync(model, ids, context, CancellationToken.None);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="UpdateRangeAsync.Dictionary"]/*'/>
        public Task<OdooResult<bool>> UpdateRangeAsync(OdooDictionaryModel model, long[] ids, OdooContext context, CancellationToken cancellationToken)
        {
            // kept from 1.0.x: this method always targets the repository table
            if (model != null)
                model.TableName = TableName;
            return OdooClient.UpdateRangeAsync(model, ids, context, cancellationToken);
        }

        #endregion

        #region Delete

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="DeleteAsync.Model"]/*'/>
        public Task<OdooResult<bool>> DeleteAsync(T model, OdooContext context = null)
            => DeleteAsync(model, context, CancellationToken.None);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="DeleteAsync.Model"]/*'/>
        public Task<OdooResult<bool>> DeleteAsync(T model, OdooContext context, CancellationToken cancellationToken)
            => OdooClient.DeleteAsync(model, context, cancellationToken);

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="DeleteAsync.Id"]/*'/>
        public Task<OdooResult<bool>> DeleteAsync(long id, OdooContext context = null)
            => DeleteAsync(id, context, CancellationToken.None);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="DeleteAsync.Id"]/*'/>
        public Task<OdooResult<bool>> DeleteAsync(long id, OdooContext context, CancellationToken cancellationToken)
            => OdooClient.DeleteAsync(TableName, id, context, cancellationToken);

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="DeleteRangeAsync"]/*'/>
        public Task<OdooResult<bool>> DeleteRangeAsync(T[] models, OdooContext context = null)
            => DeleteRangeAsync(models, context, CancellationToken.None);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="DeleteRangeAsync"]/*'/>
        public Task<OdooResult<bool>> DeleteRangeAsync(T[] models, OdooContext context, CancellationToken cancellationToken)
            => OdooClient.DeleteRangeAsync(models?.Cast<IOdooModel>().ToArray(), context, cancellationToken);

        #endregion

        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="ActionAsync"]/*'/>
        public Task<OdooResult<object>> ActionAsync(string action, object args, OdooContext context = null)
            => ActionAsync(action, args, context, CancellationToken.None);
        /// <include file='Docs/OdooRepository.xml' path='docs/member[@name="ActionAsync"]/*'/>
        public Task<OdooResult<object>> ActionAsync(string action, object args, OdooContext context, CancellationToken cancellationToken)
            => OdooClient.ActionAsync(TableName, action, args, context, cancellationToken);

        /// <summary>
        /// Dictionary models without table name are sent to this repository's table.
        /// </summary>
        private OdooDictionaryModel WithTableName(OdooDictionaryModel model)
        {
            if (model != null && model.TableName == null)
                model.TableName = TableName;
            return model;
        }
    }
}
