using System.Collections.Generic;
using Sikiro.Dapper.Extension.HighAvailability.Enums;
using Sikiro.Dapper.Extension.HighAvailability.Rule;

namespace Sikiro.Dapper.Extension.HighAvailability
{
    /// <summary>
    /// 
    /// </summary>
    public class Balancer
    {
        private static LoadBalanceRule _loadBalanceRule;

        public static LoadBalanceRule Create(ELoadBalance loadBalance,
            IList<WeightedRuleOption> weightedRuleOptionCollection)
        {
            if (_loadBalanceRule != null)
                return _loadBalanceRule;

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
    }
}
