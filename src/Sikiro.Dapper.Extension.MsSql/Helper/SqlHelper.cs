using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Sikiro.Dapper.Extension.Extension;

namespace Sikiro.Dapper.Extension.MsSql.Helper
{
    internal static class SqlHelper
    {
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="list">源数据</param>
        internal static void BulkCopy<T>(IDbConnection conn, IEnumerable<T> list)
        {
            var dt = list.ToDataTable();

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            using (var sqlbulkcopy = new SqlBulkCopy((SqlConnection)conn))
            {
                sqlbulkcopy.DestinationTableName = dt.TableName;
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                }
                sqlbulkcopy.WriteToServer(dt);
            }
        }
    }
}
