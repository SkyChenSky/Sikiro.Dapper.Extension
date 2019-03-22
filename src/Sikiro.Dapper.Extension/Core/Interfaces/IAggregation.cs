using System;
using System.Linq.Expressions;

namespace Sikiro.Dapper.Extension.Core.Interfaces
{
    public interface IAggregation<T>
    {
        /// <summary>
        /// 条数
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        /// 总和
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sumExpression"></param>
        /// <returns></returns>
        TResult Sum<TResult>(Expression<Func<T, TResult>> sumExpression);
    }
}
