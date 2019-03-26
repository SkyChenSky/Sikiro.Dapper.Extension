using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Sikiro.Dapper.Extension.Exception;
using Sikiro.Dapper.Extension.Model;
using Sikiro.Dapper.Extension.MsSql.Helper;

namespace Sikiro.Dapper.Extension.MsSql
{
    internal class MsSqlProvider : SqlProvider
    {
        private const char OpenQuote = '[';
        private const char CloseQuote = ']';
        private const char ParameterPrefix = '@';

        public MsSqlProvider()
        {
            ProviderOption = new ProviderOption(OpenQuote, CloseQuote, ParameterPrefix);
            ResolveExpression.InitOption(ProviderOption);
        }

        protected sealed override ProviderOption ProviderOption { get; set; }

        public override SqlProvider FormatGet<T>()
        {
            var selectSql = ResolveExpression.ResolveSelect(typeof(T).GetProperties(), Context.Set.SelectExpression, 1);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.Set.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var orderbySql = ResolveExpression.ResolveOrderBy(Context.Set.OrderbyExpressionList);

            SqlString = $"{selectSql} {fromTableSql} {whereSql} {orderbySql}";

            return this;
        }

        public override SqlProvider FormatToList<T>()
        {
            var TopNum = DataBaseContext<T>().QuerySet.TopNum;

            var selectSql = ResolveExpression.ResolveSelect(typeof(T).GetProperties(), Context.Set.SelectExpression, TopNum);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.Set.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var orderbySql = ResolveExpression.ResolveOrderBy(Context.Set.OrderbyExpressionList);

            SqlString = $"{selectSql} {fromTableSql} {whereSql} {orderbySql}";

            return this;
        }

        public override SqlProvider FormatToPageList<T>(int pageIndex, int pageSize)
        {
            var orderbySql = ResolveExpression.ResolveOrderBy(Context.Set.OrderbyExpressionList);
            if (string.IsNullOrEmpty(orderbySql))
                throw new DapperExtensionException("order by takes precedence over pagelist");

            var selectSql = ResolveExpression.ResolveSelect(typeof(T).GetProperties(), Context.Set.SelectExpression, pageSize);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.Set.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"SELECT COUNT(1) {fromTableSql} {whereSql};";
            SqlString += $@"{selectSql}
            FROM    ( SELECT *
                      ,ROW_NUMBER() OVER ( {orderbySql} ) AS ROWNUMBER
                      {fromTableSql}
                      {whereSql}
                    ) T
            WHERE   ROWNUMBER > {(pageIndex - 1) * pageSize}
                    AND ROWNUMBER <= {pageIndex * pageSize} {orderbySql};";

            return this;
        }

        public override SqlProvider FormatCount()
        {
            var selectSql = "SELECT COUNT(1)";

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.Set.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"{selectSql} {fromTableSql} {whereSql} ";

            return this;
        }

        public override SqlProvider FormatExists()
        {
            var selectSql = "SELECT TOP 1 1";

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.Set.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"{selectSql} {fromTableSql} {whereSql}";

            return this;
        }

        public override SqlProvider FormatDelete()
        {
            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.Set.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"DELETE {fromTableSql} {whereSql }";

            return this;
        }

        public override SqlProvider FormatInsert<T>(T entity)
        {
            var paramsAndValuesSql = FormatInsertParamsAndValues(entity);

            if (Context.Set.IfNotExistsExpression == null)
                SqlString = $"INSERT INTO {FormatTableName(false)} ({paramsAndValuesSql[0]}) VALUES({paramsAndValuesSql[1]})";
            else
            {
                var ifnotexistsWhere = ResolveExpression.ResolveWhere(Context.Set.IfNotExistsExpression, "INT_");

                SqlString = string.Format(@"INSERT INTO {0}({1})  
                SELECT {2}
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

            var where = ResolveExpression.ResolveWhere(Context.Set.WhereExpression);

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

            var whereParams = ResolveExpression.ResolveWhere(Context.Set.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"{selectSql} {fromTableSql} {whereSql} ";

            return this;
        }

        public override SqlProvider FormatUpdateSelect<T>(Expression<Func<T, T>> updator)
        {
            var update = ResolveExpression.ResolveUpdate(updator);

            var selectSql = ResolveExpression.ResolveSelectOfUpdate(typeof(T).GetProperties(), Context.Set.SelectExpression);

            var where = ResolveExpression.ResolveWhere(Context.Set.WhereExpression);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            var topNum = DataBaseContext<T>().QuerySet.TopNum;

            var topSql = topNum.HasValue ? " TOP " + topNum.Value : "";
            SqlString = $"UPDATE {topSql} {FormatTableName(false)} WITH ( UPDLOCK, READPAST ) {update.SqlCmd} {selectSql} {whereSql}";

            return this;
        }

        public override SqlProvider ExcuteBulkCopy<T>(IDbConnection conn, IEnumerable<T> list)
        {
            SqlHelper.BulkCopy(conn, list);
            return this;
        }
    }
}
