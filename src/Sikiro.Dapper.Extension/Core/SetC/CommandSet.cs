using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Sikiro.Dapper.Extension.Core.Interfaces;
using Sikiro.Dapper.Extension.Helper;

namespace Sikiro.Dapper.Extension.Core.SetC
{
    /// <summary>
    /// 指令集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandSet<T> : Command<T>, ICommandSet<T>
    {
        public CommandSet(IDbConnection conn, SqlProvider sqlProvider) : base(sqlProvider, conn)
        {
            SqlProvider.TableType = typeof(T);
        }

        public CommandSet(IDbConnection conn, SqlProvider sqlProvider, IDbTransaction dbTransaction) : base(sqlProvider, conn, dbTransaction)
        {
            SqlProvider.TableType = typeof(T);
        }

        internal CommandSet(IDbConnection conn, SqlProvider sqlProvider, Type tableType, LambdaExpression whereExpression) : base(sqlProvider, conn)
        {
            SqlProvider.TableType = tableType;
            SqlProvider.WhereExpression = whereExpression;
        }

        public ICommand<T> Where(Expression<Func<T, bool>> predicate)
        {
            SqlProvider.WhereExpression = SqlProvider.WhereExpression == null ? predicate : ((Expression<Func<T, bool>>)SqlProvider.WhereExpression).And(predicate);

            return this;
        }

        public IInsert<T> IfNotExists(Expression<Func<T, bool>> predicate)
        {
            SqlProvider.IfNotExistsExpression = SqlProvider.IfNotExistsExpression == null ? predicate : ((Expression<Func<T, bool>>)SqlProvider.IfNotExistsExpression).And(predicate);

            return this;
        }

        public void BatchInsert(IEnumerable<T> entities, int timeout = 120)
        {
            SqlProvider.ExcuteBulkCopy(DbCon, entities);
        }
    }
}
