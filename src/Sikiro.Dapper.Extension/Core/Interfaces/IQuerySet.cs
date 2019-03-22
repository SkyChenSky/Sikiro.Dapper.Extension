using System;
using System.Linq.Expressions;
using Sikiro.Dapper.Extension.Core.SetQ;

namespace Sikiro.Dapper.Extension.Core.Interfaces
{
    public interface IQuerySet<T> : IDapperSet
    {
        QuerySet<T> Where(Expression<Func<T, bool>> predicate);
    }
}
