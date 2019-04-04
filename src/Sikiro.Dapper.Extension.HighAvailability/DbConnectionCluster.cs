using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sikiro.Dapper.Extension.HighAvailability.Enums;
using Sikiro.Dapper.Extension.HighAvailability.Rule;

namespace Sikiro.Dapper.Extension.HighAvailability
{
    public class DbConnectionCluster
    {
        private readonly IEnumerable<IDbConnection> _slaveConnectionList;
        private readonly ELoadBalance _loadBalanceType;
        public DbConnectionCluster(IReadOnlyList<IDbConnection> dbConnectionList) : this(dbConnectionList, ELoadBalance.WeightedRoundRobin)
        {
        }

        public DbConnectionCluster(IReadOnlyList<IDbConnection> dbConnectionList, ELoadBalance loadBalanceType)
        {
            Master = dbConnectionList[0];
            _slaveConnectionList = dbConnectionList.Count > 1 ? dbConnectionList.Skip(1) : new List<IDbConnection>();
            _loadBalanceType = loadBalanceType;
        }

        public IDbConnection Master { get; }

        public IDbConnection Slave => Balancer.Create(_loadBalanceType).Choose(_slaveConnectionList);
    }
}
