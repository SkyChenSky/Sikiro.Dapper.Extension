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
            SqlProvider.SetContext.TableType = typeof(T);
        }

        public QuerySet(IDbConnection conn, SqlProvider sqlProvider, IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {
            SqlProvider.SetContext.TableType = typeof(T);
        }

        internal QuerySet(IDbConnection conn, SqlProvider sqlProvider, Type tableType,IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {
            SqlProvider.SetContext.TableType = tableType;
        }

        public QuerySet<T> Where(Expression<Func<T, bool>> predicate)
        {
            SqlProvider.SetContext.WhereExpression = SqlProvider.SetContext.WhereExpression == null ? predicate : ((Expression<Func<T, bool>>)SqlProvider.SetContext.WhereExpression).And(predicate);

            return this;
        }

        public QuerySet<T> WithNoLock()
        {
            SqlProvider.SetContext.NoLock = true;
            return this;
        }
    }
}
