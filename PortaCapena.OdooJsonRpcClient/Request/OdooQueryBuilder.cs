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
        private readonly OdooRepository<T> _odooRepository;
        private readonly OdooQuery<T> _query;

        internal OdooQueryBuilder(OdooRepository<T> odooRepository) : base()
        {
            _odooRepository = odooRepository;
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

        public OdooQueryBuilder<T> ById(long id)
        {
            _query.ById(id);
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
            return await _odooRepository.GetAsync(_query);
        }

        public async Task<OdooResult<T>> FirstAsync()
        {
            Take(1);
            var result = await _odooRepository.GetAsync(_query);
            return result.Succeed ? result.ToResult(result.Value.First()) : OdooResult<T>.FailedResult(result);
        }

        public async Task<OdooResult<T>> FirstOrDefaultAsync()
        {
            Take(1);
            var result = await _odooRepository.GetAsync(_query);
            return result.Succeed ? result.ToResult(result.Value.FirstOrDefault()) : OdooResult<T>.FailedResult(result);
        }

        public async Task<OdooResult<int>> CountAsync()
        {
            //   return await  _odooRepository.GetAsync(this);
            throw new NotImplementedException();
        }
        public async Task<OdooResult<bool>> AnyAsync()
        {
            //  return await  _odooRepository.GetAsync(this);
            throw new NotImplementedException();
        }
    }
}