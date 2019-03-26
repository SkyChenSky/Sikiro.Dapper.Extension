using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.Core.Interfaces
{
    public abstract class AbstractSet
    {
        public Type TableType { get; protected set; }

        public LambdaExpression WhereExpression { get; protected set; }

        public LambdaExpression IfNotExistsExpression { get; protected set; }

        public Dictionary<EOrderBy, LambdaExpression> OrderbyExpressionList { get; protected set; }

        public LambdaExpression SelectExpression { get; protected set; }
    }
}
