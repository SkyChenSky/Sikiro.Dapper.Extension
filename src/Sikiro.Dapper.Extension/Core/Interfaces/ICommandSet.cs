using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sikiro.Dapper.Extension.Core.Interfaces
{
    public interface ICommandSet<T> : IDapperSet
    {
        ICommand<T> Where(Expression<Func<T, bool>> predicate);

        IInsert<T> IfNotExists(Expression<Func<T, bool>> predicate);

        void BatchInsert(IEnumerable<T> entities, int timeout = 120);
    }
}
