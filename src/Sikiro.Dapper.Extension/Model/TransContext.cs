using System.Data;
using Sikiro.Dapper.Extension.Core.SetC;
using Sikiro.Dapper.Extension.Core.SetQ;

namespace Sikiro.Dapper.Extension.Model
{
    public class TransContext
    {
        public IDbConnection SqlConnection { internal get; set; }

        public IDbTransaction DbTransaction { internal get; set; }

        private readonly SqlProvider _sqlProvider;

        public TransContext(IDbConnection sqlConnection, IDbTransaction dbTransaction,SqlProvider sqlProvider)
        {
            SqlConnection = sqlConnection;
            DbTransaction = dbTransaction;
            _sqlProvider = sqlProvider;
        }

        public QuerySet<T> QuerySet<T>()
        {
            return new QuerySet<T>(SqlConnection, _sqlProvider, DbTransaction);
        }

        public CommandSet<T> CommandSet<T>()
        {
            return new CommandSet<T>(SqlConnection,_sqlProvider, DbTransaction);
        }
    }
}