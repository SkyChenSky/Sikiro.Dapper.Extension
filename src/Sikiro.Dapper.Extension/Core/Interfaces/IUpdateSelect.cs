using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sikiro.Dapper.Extension.Core.Interfaces
{
    public interface IUpdateSelect<T>
    {
        List<T> UpdateSelect(Expression<Func<T, T>> where);

        Task<IEnumerable<T>> UpdateSelectAsync(Expression<Func<T, T>> updator);
    }
}
