using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Sikiro.Dapper.Extension.HighAvailability.Enums;
using Sikiro.Dapper.Extension.MsSql;
using Xunit;

namespace Sikiro.Dapper.Extension.HighAvailability.Test
{
    public class UnitTest1
    {
        /// <summary>
        /// 读写分离
        /// </summary>
        [Fact]
        public void Test1()
        {
            //索引0为主库
            var list = new List<IDbConnection> { new SqlConnection(""), new SqlConnection(""), new SqlConnection("") };

            //加权随机
            new DbConnectionCluster(list, ELoadBalance.WeightedRandom).Slave.QuerySet<SysUser>()
                .Where(a => a.Mobile.Equals("18988561111"));



            new DbConnectionCluster(list).Master.CommandSet<SysUser>().Insert(new SysUser());
        }
    }
}
