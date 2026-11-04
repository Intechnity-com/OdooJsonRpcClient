using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Result;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OdooQueryBuilder"]/*'/>
    public class OdooQueryBuilder<T> where T : IOdooModel, new()
    {
        private readonly OdooClient _odooClient;
        private readonly OdooQuery<T> _query;

        private OdooContext _odooContext;

        internal OdooQueryBuilder(OdooClient odooClient) : base()
        {
            _odooClient = odooClient;
            _query = new OdooQuery<T>();
        }

        #region Select

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Select.Selector"]/*'/>
        public OdooQueryBuilder<T> Select(Func<T, object> selector)
        {
            _query.Select(selector);
            return this;
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Select.Fields"]/*'/>
        public OdooQueryBuilder<T> Select(params string[] fields)
        {
            _query.Select(fields);
            return this;
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="SelectSimplifiedModel"]/*'/>
        public OdooQueryBuilder<T> SelectSimplifiedModel()
        {
            _query.SelectSimplifiedModel();
            return this;
        }

        #endregion

        #region Where
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.Filter"]/*'/>
        public OdooQueryBuilder<T> Where(OdooFilter filter)
        {
            _query.Where(filter);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.Expression"]/*'/>
        public OdooQueryBuilder<T> Where(Expression<Func<T, object>> expression, OdooOperator odooOperator, object value)
        {
            _query.Where(expression, odooOperator, value);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.ForeignKeyLevel1"]/*'/>
        public OdooQueryBuilder<T> Where<TForeignKeyLevel1>(Expression<Func<T, long>> expression, Expression<Func<TForeignKeyLevel1, object>> expressionForeignKeyLevel1, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new()
        {
            _query.Where(expression, expressionForeignKeyLevel1, odooOperator, value);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.ForeignKeyLevel1"]/*'/>
        public OdooQueryBuilder<T> Where<TForeignKeyLevel1>(Expression<Func<T, long?>> expression, Expression<Func<TForeignKeyLevel1, object>> expressionForeignKeyLevel1, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new()
        {
            _query.Where(expression, expressionForeignKeyLevel1, odooOperator, value);
            return this;
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.ForeignKeyLevel2"]/*'/>
        public OdooQueryBuilder<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long>> expression, Expression<Func<TForeignKeyLevel1, long>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            _query.Where(expression, expressionForeignKeyLevel1, expressionForeignKeyLevel2, odooOperator, value);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.ForeignKeyLevel2"]/*'/>
        public OdooQueryBuilder<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long?>> expression, Expression<Func<TForeignKeyLevel1, long?>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            _query.Where(expression, expressionForeignKeyLevel1, expressionForeignKeyLevel2, odooOperator, value);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.ForeignKeyLevel2"]/*'/>
        public OdooQueryBuilder<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long>> expression, Expression<Func<TForeignKeyLevel1, long?>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            _query.Where(expression, expressionForeignKeyLevel1, expressionForeignKeyLevel2, odooOperator, value);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.ForeignKeyLevel2"]/*'/>
        public OdooQueryBuilder<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long?>> expression, Expression<Func<TForeignKeyLevel1, long>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            _query.Where(expression, expressionForeignKeyLevel1, expressionForeignKeyLevel2, odooOperator, value);
            return this;
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="ById"]/*'/>
        public OdooQueryBuilder<T> ById(long id)
        {
            _query.ById(id);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="ByIds"]/*'/>
        public OdooQueryBuilder<T> ByIds(params long[] ids)
        {
            _query.ByIds(ids);
            return this;
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="WithContext"]/*'/>
        public OdooQueryBuilder<T> WithContext(OdooContext context)
        {
            _odooContext = new OdooContext(context);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="WithContext.KeyValue"]/*'/>
        public OdooQueryBuilder<T> WithContext(string key, object value)
        {
            if (_odooContext == null) _odooContext = new OdooContext();
            _odooContext[key] = value;
            return this;
        }

        #endregion

        #region Skip

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Take"]/*'/>
        public OdooQueryBuilder<T> Take(int limit)
        {
            _query.Take(limit);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Skip"]/*'/>
        public OdooQueryBuilder<T> Skip(int offset)
        {
            _query.Skip(offset);
            return this;
        }

        #endregion

        #region Order

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OrderBy"]/*'/>
        public OdooQueryBuilder<T> OrderBy(Expression<Func<T, object>> expression)
        {
            _query.OrderBy(expression);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OrderByDescending"]/*'/>
        public OdooQueryBuilder<T> OrderByDescending(Expression<Func<T, object>> expression)
        {
            _query.OrderByDescending(expression);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="ThenOrderBy"]/*'/>
        public OdooQueryBuilder<T> ThenOrderBy(Expression<Func<T, object>> expression)
        {
            _query.ThenOrderBy(expression);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="ThenOrderByDescending"]/*'/>
        public OdooQueryBuilder<T> ThenOrderByDescending(Expression<Func<T, object>> expression)
        {
            _query.ThenOrderByDescending(expression);
            return this;
        }

        #endregion


        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="ToListAsync"]/*'/>
        public Task<OdooResult<T[]>> ToListAsync(CancellationToken cancellationToken = default)
        {
            return _odooClient.GetAsync<T>(_query, _odooContext, cancellationToken);
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="FirstAsync"]/*'/>
        public async Task<OdooResult<T>> FirstAsync(CancellationToken cancellationToken = default)
        {
            var result = await ToListWithLimitAsync(1, cancellationToken).ConfigureAwait(false);
            return result.Succeed ? result.ToResult(result.Value.First()) : OdooResult<T>.FailedResult(result);
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="FirstOrDefaultAsync"]/*'/>
        public async Task<OdooResult<T>> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            var result = await ToListWithLimitAsync(1, cancellationToken).ConfigureAwait(false);
            return result.Succeed ? result.ToResult(result.Value.FirstOrDefault()) : OdooResult<T>.FailedResult(result);
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="SingleAsync"]/*'/>
        public async Task<OdooResult<T>> SingleAsync(CancellationToken cancellationToken = default)
        {
            var result = await ToListWithLimitAsync(2, cancellationToken).ConfigureAwait(false);
            return result.Succeed ? result.ToResult(result.Value.Single()) : OdooResult<T>.FailedResult(result);
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="CountAsync"]/*'/>
        public Task<OdooResult<long>> CountAsync(CancellationToken cancellationToken = default)
        {
            return _odooClient.GetCountAsync<T>(_query, _odooContext, cancellationToken);
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="AnyAsync"]/*'/>
        public async Task<OdooResult<bool>> AnyAsync(CancellationToken cancellationToken = default)
        {
            var result = await ToListWithLimitAsync(1, cancellationToken).ConfigureAwait(false);
            return result.Succeed ? result.ToResult(result.Value.Any()) : OdooResult<bool>.FailedResult(result);
        }

        /// <summary>
        /// Runs the query with a limit applied to a copy, so the builder can be reused afterwards.
        /// </summary>
        private Task<OdooResult<T[]>> ToListWithLimitAsync(int limit, CancellationToken cancellationToken)
        {
            var query = new OdooQuery
            {
                Filters = _query.Filters,
                ReturnFields = _query.ReturnFields,
                Offset = _query.Offset,
                Order = _query.Order,
                Limit = _query.Limit.HasValue ? Math.Min(_query.Limit.Value, limit) : limit
            };
            return _odooClient.GetAsync<T>(query, _odooContext, cancellationToken);
        }
    }
}
