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
        public AbstractDataBaseContext Context { get; set; }

        protected SqlProvider()
        {
            Params = new DynamicParameters();
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
            var typeOfTableClass = Context.Set.TableType;

            var tableName = typeOfTableClass.GetTableAttributeName();

            SqlString = $" {ProviderOption.OpenQuote}{tableName}{ProviderOption.CloseQuote} ";
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

                var name = propertiy.GetColumnAttributeName();

                paramSqlBuilder.AppendFormat("{0}{1}{2}", ProviderOption.OpenQuote, name, ProviderOption.CloseQuote);
                valueSqlBuilder.Append(ProviderOption.ParameterPrefix + name);

                Params.Add(ProviderOption.ParameterPrefix + name, propertiy.GetValue(entity));

                isAppend = true;
            }

            return new[] { paramSqlBuilder.ToString(), valueSqlBuilder.ToString() };
        }

        protected DataBaseContext<T> DataBaseContext<T>()
        {
            return (DataBaseContext<T>)Context;
        }
    }
}
