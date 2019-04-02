using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sikiro.Dapper.Extension.Helper;

namespace Sikiro.Dapper.Extension.Extension
{
    internal static class MapperExtension
    {
        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            var type = typeof(T);
            var tableName = type.GetTableAttributeName();
            var properties = type.GetProperties().ToList();

            var newDt = new DataTable(tableName);

            properties.ForEach(propertie =>
            {
                Type columnType;
                if (propertie.PropertyType.IsGenericType && propertie.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = propertie.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    columnType = propertie.PropertyType;
                }

                var columnName = propertie.GetColumnAttributeName();
                newDt.Columns.Add(columnName, columnType);
            });

            foreach (var item in list)
            {
                var newRow = newDt.NewRow();

                properties.ForEach(propertie =>
                {
                    newRow[propertie.GetColumnAttributeName()] = propertie.GetValue(item, null) ?? DBNull.Value;
                });

                newDt.Rows.Add(newRow);
            }

            return newDt;
        }
    }
}
