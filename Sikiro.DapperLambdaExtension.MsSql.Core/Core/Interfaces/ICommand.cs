using System;
using System.Linq.Expressions;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Core.Interfaces
{
    public interface ICommand<T>
    {
        int Update(T entity);
        int Update(Expression<Func<T, T>> updateExpression);
        int Delete();
    }
}
