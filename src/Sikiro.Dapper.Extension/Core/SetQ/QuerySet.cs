using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Sikiro.Dapper.Extension.Core.Interfaces;
using Sikiro.Dapper.Extension.Helper;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.Core.SetQ
{
    /// <summary>
    /// 查询集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuerySet<T> : Aggregation<T>, IQuerySet<T>
    {
        public QuerySet(IDbConnection conn, SqlProvider sqlProvider) : base(conn, sqlProvider)
        {
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                Set = this,
                OperateType = EOperateType.Query
            };

            sqlProvider.Context = SetContext;
        }

        public QuerySet(IDbConnection conn, SqlProvider sqlProvider, IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                Set = this,
                OperateType = EOperateType.Query
            };

            sqlProvider.Context = SetContext;
        }

        internal QuerySet(IDbConnection conn, SqlProvider sqlProvider, Type tableType, LambdaExpression whereExpression, LambdaExpression selectExpression, int? topNum, Dictionary<EOrderBy, LambdaExpression> orderbyExpressionList, IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {
            TableType = tableType;
            WhereExpression = whereExpression;
            SelectExpression = selectExpression;
            TopNum = topNum;
            OrderbyExpressionList = orderbyExpressionList;

            SetContext = new DataBaseContext<T>
            {
                Set = this,
                OperateType = EOperateType.Query
            };

            sqlProvider.Context = SetContext;
        }

        public QuerySet<T> Where(Expression<Func<T, bool>> predicate)
        {
            WhereExpression = WhereExpression == null ? predicate : ((Expression<Func<T, bool>>)WhereExpression).And(predicate);

            return this;
        }
    }
}
