using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.Core.Interfaces
{
    public interface IDapperSet
    {
        Type TableType { get; set; }

        LambdaExpression WhereExpression { get; set; }

        LambdaExpression IfNotExistsExpression { get; set; }

        Dictionary<EOrderBy, LambdaExpression> OrderbyExpressionList { get; set; }

        LambdaExpression SelectExpression { get; set; }
    }
}
