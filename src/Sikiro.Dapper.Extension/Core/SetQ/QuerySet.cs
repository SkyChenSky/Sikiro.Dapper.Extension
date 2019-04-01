using System;
using System.Data;
using System.Linq.Expressions;
using Sikiro.Dapper.Extension.Core.Interfaces;
using Sikiro.Dapper.Extension.Helper;

namespace Sikiro.Dapper.Extension.Core.SetQ
{
    /// <summary>
    /// 查询集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuerySet<T> : Aggregation<T>, IQuerySet<T>
    {
        public QuerySet(IDbConnection conn, SqlProvider sqlProvider) : base(conn, sqlProvider)
        {
            SqlProvider.TableType = typeof(T);
        }

        public QuerySet(IDbConnection conn, SqlProvider sqlProvider, IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {
            SqlProvider.TableType = typeof(T);
        }

        internal QuerySet(IDbConnection conn, SqlProvider sqlProvider, Type tableType,IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {
            SqlProvider.TableType = tableType;
        }

        public QuerySet<T> Where(Expression<Func<T, bool>> predicate)
        {
            SqlProvider.WhereExpression = SqlProvider.WhereExpression == null ? predicate : ((Expression<Func<T, bool>>)SqlProvider.WhereExpression).And(predicate);

            return this;
        }

        public QuerySet<T> WithNoLock()
        {
            SqlProvider.NoLock = true;
            return this;
        }
    }
}
