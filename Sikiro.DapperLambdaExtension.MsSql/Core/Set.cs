using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Sikiro.DapperLambdaExtension.MsSql.Helper;
using Sikiro.DapperLambdaExtension.MsSql.Model;

namespace Sikiro.DapperLambdaExtension.MsSql.Core
{
    public class Set<T> : Aggregation<T>
    {
        internal Type TableType { get; set; }

        internal LambdaExpression WhereExpression { get; set; }

        public Set(IDbConnection conn, SqlProvider<T> sqlProvider) : base(conn, sqlProvider)
        {
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                Set = this
            };

            sqlProvider.Context = SetContext;
        }

        internal Set(IDbConnection conn, SqlProvider<T> sqlProvider, Type tableType, LambdaExpression whereExpression, LambdaExpression selectExpression, int? topNum, Dictionary<EOrderBy, LambdaExpression> orderbyExpressionList) : base(conn, sqlProvider)
        {
            TableType = tableType;
            WhereExpression = whereExpression;
            SelectExpression = selectExpression;
            TopNum = topNum;
            OrderbyExpressionList = orderbyExpressionList;

            SetContext = new DataBaseContext<T>
            {
                Set = this
            };

            sqlProvider.Context = SetContext;
        }

        public virtual Set<T> Where(Expression<Func<T, bool>> predicate)
        {
            WhereExpression = WhereExpression == null ? predicate : ((Expression<Func<T, bool>>)WhereExpression).And(predicate);

            return this;
        }
    }
}
