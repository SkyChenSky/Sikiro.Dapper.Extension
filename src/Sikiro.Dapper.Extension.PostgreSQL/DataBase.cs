using System;
using System.Data;
using Sikiro.Dapper.Extension.Core.SetC;
using Sikiro.Dapper.Extension.Core.SetQ;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.PostgreSql
{
    public static class DataBase
    {
        public static QuerySet<T> QuerySet<T>(this IDbConnection sqlConnection)
        {
            return new QuerySet<T>(sqlConnection, new PostgreSqlProvider());
        }

        public static QuerySet<T> QuerySet<T>(this IDbConnection sqlConnection, IDbTransaction dbTransaction)
        {
            return new QuerySet<T>(sqlConnection, new PostgreSqlProvider(), dbTransaction);
        }

        public static CommandSet<T> CommandSet<T>(this IDbConnection sqlConnection, IDbTransaction dbTransaction)
        {
            return new CommandSet<T>(sqlConnection, new PostgreSqlProvider(), dbTransaction);
        }

        public static CommandSet<T> CommandSet<T>(this IDbConnection sqlConnection)
        {
            return new CommandSet<T>(sqlConnection, new PostgreSqlProvider());
        }

        public static void Transaction(this IDbConnection sqlConnection, Action<TransContext> action,
            Action<System.Exception> exAction = null)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();

            var transaction = sqlConnection.BeginTransaction();
            try
            {
                action(new TransContext(sqlConnection, transaction, new PostgreSqlProvider()));
                transaction.Commit();
            }
            catch (System.Exception ex)
            {
                if (exAction != null)
                    exAction(ex);
                else
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
