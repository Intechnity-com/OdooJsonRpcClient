using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Result;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    public class OdooQueryBuilder<T> where T : IOdooModel, new()
    {
        private readonly OdooClient _odooClient;
        private readonly OdooQuery<T> _query;

        internal OdooQueryBuilder(OdooClient odooClient) : base()
        {
            _odooClient = odooClient;
            _query = new OdooQuery<T>();
        }

        #region Select

        public OdooQueryBuilder<T> Select(Func<T, object> selector)
        {
            _query.Select(selector);
            return this;
        }

        public OdooQueryBuilder<T> Select(params string[] fields)
        {
            _query.Select(fields);
            return this;
        }

        public OdooQueryBuilder<T> SelectSimplifiedModel()
        {
            _query.SelectSimplifiedModel();
            return this;
        }

        #endregion

        #region Where
        public OdooQueryBuilder<T> Where(OdooFilter filter)
        {
            _query.Where(filter);
            return this;
        }
        public OdooQueryBuilder<T> Where(Expression<Func<T, object>> expression, OdooOperator odooOperator, object value)
        {
            _query.Where(expression, odooOperator, value);
            return this;
        }
        public OdooQueryBuilder<T> Where<TForeignKeyLevel1>(Expression<Func<T, long>> expression, Expression<Func<TForeignKeyLevel1, object>> expressionForeignKeyLevel1, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new()
        {
            _query.Where(expression, expressionForeignKeyLevel1, odooOperator, value);
            return this;
        }
        public OdooQueryBuilder<T> Where<TForeignKeyLevel1>(Expression<Func<T, long?>> expression, Expression<Func<TForeignKeyLevel1, object>> expressionForeignKeyLevel1, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new()
        {
            _query.Where(expression, expressionForeignKeyLevel1, odooOperator, value);
            return this;
        }

        public OdooQueryBuilder<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long>> expression, Expression<Func<TForeignKeyLevel1, long>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            _query.Where(expression, expressionForeignKeyLevel1, expressionForeignKeyLevel2, odooOperator, value);
            return this;
        }
        public OdooQueryBuilder<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long?>> expression, Expression<Func<TForeignKeyLevel1, long?>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            _query.Where(expression, expressionForeignKeyLevel1, expressionForeignKeyLevel2, odooOperator, value);
            return this;
        }
        public OdooQueryBuilder<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long>> expression, Expression<Func<TForeignKeyLevel1, long?>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            _query.Where(expression, expressionForeignKeyLevel1, expressionForeignKeyLevel2, odooOperator, value);
            return this;
        }
        public OdooQueryBuilder<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long?>> expression, Expression<Func<TForeignKeyLevel1, long>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            _query.Where(expression, expressionForeignKeyLevel1, expressionForeignKeyLevel2, odooOperator, value);
            return this;
        }

        public OdooQueryBuilder<T> ById(long id)
        {
            _query.ById(id);
            return this;
        }
        public OdooQueryBuilder<T> ByIds(params long[] ids)
        {
            _query.ByIds(ids);
            return this;
        }

        #endregion

        #region Skip

        public OdooQueryBuilder<T> Take(int limit)
        {
            _query.Take(limit);
            return this;
        }
        public OdooQueryBuilder<T> Skip(int offset)
        {
            _query.Skip(offset);
            return this;
        }

        #endregion

        #region Order

        public OdooQueryBuilder<T> OrderBy(Expression<Func<T, object>> expression)
        {
            _query.OrderBy(expression);
            return this;
        }
        public OdooQueryBuilder<T> OrderByDescending(Expression<Func<T, object>> expression)
        {
            _query.OrderByDescending(expression);
            return this;
        }
        public OdooQueryBuilder<T> ThenOrderBy(Expression<Func<T, object>> expression)
        {
            _query.ThenOrderBy(expression);
            return this;
        }
        public OdooQueryBuilder<T> ThenOrderByDescending(Expression<Func<T, object>> expression)
        {
            _query.ThenOrderByDescending(expression);
            return this;
        }

        #endregion


        public async Task<OdooResult<T[]>> ToListAsync()
        {
            return await _odooClient.GetAsync<T>(_query);
        }

        public async Task<OdooResult<T>> FirstAsync()
        {
            Take(1);
            var result = await ToListAsync();
            return result.Succeed ? result.ToResult(result.Value.First()) : OdooResult<T>.FailedResult(result);
        }

        public async Task<OdooResult<T>> FirstOrDefaultAsync()
        {
            Take(1);
            var result = await ToListAsync();
            return result.Succeed ? result.ToResult(result.Value.FirstOrDefault()) : OdooResult<T>.FailedResult(result);
        }

        public async Task<OdooResult<T>> SingleAsync()
        {
            Take(2);
            var result = await ToListAsync();
            return result.Succeed ? result.ToResult(result.Value.Single()) : OdooResult<T>.FailedResult(result);
        }

        public async Task<OdooResult<long>> CountAsync()
        {
            return await _odooClient.GetCountAsync<T>(_query);
        }
        public async Task<OdooResult<bool>> AnyAsync()
        {
            Take(1);
            var result = await ToListAsync();
            return result.Succeed ? result.ToResult(result.Value.Any()) : OdooResult<bool>.FailedResult(result);
        }
    }
}