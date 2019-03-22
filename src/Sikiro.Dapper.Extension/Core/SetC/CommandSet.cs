using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Sikiro.Dapper.Extension.Core.Interfaces;
using Sikiro.Dapper.Extension.Helper;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.Core.SetC
{
    /// <summary>
    /// 指令集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandSet<T> : Command<T>, ICommandSet<T>
    {
        public Type TableType { get; set; }

        public CommandSet(IDbConnection conn, SqlProvider sqlProvider) : base(conn, sqlProvider)
        {
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                Set = this,
                OperateType = EOperateType.Command
            };

            sqlProvider.Context = SetContext;
        }

        public CommandSet(IDbConnection conn, SqlProvider sqlProvider, IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                Set = this,
                OperateType = EOperateType.Command
            };

            sqlProvider.Context = SetContext;
        }

        internal CommandSet(IDbConnection conn, SqlProvider sqlProvider, Type tableType, LambdaExpression whereExpression) : base(conn, sqlProvider)
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
