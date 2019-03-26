using System;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Sikiro.Dapper.Extension.Core.Interfaces;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.Core.SetC
{
    /// <summary>
    /// 指令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Command<T> : AbstractSet, ICommand<T>, IInsert<T>
    {
        protected readonly SqlProvider SqlProvider;
        protected readonly IDbConnection DbCon;
        private readonly IDbTransaction _dbTransaction;

        protected DataBaseContext<T> SetContext { get; set; }

        protected Command(IDbConnection conn, SqlProvider sqlProvider)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
        }

        protected Command(IDbConnection conn, SqlProvider sqlProvider, IDbTransaction dbTransaction)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
            _dbTransaction = dbTransaction;
        }

        public int Update(T entity)
        {
            SqlProvider.FormatUpdate(entity);

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public async Task<int> UpdateAsync(T entity)
        {
            SqlProvider.FormatUpdate(entity);

            return await DbCon.ExecuteAsync(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public int Update(Expression<Func<T, T>> updateExpression)
        {
            SqlProvider.FormatUpdate(updateExpression);

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public async Task<int> UpdateAsync(Expression<Func<T, T>> updateExpression)
        {
            SqlProvider.FormatUpdate(updateExpression);

            return await DbCon.ExecuteAsync(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public int Delete()
        {
            SqlProvider.FormatDelete();

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public async Task<int> DeleteAsync()
        {
            SqlProvider.FormatDelete();

            return await DbCon.ExecuteAsync(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public int Insert(T entity)
        {
            SqlProvider.FormatInsert(entity);

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public async Task<int> InsertAsync(T entity)
        {
            SqlProvider.FormatInsert(entity);

            return await DbCon.ExecuteAsync(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }
    }
}
