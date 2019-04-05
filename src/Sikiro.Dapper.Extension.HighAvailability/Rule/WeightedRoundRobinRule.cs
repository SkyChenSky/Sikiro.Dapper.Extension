using System.Collections.Generic;
using System.Data;

namespace Sikiro.Dapper.Extension.HighAvailability.Rule
{
    /// <summary>
    /// 加权轮询算法
    /// </summary>
    public class WeightedRoundRobinRule : LoadBalanceRule
    {
        public WeightedRoundRobinRule(IList<WeightedRuleOption> weightedRuleOptionCollection) : base(weightedRuleOptionCollection)
        {
        }

        public override IDbConnection Select()
        {
            throw new System.NotImplementedException();
        }
    }
}
