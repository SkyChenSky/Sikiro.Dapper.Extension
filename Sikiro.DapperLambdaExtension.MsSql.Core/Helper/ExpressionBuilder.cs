using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Helper
{
    public static class ExpressionBuilder
    {
        /// <summary>
        /// 默认True条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Init<T>() { return expression => true; }

        /// <summary>
        /// 拼接or条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldExpression"></param>
        /// <param name="newExpression"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> oldExpression, Expression<Func<T, bool>> newExpression)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.Or(oldExpression.Body, newExpression.Body);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        /// <summary>
        /// 拼接and条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldExpression"></param>
        /// <param name="newExpression"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> oldExpression, Expression<Func<T, bool>> newExpression)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(oldExpression.Body, newExpression.Body);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
