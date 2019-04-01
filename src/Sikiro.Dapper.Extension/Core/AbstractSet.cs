using System.Data;

namespace Sikiro.Dapper.Extension.Core.Interfaces
{
    public abstract class AbstractSet
    {
        public SqlProvider SqlProvider { get; protected set; }
        public IDbConnection DbCon { get; protected set; }
        public IDbTransaction DbTransaction { get; protected set; }

        protected AbstractSet(IDbConnection dbCon, SqlProvider sqlProvider, IDbTransaction dbTransaction)
        {
            SqlProvider = sqlProvider;
            DbCon = dbCon;
            DbTransaction = dbTransaction;
        }

        protected AbstractSet(IDbConnection dbCon, SqlProvider sqlProvider)
        {
            SqlProvider = sqlProvider;
            DbCon = dbCon;
        }
    }
}
