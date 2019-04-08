using System.Collections.Generic;
using System.Data;
using Sikiro.DbConnection.HighAvailability.Enums;
using Sikiro.DbConnection.HighAvailability.Helper;
using Sikiro.DbConnection.HighAvailability.Rule;

namespace Sikiro.DbConnection.HighAvailability
{
    /// <summary>
    /// 
    /// </summary>
    internal class Balancer
    {
        private static LoadBalanceRule _loadBalanceRule;

        internal static LoadBalanceRule Create(ELoadBalance loadBalance,
            List<WeightedRuleOption> weightedRuleOptionCollection)
        {
            switch (loadBalance)
            {
                case ELoadBalance.WeightedRandom:
                    _loadBalanceRule = new WeightedRandomRule(weightedRuleOptionCollection);
                    break;
                default:
                    _loadBalanceRule = new WeightedRoundRobinRule(weightedRuleOptionCollection);
                    break;
            }

            return _loadBalanceRule;
        }

        public static IDbConnection Choose()
        {
            var resultDbConnection = _loadBalanceRule.Select();

            while (resultDbConnection != null && !resultDbConnection.TryConnection())
            {
                resultDbConnection = _loadBalanceRule.ReSelect();
            }

            return resultDbConnection;
        }
    }
}
