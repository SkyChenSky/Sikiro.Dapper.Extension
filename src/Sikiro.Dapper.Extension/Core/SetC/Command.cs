using System;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Sikiro.Dapper.Extension.Core.Interfaces;

namespace Sikiro.Dapper.Extension.Core.SetC
{
    /// <summary>
    /// 指令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Command<T> : AbstractSet, ICommand<T>, IInsert<T>
    {
        protected Command(SqlProvider sqlProvider, IDbConnection dbCon, IDbTransaction dbTransaction) : base(dbCon, sqlProvider, dbTransaction)
        {
        }

        protected Command(SqlProvider sqlProvider, IDbConnection dbCon) : base(dbCon, sqlProvider)
        {
        }

        public int Update(T entity)
        {
            SqlProvider.FormatUpdate(entity);

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public async Task<int> UpdateAsync(T entity)
        {
            SqlProvider.FormatUpdate(entity);

            return await DbCon.ExecuteAsync(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public int Update(Expression<Func<T, T>> updateExpression)
        {
            SqlProvider.FormatUpdate(updateExpression);

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public async Task<int> UpdateAsync(Expression<Func<T, T>> updateExpression)
        {
            SqlProvider.FormatUpdate(updateExpression);

            return await DbCon.ExecuteAsync(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public int Delete()
        {
            SqlProvider.FormatDelete();

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public async Task<int> DeleteAsync()
        {
            SqlProvider.FormatDelete();

            return await DbCon.ExecuteAsync(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public int Insert(T entity)
        {
            SqlProvider.FormatInsert(entity);

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public async Task<int> InsertAsync(T entity)
        {
            SqlProvider.FormatInsert(entity);

            return await DbCon.ExecuteAsync(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }
    }
}
