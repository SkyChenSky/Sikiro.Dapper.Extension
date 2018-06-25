using System;
using System.Data;
using System.Linq.Expressions;
using Dapper;
using Sikiro.DapperLambdaExtension.MsSql.Core.Interfaces;
using Sikiro.DapperLambdaExtension.MsSql.Model;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.SetC
{
    /// <summary>
    /// 指令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Command<T> : ICommand<T>
    {
        protected readonly SqlProvider<T> SqlProvider;
        protected readonly IDbConnection DbCon;
        protected DataBaseContext<T> SetContext { get; set; }

        protected Command(IDbConnection conn, SqlProvider<T> sqlProvider)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
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
    }
}
