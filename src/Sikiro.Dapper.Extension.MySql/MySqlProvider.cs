﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Sikiro.Dapper.Extension.Exception;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.MySql
{
    internal class MySqlProvider : SqlProvider
    {
        private const char OpenQuote = '`';
        private const char CloseQuote = '`';
        private const char ParameterPrefix = '@';

        public MySqlProvider()
        {
            ProviderOption = new ProviderOption(OpenQuote, CloseQuote, ParameterPrefix);
            ResolveExpression.InitOption(ProviderOption);
        }

        protected sealed override ProviderOption ProviderOption { get; set; }

        public override SqlProvider FormatGet<T>()
        {
            var selectSql = ResolveExpression.ResolveSelect(typeof(T).GetProperties(), SetContext.SelectExpression);

            var fromTableSql = FormatTableName();

            var nolockSql = ResolveExpression.ResolveWithNoLock(SetContext.NoLock);

            var whereParams = ResolveExpression.ResolveWhere(SetContext.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var orderbySql = ResolveExpression.ResolveOrderBy(SetContext.OrderbyExpressionList);

            SqlString = $"{selectSql} {fromTableSql} {whereSql} {nolockSql} {orderbySql} LIMIT 1";

            return this;
        }

        public override SqlProvider FormatToList<T>()
        {
            var selectSql = ResolveExpression.ResolveSelect(typeof(T).GetProperties(), SetContext.SelectExpression);

            var fromTableSql = FormatTableName();

            var nolockSql = ResolveExpression.ResolveWithNoLock(SetContext.NoLock);

            var whereParams = ResolveExpression.ResolveWhere(SetContext.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var orderbySql = ResolveExpression.ResolveOrderBy(SetContext.OrderbyExpressionList);

            var topNum = SetContext.TopNum;

            var limitSql = topNum.HasValue ? " LIMIT " + topNum.Value : "";

            SqlString = $"{selectSql} {fromTableSql} {nolockSql} {whereSql}  {orderbySql} {limitSql}";

            return this;
        }

        public override SqlProvider FormatToPageList<T>(int pageIndex, int pageSize)
        {
            var orderbySql = ResolveExpression.ResolveOrderBy(SetContext.OrderbyExpressionList);
            if (string.IsNullOrEmpty(orderbySql))
                throw new DapperExtensionException("order by takes precedence over pagelist");

            var selectSql = ResolveExpression.ResolveSelect(typeof(T).GetProperties(), SetContext.SelectExpression);

            var fromTableSql = FormatTableName();

            var nolockSql = ResolveExpression.ResolveWithNoLock(SetContext.NoLock);

            var whereParams = ResolveExpression.ResolveWhere(SetContext.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"SELECT COUNT(1) {fromTableSql} {whereSql};";
            SqlString += $"{selectSql} {fromTableSql} {nolockSql} {whereSql} {orderbySql} LIMIT {(pageIndex - 1) * pageSize},{pageSize}";

            return this;
        }

        public override SqlProvider FormatCount()
        {
            var selectSql = "SELECT COUNT(*)";

            var fromTableSql = FormatTableName();

            var nolockSql = ResolveExpression.ResolveWithNoLock(SetContext.NoLock);

            var whereParams = ResolveExpression.ResolveWhere(SetContext.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"{selectSql} {fromTableSql} {nolockSql} {whereSql}";

            return this;
        }

        public override SqlProvider FormatExists()
        {
            var selectSql = "SELECT 1";

            var fromTableSql = FormatTableName();

            var nolockSql = ResolveExpression.ResolveWithNoLock(SetContext.NoLock);

            var whereParams = ResolveExpression.ResolveWhere(SetContext.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"{selectSql} {fromTableSql} {nolockSql} {whereSql}  LIMIT 1";

            return this;
        }

        public override SqlProvider FormatDelete()
        {
            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(SetContext.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"DELETE {fromTableSql} {whereSql}";

            return this;
        }

        public override SqlProvider FormatInsert<T>(T entity)
        {
            var paramsAndValuesSql = FormatInsertParamsAndValues(entity);

            if (SetContext.IfNotExistsExpression == null)
                SqlString = $"INSERT INTO {FormatTableName(false)} ({paramsAndValuesSql[0]}) VALUES({paramsAndValuesSql[1]})";
            else
            {
                var ifnotexistsWhere = ResolveExpression.ResolveWhere(SetContext.IfNotExistsExpression, "INT_");

                SqlString = string.Format(@"INSERT INTO {0}({1})  
                SELECT {2}
                FROM DUAL
                WHERE NOT EXISTS(
                    SELECT 1
                    FROM {0}  
                {3}
                    ); ", FormatTableName(false), paramsAndValuesSql[0], paramsAndValuesSql[1], ifnotexistsWhere.SqlCmd);

                Params.AddDynamicParams(ifnotexistsWhere.Param);
            }

            return this;
        }

        public override SqlProvider FormatUpdate<T>(Expression<Func<T, T>> updateExpression)
        {
            var update = ResolveExpression.ResolveUpdate(updateExpression);

            var where = ResolveExpression.ResolveWhere(SetContext.WhereExpression);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            SqlString = $"UPDATE {FormatTableName(false)} {update.SqlCmd} {whereSql}";

            return this;
        }

        public override SqlProvider FormatUpdate<T>(T entity)
        {
            var update = ResolveExpression.ResolveUpdate<T>(a => entity);

            var where = ResolveExpression.ResolveWhere(entity);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            SqlString = $"UPDATE {FormatTableName(false)} {update.SqlCmd} {whereSql}";

            return this;
        }

        public override SqlProvider FormatSum<T>(LambdaExpression lambdaExpression)
        {
            var selectSql = ResolveExpression.ResolveSum(typeof(T).GetProperties(), lambdaExpression);

            var fromTableSql = FormatTableName();

            var nolockSql = ResolveExpression.ResolveWithNoLock(SetContext.NoLock);

            var whereParams = ResolveExpression.ResolveWhere(SetContext.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"{selectSql} {fromTableSql} {nolockSql} {whereSql}";

            return this;
        }

        public override SqlProvider FormatUpdateSelect<T>(Expression<Func<T, T>> updator)
        {
            throw new DapperExtensionException("the function is not support on mysql");
        }

        public override SqlProvider ExcuteBulkCopy<T>(IDbConnection conn, IEnumerable<T> list)
        {
            throw new NotImplementedException();
        }
    }
}
