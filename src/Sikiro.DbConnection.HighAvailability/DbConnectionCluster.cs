using System.Collections.Generic;
using System.Data;
using Sikiro.DbConnection.HighAvailability.Enums;
using Sikiro.DbConnection.HighAvailability.Rule;

namespace Sikiro.DbConnection.HighAvailability
{
    public sealed class DbConnectionCluster
    {
        public DbConnectionCluster(DbConnectionClusterOption dbConnectionClusterOption)
        {
            Master = dbConnectionClusterOption.MasterConnection;
            Balancer.Create(dbConnectionClusterOption.LoadBalanceType, dbConnectionClusterOption.SlaveConnection ?? new List<WeightedRuleOption>());
        }

        public IDbConnection Master { get; }

        public IDbConnection Slave => Balancer.Choose() ?? Master;
    }

    public class DbConnectionClusterOption
    {
        public virtual IDbConnection MasterConnection { get; set; }

        public virtual List<WeightedRuleOption> SlaveConnection { get; set; }

        public ELoadBalance LoadBalanceType { get; set; }
    }
}
