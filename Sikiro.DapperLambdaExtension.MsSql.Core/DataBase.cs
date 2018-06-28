using System;
using System.Data;
using System.Data.SqlClient;
using Sikiro.DapperLambdaExtension.MsSql.Core.Core.SetC;
using Sikiro.DapperLambdaExtension.MsSql.Core.Core.SetQ;

namespace Sikiro.DapperLambdaExtension.MsSql.Core
{
    public static class DataBase
    {
        public static QuerySet<T> QuerySet<T>(this SqlConnection sqlConnection)
        {
            return new QuerySet<T>(sqlConnection, new SqlProvider<T>());
        }

        public static CommandSet<T> CommandSet<T>(this SqlConnection sqlConnection)
        {
            return new CommandSet<T>(sqlConnection, new SqlProvider<T>());
        }

        public static void Transaction(this SqlConnection sqlConnection, Action<TransContext> action)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();

            IDbTransaction transaction = sqlConnection.BeginTransaction();
            try
            {
                action(new TransContext { IDbTransaction = transaction, SqlConnection = sqlConnection });
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
        public SqlConnection SqlConnection { internal get; set; }

        public IDbTransaction IDbTransaction { internal get; set; }

        public QuerySet<T> QuerySet<T>()
        {
            return new QuerySet<T>(SqlConnection, new SqlProvider<T>(), IDbTransaction);
        }

        public CommandSet<T> CommandSet<T>()
        {
            return new CommandSet<T>(SqlConnection, new SqlProvider<T>(), IDbTransaction);
        }
    }
}
