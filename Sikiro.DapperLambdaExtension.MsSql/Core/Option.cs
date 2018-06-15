using System;
using System.Data;
using System.Linq.Expressions;

namespace Sikiro.DapperLambdaExtension.MsSql.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Option<T> : Query<T>
    {
        protected Option(IDbConnection conn, SqlProvider<T> sqlProvider) : base(conn, sqlProvider)
        {

        }

        internal int? TopNum { get; set; }

        internal LambdaExpression SelectExpression { get; set; }

        public virtual Query<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            SelectExpression = selector;

            var thisObject = (Set<T>)this;

            return new Set<TResult>(DbCon, new SqlProvider<TResult>(), typeof(T), thisObject.WhereExpression, thisObject.SelectExpression, thisObject.TopNum, thisObject.OrderbyExpressionList);
        }

        public virtual Option<T> Top(int num)
        {
            TopNum = num;
            return this;
        }
    }
}
