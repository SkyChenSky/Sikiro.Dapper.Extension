using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Sikiro.DapperLambdaExtension.MsSql.Model;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.SetQ
{
    /// <inheritdoc />
    /// <summary>
    /// 排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Order<T> : Option<T>
    {
        internal Dictionary<EOrderBy, LambdaExpression> OrderbyExpressionList { get; set; }

        protected Order(IDbConnection conn, SqlProvider<T> sqlProvider) : base(conn, sqlProvider)
        {
            OrderbyExpressionList = new Dictionary<EOrderBy, LambdaExpression>();
        }


        /// <summary>
        /// 顺序
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public virtual Order<T> OrderBy<TProperty>(Expression<Func<T, TProperty>> field)
        {
            if (field != null)
                OrderbyExpressionList.Add(EOrderBy.Asc, field);

            return this;
        }

        /// <summary>
        /// 倒叙
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public virtual Order<T> OrderByDescing<TProperty>(Expression<Func<T, TProperty>> field)
        {
            if (field != null)
                OrderbyExpressionList.Add(EOrderBy.Desc, field);

            return this;
        }
    }
}
