using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Dapper;
using Sikiro.DapperLambdaExtension.MsSql.Core.Interfaces;
using Sikiro.DapperLambdaExtension.MsSql.Helper;

namespace Sikiro.DapperLambdaExtension.MsSql.Core
{
    /// <summary>
    /// 指令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Command<T> : Order<T>, ICommand<T>
    {
        protected Command(IDbConnection conn, SqlProvider<T> sqlProvider) : base(conn, sqlProvider)
        {

        }

        public int Update(T entity)
        {
            SqlProvider.FormatUpdate(a => entity);

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params);
        }

        public int Update(Expression<Func<T, T>> updateExpression)
        {
            SqlProvider.FormatUpdate(updateExpression);

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params);
        }

        public int Delete()
        {
            SqlProvider.FormatDelete();

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params);
        }

        public int Insert(T entity)
        {
            SqlProvider.FormatInsert(entity);

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params);
        }

        public void BatchInsert(IEnumerable<T> entities, int timeout = 120)
        {
            SqlHelper.BulkCopy(DbCon, entities);
        }
    }
}
