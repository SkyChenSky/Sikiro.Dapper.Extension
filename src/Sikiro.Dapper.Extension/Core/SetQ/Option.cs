using System;
using System.Data;
using System.Linq.Expressions;
using Sikiro.Dapper.Extension.Core.Interfaces;

namespace Sikiro.Dapper.Extension.Core.SetQ
{
    /// <summary>
    /// 配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Option<T> : Query<T>, IOption<T>
    {
        protected Option(IDbConnection conn, SqlProvider sqlProvider) : base(conn, sqlProvider)
        {

        }

        protected Option(IDbConnection conn, SqlProvider sqlProvider, IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {

        }

        /// <inheritdoc />
        public virtual Query<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            SqlProvider.SelectExpression = selector;

            return new QuerySet<TResult>(DbCon, SqlProvider, typeof(T), DbTransaction);
        }

        /// <inheritdoc />
        public virtual Option<T> Top(int num)
        {
            SqlProvider.TopNum = num;
            return this;
        }
    }
}
