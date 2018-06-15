using System;
using System.Data;
using Sikiro.DapperLambdaExtension.MsSql.Core;
using Sikiro.DapperLambdaExtension.MsSql.Core.Interfaces;

namespace Sikiro.DapperLambdaExtension.MsSql
{
    public class DataBase : IDatabase, IDisposable
    {
        private IDbConnection Conn { get; }

        public DataBase(IDbConnection con)
        {
            Conn = con;
        }
        public Set<T> Set<T>()
        {
            return new Set<T>(Conn, new SqlProvider<T>());
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
