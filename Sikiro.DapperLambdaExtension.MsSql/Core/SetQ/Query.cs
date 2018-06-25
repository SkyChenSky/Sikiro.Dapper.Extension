using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using Sikiro.DapperLambdaExtension.MsSql.Core.Interfaces;
using Sikiro.DapperLambdaExtension.MsSql.Model;
using Sikiro.Tookits.Base;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.SetQ
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Query<T> : IQuery<T>, IUpdateSelect<T>
    {
        protected readonly SqlProvider<T> SqlProvider;
        protected readonly IDbConnection DbCon;
        protected DataBaseContext<T> SetContext { get; set; }

        protected Query(IDbConnection conn, SqlProvider<T> sqlProvider)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
        }

        public T Get()
        {
            SqlProvider.FormatGet();

            return DbCon.QuerySingle<T>(SqlProvider.SqlString, SqlProvider.Params);
        }

        public List<T> ToList()
        {
            SqlProvider.FormatToList();

            return DbCon.Query<T>(SqlProvider.SqlString, SqlProvider.Params).ToList();
        }

        public PageList<T> PageList(int pageIndex, int pageSize)
        {
            SqlProvider.FormatToPageList(pageIndex, pageSize);

            using (var queryResult = DbCon.QueryMultiple(SqlProvider.SqlString, SqlProvider.Params))
            {
                var pageTotal = queryResult.ReadFirst<int>();

                var itemList = queryResult.Read<T>().ToList();

                return new PageList<T>(pageIndex, pageSize, pageTotal, itemList);
            }
        }

        public List<T> UpdateSelect(Expression<Func<T, T>> updator)
        {
            SqlProvider.FormatUpdateSelect(updator);

            return DbCon.Query<T>(SqlProvider.SqlString, SqlProvider.Params).ToList();
        }
    }
}
