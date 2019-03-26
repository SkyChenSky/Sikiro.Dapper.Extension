using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Sikiro.Dapper.Extension.Core.Interfaces;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.Core.SetQ
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Query<T> : AbstractSet, IQuery<T>
    {
        public readonly SqlProvider SqlProvider;
        public readonly IDbConnection DbCon;
        public readonly IDbTransaction DbTransaction;

        protected DataBaseContext<T> SetContext { get; set; }

        protected Query(IDbConnection conn, SqlProvider sqlProvider)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
        }

        protected Query(IDbConnection conn, SqlProvider sqlProvider, IDbTransaction dbTransaction)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
            DbTransaction = dbTransaction;
        }

        public T Get()
        {
            SqlProvider.FormatGet<T>();

            return DbCon.QueryFirstOrDefault<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public async Task<T> GetAsync()
        {
            SqlProvider.FormatGet<T>();

            return await DbCon.QueryFirstOrDefaultAsync<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public IEnumerable<T> ToList()
        {
            SqlProvider.FormatToList<T>();

            return DbCon.Query<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public async Task<IEnumerable<T>> ToListAsync()
        {
            SqlProvider.FormatToList<T>();

            return await DbCon.QueryAsync<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public PageList<T> PageList(int pageIndex, int pageSize)
        {
            SqlProvider.FormatToPageList<T>(pageIndex, pageSize);

            using (var queryResult = DbCon.QueryMultiple(SqlProvider.SqlString, SqlProvider.Params, DbTransaction))
            {
                var pageTotal = queryResult.ReadFirst<int>();

                var itemList = queryResult.Read<T>().ToList();

                return new PageList<T>(pageIndex, pageSize, pageTotal, itemList);
            }
        }
    }
}
