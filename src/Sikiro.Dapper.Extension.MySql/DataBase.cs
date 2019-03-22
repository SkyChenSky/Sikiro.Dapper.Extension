using System;
using System.Data;
using Sikiro.Dapper.Extension.Core.SetC;
using Sikiro.Dapper.Extension.Core.SetQ;

namespace Sikiro.Dapper.Extension.MySql
{
    public static class DataBase
    {
        public static QuerySet<T> QuerySet<T>(this IDbConnection sqlConnection)
        {
            return new QuerySet<T>(sqlConnection, new MySqlProvider());
        }

        public static CommandSet<T> CommandSet<T>(this IDbConnection sqlConnection)
        {
            return new CommandSet<T>(sqlConnection, new MySqlProvider());
        }

        public static void Transaction(this IDbConnection sqlConnection, Action<TransContext> action)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();

            var transaction = sqlConnection.BeginTransaction();
            try
            {
                action(new TransContext { DbTransaction = transaction, SqlConnection = sqlConnection });
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }

    public class TransContext
    {
        public IDbConnection SqlConnection { internal get; set; }

        public IDbTransaction DbTransaction { internal get; set; }

        public QuerySet<T> QuerySet<T>()
        {
            return new QuerySet<T>(SqlConnection, new MySqlProvider(), DbTransaction);
        }

        public CommandSet<T> CommandSet<T>()
        {
            return new CommandSet<T>(SqlConnection, new MySqlProvider(), DbTransaction);
        }
    }
}
