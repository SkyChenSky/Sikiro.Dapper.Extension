using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Sikiro.DapperLambdaExtension.MsSql.Core.Core.Interfaces;
using Sikiro.DapperLambdaExtension.MsSql.Core.Helper;
using Sikiro.DapperLambdaExtension.MsSql.Core.Model;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Core.SetC
{
    public class CommandSet<T> : Command<T>, Interfaces.ISet<T>
    {
        internal Type TableType { get; set; }

        internal LambdaExpression WhereExpression { get; set; }

        internal LambdaExpression IfNotExistsExpression { get; set; }

        public CommandSet(IDbConnection conn, SqlProvider<T> sqlProvider) : base(conn, sqlProvider)
        {
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                Set = this,
                OperateType = EOperateType.Command
            };

            sqlProvider.Context = SetContext;
        }

        public CommandSet(IDbConnection conn, SqlProvider<T> sqlProvider, IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                Set = this,
                OperateType = EOperateType.Command
            };

            sqlProvider.Context = SetContext;
        }

        internal CommandSet(IDbConnection conn, SqlProvider<T> sqlProvider, Type tableType, LambdaExpression whereExpression) : base(conn, sqlProvider)
        {
            TableType = tableType;
            WhereExpression = whereExpression;

            SetContext = new DataBaseContext<T>
            {
                Set = this,
                OperateType = EOperateType.Command
            };

            sqlProvider.Context = SetContext;
        }

        public ICommand<T> Where(Expression<Func<T, bool>> predicate)
        {
            WhereExpression = WhereExpression == null ? predicate : ((Expression<Func<T, bool>>)WhereExpression).And(predicate);

            return this;
        }

        public IInsert<T> IfNotExists(Expression<Func<T, bool>> predicate)
        {
            IfNotExistsExpression = IfNotExistsExpression == null ? predicate : ((Expression<Func<T, bool>>)IfNotExistsExpression).And(predicate);

            return this;
        }

        public void BatchInsert(IEnumerable<T> entities, int timeout = 120)
        {
            SqlHelper.BulkCopy(DbCon, entities);
        }
    }
}
