using System;
using System.Linq.Expressions;

namespace Sikiro.DapperLambdaExtension.MsSql.Helper
{
    public static class ExpressionBuilder
    {
        /// <summary>
        /// 默认True条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return expression => true; }

        /// <summary>
        /// 默认False条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return expression => false; }

        /// <summary>
        /// 拼接or条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldExpression"></param>
        /// <param name="newExpression"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> oldExpression, Expression<Func<T, bool>> newExpression)
        {
            var inv = Expression.Invoke(newExpression, oldExpression.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.Or(oldExpression.Body, inv), oldExpression.Parameters);
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
            var inv = Expression.Invoke(newExpression, oldExpression.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.And(oldExpression.Body, inv), oldExpression.Parameters);
        }
    }
}
