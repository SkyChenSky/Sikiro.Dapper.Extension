using System;
using System.Linq.Expressions;
using Sikiro.Dapper.Extension.Core.SetQ;

namespace Sikiro.Dapper.Extension.Core.Interfaces
{
    public interface IOrder<T>
    {
        /// <summary>
        /// 顺序
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        Order<T> OrderBy<TProperty>(Expression<Func<T, TProperty>> field);

        /// <summary>
        /// 倒叙
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        Order<T> OrderByDescing<TProperty>(Expression<Func<T, TProperty>> field);
    }
}
