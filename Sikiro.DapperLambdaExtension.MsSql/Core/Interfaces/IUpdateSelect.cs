using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Interfaces
{
    public interface IUpdateSelect<T>
    {
        List<T> UpdateSelect(Expression<Func<T, T>> where);
    }
}
