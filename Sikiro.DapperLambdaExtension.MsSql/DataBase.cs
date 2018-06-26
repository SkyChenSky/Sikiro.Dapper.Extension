using System.Data.SqlClient;
using Sikiro.DapperLambdaExtension.MsSql.Core.SetC;
using Sikiro.DapperLambdaExtension.MsSql.Core.SetQ;

namespace Sikiro.DapperLambdaExtension.MsSql
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
    }
}
