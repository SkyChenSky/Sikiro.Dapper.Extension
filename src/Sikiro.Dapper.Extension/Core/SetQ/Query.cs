using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Sikiro.Dapper.Extension.Core.Interfaces;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.Core.SetQ
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Query<T> : IQuery<T>, IUpdateSelect<T>
    {
        protected readonly SqlProvider SqlProvider;
        protected readonly IDbConnection DbCon;
        protected readonly IDbTransaction DbTransaction;

        public LambdaExpression WhereExpression { get; set; }
        public LambdaExpression IfNotExistsExpression { get; set; }
        public Dictionary<EOrderBy, LambdaExpression> OrderbyExpressionList { get; set; }
        public LambdaExpression SelectExpression { get; set; }

        protected DataBaseContext<T> SetContext { get; set; }

        protected Query(IDbConnection conn, SqlProvider sqlProvider)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
        }

        protected Query(IDbConnection conn, SqlProvider sqlProvider, IDbTransaction dbTransaction)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
            DbTransaction = dbTransaction;
        }

        public T Get()
        {
            SqlProvider.FormatGet<T>();

            return DbCon.QueryFirstOrDefault<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public async Task<T> GetAsync()
        {
            SqlProvider.FormatGet<T>();

            return await DbCon.QueryFirstOrDefaultAsync<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public IEnumerable<T> ToList()
        {
            SqlProvider.FormatToList<T>();

            return DbCon.Query<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public async Task<IEnumerable<T>> ToListAsync()
        {
            SqlProvider.FormatToList<T>();

            return await DbCon.QueryAsync<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public PageList<T> PageList(int pageIndex, int pageSize)
        {
            SqlProvider.FormatToPageList<T>(pageIndex, pageSize);

            using (var queryResult = DbCon.QueryMultiple(SqlProvider.SqlString, SqlProvider.Params, DbTransaction))
            {
                var pageTotal = queryResult.ReadFirst<int>();

                var itemList = queryResult.Read<T>().ToList();

                return new PageList<T>(pageIndex, pageSize, pageTotal, itemList);
            }
        }

        public List<T> UpdateSelect(Expression<Func<T, T>> updator)
        {
            SqlProvider.FormatUpdateSelect(updator);

            return DbCon.Query<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction).ToList();
        }

        public async Task<IEnumerable<T>> UpdateSelectAsync(Expression<Func<T, T>> updator)
        {
            SqlProvider.FormatUpdateSelect(updator);

            return await DbCon.QueryAsync<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }
    }
}
