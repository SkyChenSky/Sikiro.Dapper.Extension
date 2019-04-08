using System.Collections.Generic;
using System.Data.SqlClient;
using Sikiro.Dapper.Extension.MsSql;
using Sikiro.DbConnection.HighAvailability;
using Sikiro.DbConnection.HighAvailability.Enums;
using Sikiro.DbConnection.HighAvailability.MsSql;
using Sikiro.DbConnection.HighAvailability.Rule;
using Xunit;

namespace Sikiro.Dapper.Extension.HighAvailability.Test
{
    public class UnitTest1
    {
        /// <summary>
        /// 读写分离
        /// </summary>
        [Fact]
        public void DbConnectionCluster_DbConnectionClusterOption()
        {
            var list = new List<WeightedRuleOption>
            {
                new WeightedRuleOption
                {
                    DbConnection = new SqlConnection("Data Source=192.168.20.118;Initial Catalog=MF_GlobalAuth;Persist Security Info=True;MultipleActiveResultSets=True;MultiSubnetFailover=True;User ID=Finance_DEV;Password=Finance_DEVDEV"),
                    Weight = 0
                },
                new WeightedRuleOption
                {
                    DbConnection = new SqlConnection("Data Source=10.1.20.30;Initial Catalog=MF_CorpCredit;Persist Security Info=True;MultipleActiveResultSets=True;MultiSubnetFailover=True;User ID=Finance_T;Password=Finance_TFinance_T"),
                    Weight = 0
                }
            };

            //加权随机
            new DbConnectionCluster(new DbConnectionClusterOption
            {
                LoadBalanceType = ELoadBalance.WeightedRoundRobin,
                MasterConnection = new SqlConnection("Data Source=192.168.13.86;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789"),
                SlaveConnection = list
            }).Slave.QuerySet<SysUser>().Where(a => a.Mobile.Equals("18988561111"));

            new DbConnectionCluster(new DbConnectionClusterOption
            {
                LoadBalanceType = ELoadBalance.WeightedRoundRobin,
                MasterConnection = new SqlConnection("Data Source=192.168.13.86;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789"),
                SlaveConnection = list
            }).Master.QuerySet<SysUser>().Where(a => a.Mobile.Equals("18988561111"));
        }

        /// <summary>
        /// 读写分离
        /// </summary>
        [Fact]
        public void DbConnectionCluster_DbConnectionClusterOption2()
        {
            //加权随机
            new DbConnectionCluster(new MsSqlDbConnectionClusterOption
            {
                LoadBalanceType = ELoadBalance.WeightedRoundRobin,
                MasterConnectionStr = "Data Source=192.168.13.86;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789",
                SlaveConnection = new List<WeightedRuleOption>
                {
                    new MsSqlWeightedRuleOption
                    {
                        DbConnectionStr = "Data Source=10.1.20.30;Initial Catalog=MF_CorpCredit;Persist Security Info=True;MultipleActiveResultSets=True;MultiSubnetFailover=True;User ID=Finance_T;Password=Finance_TFinance_T",
                        Weight = 2
                    },
                    new MsSqlWeightedRuleOption
                    {
                        DbConnectionStr = "Data Source=192.168.20.118;Initial Catalog=MF_GlobalAuth;Persist Security Info=True;MultipleActiveResultSets=True;MultiSubnetFailover=True;User ID=Finance_DEV;Password=Finance_DEVDEV",
                        Weight = 2
                    }
                }
            }).Slave.QuerySet<SysUser>().Where(a => a.Mobile.Equals("18988561111"));

            new DbConnectionCluster(new MsSqlDbConnectionClusterOption
            {
                LoadBalanceType = ELoadBalance.WeightedRoundRobin,
                MasterConnectionStr = "Data Source=192.168.13.86;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789",
                SlaveConnection = new List<WeightedRuleOption>
                {
                    new MsSqlWeightedRuleOption
                    {
                        DbConnectionStr = "Data Source=10.1.20.30;Initial Catalog=MF_CorpCredit;Persist Security Info=True;MultipleActiveResultSets=True;MultiSubnetFailover=True;User ID=Finance_T;Password=Finance_TFinance_T",
                        Weight = 2
                    },
                    new MsSqlWeightedRuleOption
                    {
                        DbConnectionStr = "Data Source=192.168.20.118;Initial Catalog=MF_GlobalAuth;Persist Security Info=True;MultipleActiveResultSets=True;MultiSubnetFailover=True;User ID=Finance_DEV;Password=Finance_DEVDEV",
                        Weight = 2,
                    }
                }
            }).Master.QuerySet<SysUser>().Where(a => a.Mobile.Equals("18988561111"));
        }
    }
}
