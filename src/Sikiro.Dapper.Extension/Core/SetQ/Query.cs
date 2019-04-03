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
        protected Query(IDbConnection dbCon, SqlProvider sqlProvider) : base(dbCon, sqlProvider)
        {
        }
        protected Query(IDbConnection dbCon, SqlProvider sqlProvider, IDbTransaction dbTransaction) : base(dbCon, sqlProvider, dbTransaction)
        {
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

                var itemList = queryResult.Read<T>();

                return new PageList<T>(pageIndex, pageSize, pageTotal, itemList);
            }
        }
    }
}
