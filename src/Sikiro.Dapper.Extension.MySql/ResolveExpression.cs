using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Sikiro.Dapper.Extension.Exception;
using Sikiro.Dapper.Extension.Expressions;
using Sikiro.Dapper.Extension.Extension;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.MySql
{
    internal static class ResolveExpression
    {
        public static ProviderOption ProviderOption;

        public static void InitOption(ProviderOption providerOption)
        {
            ProviderOption = providerOption;
        }

        public static string ResolveOrderBy(Dictionary<EOrderBy, LambdaExpression> orderbyExpressionDic)
        {
            var orderByList = orderbyExpressionDic.Select(a =>
            {
                var columnName = ((MemberExpression)a.Value.Body).Member.GetColumnAttributeName();
                return ProviderOption.CombineFieldName(columnName) + (a.Key == EOrderBy.Asc ? " ASC " : " DESC ");
            }).ToList();

            if (!orderByList.Any())
                return "";

            return "ORDER BY " + string.Join(",", orderByList);
        }

        public static WhereExpression ResolveWhere(LambdaExpression whereExpression, string prefix = null)
        {
            var where = new WhereExpression(whereExpression, prefix, ProviderOption);

            return where;
        }

        public static UpdateEntityWhereExpression ResolveWhere(object obj)
        {
            var where = new UpdateEntityWhereExpression(obj,ProviderOption);
            where.Resolve();
            return where;
        }

        public static string ResolveSelect(PropertyInfo[] propertyInfos, LambdaExpression selector)
        {
            var selectFormat = " SELECT {0} ";
            var selectSql = "";

            if (selector == null)
            {
                var propertyBuilder = new StringBuilder();
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyBuilder.Length > 0)
                        propertyBuilder.Append(",");
                    propertyBuilder.AppendFormat($"{ProviderOption.CombineFieldName(propertyInfo.GetColumnAttributeName()) } AS {  ProviderOption.CombineFieldName(propertyInfo.Name)}");
                }
                selectSql = string.Format(selectFormat, propertyBuilder);
            }
            else
            {
                var nodeType = selector.Body.NodeType;
                if (nodeType == ExpressionType.MemberAccess)
                {
                    var columnName = ((MemberExpression)selector.Body).Member.GetColumnAttributeName();
                    selectSql = string.Format(selectFormat, ProviderOption.CombineFieldName(columnName));
                }
                else if (nodeType == ExpressionType.MemberInit)
                {
                    var memberInitExpression = (MemberInitExpression)selector.Body;
                    selectSql = string.Format(selectFormat, string.Join(",", memberInitExpression.Bindings.Select(a => ProviderOption.CombineFieldName(a.Member.GetColumnAttributeName()))));
                }
            }

            return selectSql;
        }

        public static string ResolveSelectOfUpdate(PropertyInfo[] propertyInfos, LambdaExpression selector)
        {
            var selectSql = "";

            if (selector == null)
            {
                var propertyBuilder = new StringBuilder();
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyBuilder.Length > 0)
                        propertyBuilder.Append(",");
                    propertyBuilder.AppendFormat($"INSERTED.{ ProviderOption.CombineFieldName(propertyInfo.GetColumnAttributeName())} { ProviderOption.CombineFieldName(propertyInfo.Name)}");
                }
                selectSql = propertyBuilder.ToString();
            }
            else
            {
                var nodeType = selector.Body.NodeType;
                if (nodeType == ExpressionType.MemberAccess)
                {
                    var columnName = ((MemberExpression)selector.Body).Member.GetColumnAttributeName();
                    selectSql = "INSERTED." + ProviderOption.CombineFieldName(columnName);
                }
                else if (nodeType == ExpressionType.MemberInit)
                {
                    var memberInitExpression = (MemberInitExpression)selector.Body;
                    selectSql = string.Join(",", memberInitExpression.Bindings.Select(a => "INSERTED." + ProviderOption.CombineFieldName(a.Member.GetColumnAttributeName())));
                }
            }

            return "OUTPUT " + selectSql;
        }

        public static string ResolveSum(PropertyInfo[] propertyInfos, LambdaExpression selector)
        {
            if (selector == null)
                throw new ArgumentException("selector");
            var selectSql = "";

            var nodeType = selector.Body.NodeType;
            switch (nodeType)
            {
                case ExpressionType.MemberAccess:
                    var columnName = ((MemberExpression)selector.Body).Member.GetColumnAttributeName();
                    selectSql = $" SELECT IFNULL(SUM({ProviderOption.CombineFieldName(columnName)}),0)  ";
                    break;
                case ExpressionType.MemberInit:
                    throw new DapperExtensionException("不支持该表达式类型");
            }

            return selectSql;
        }

        public static UpdateExpression ResolveUpdate<T>(Expression<Func<T, T>> updateExpression)
        {
            return new UpdateExpression(updateExpression,ProviderOption);
        }
    }
}
