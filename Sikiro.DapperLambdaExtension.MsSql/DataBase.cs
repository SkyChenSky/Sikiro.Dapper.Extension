using System;
using System.Data;
using Sikiro.DapperLambdaExtension.MsSql.Core.Interfaces;
using Sikiro.DapperLambdaExtension.MsSql.Core.SetC;
using Sikiro.DapperLambdaExtension.MsSql.Core.SetQ;

namespace Sikiro.DapperLambdaExtension.MsSql
{
    public class DataBase : IDatabase, IDisposable
    {
        private IDbConnection Conn { get; }

        public DataBase(IDbConnection con)
        {
            Conn = con;
        }
        public QuerySet<T> QuerySet<T>()
        {
            return new QuerySet<T>(Conn, new SqlProvider<T>());
        }

        public CommandSet<T> CommandSet<T>()
        {
            return new CommandSet<T>(Conn, new SqlProvider<T>());
        }

        public IDbConnection GetConnection()
        {
            return Conn;
        }

        public void Dispose()
        {
            Conn?.Dispose();
        }
    }
}
