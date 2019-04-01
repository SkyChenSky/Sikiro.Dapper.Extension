using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using Sikiro.Dapper.Extension.Extension;
using Sikiro.Dapper.Extension.Helper;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension
{
    public abstract class SqlProvider
    {
        public Type TableType { get; internal set; }

        public LambdaExpression WhereExpression { get; internal set; }

        public LambdaExpression IfNotExistsExpression { get; internal set; }

        public Dictionary<EOrderBy, LambdaExpression> OrderbyExpressionList { get; internal set; }

        public LambdaExpression SelectExpression { get; internal set; }

        public int? TopNum { get; internal set; }

        public bool NoLock { get; set; }

        protected SqlProvider()
        {
            Params = new DynamicParameters();
            OrderbyExpressionList = new Dictionary<EOrderBy, LambdaExpression>();
        }

        protected abstract ProviderOption ProviderOption { get; set; }

        public string SqlString { get; set; }

        public DynamicParameters Params { get; set; }

        public abstract SqlProvider FormatGet<T>();

        public abstract SqlProvider FormatToList<T>();

        public abstract SqlProvider FormatToPageList<T>(int pageIndex, int pageSize);

        public abstract SqlProvider FormatCount();

        public abstract SqlProvider FormatExists();

        public abstract SqlProvider FormatDelete();

        public abstract SqlProvider FormatInsert<T>(T entity);

        public abstract SqlProvider FormatUpdate<T>(Expression<Func<T, T>> updateExpression);

        public abstract SqlProvider FormatUpdate<T>(T entity);

        public abstract SqlProvider FormatSum<T>(LambdaExpression lambdaExpression);

        public abstract SqlProvider FormatUpdateSelect<T>(Expression<Func<T, T>> updator);

        public abstract SqlProvider ExcuteBulkCopy<T>(IDbConnection conn, IEnumerable<T> list);

        protected string FormatTableName(bool isNeedFrom = true)
        {
            var typeOfTableClass = TableType;

            var tableName = typeOfTableClass.GetTableAttributeName();

            SqlString = $" {ProviderOption.CombineFieldName(tableName)} ";
            if (isNeedFrom)
                SqlString = " FROM " + SqlString;

            return SqlString;
        }

        protected string[] FormatInsertParamsAndValues<T>(T entity)
        {
            var paramSqlBuilder = new StringBuilder(64);
            var valueSqlBuilder = new StringBuilder(64);

            var properties = entity.GetProperties();

            var isAppend = false;
            foreach (var propertiy in properties)
            {
                if (isAppend)
                {
                    paramSqlBuilder.Append(",");
                    valueSqlBuilder.Append(",");
                }

                var columnName = propertiy.GetColumnAttributeName();
                var paramterName = ProviderOption.ParameterPrefix + columnName;
                paramSqlBuilder.Append(ProviderOption.CombineFieldName(columnName));
                valueSqlBuilder.Append(paramterName);

                Params.Add(paramterName, propertiy.GetValue(entity));

                isAppend = true;
            }

            return new[] { paramSqlBuilder.ToString(), valueSqlBuilder.ToString() };
        }
    }
}
