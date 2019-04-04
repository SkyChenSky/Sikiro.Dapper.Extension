using Sikiro.Dapper.Extension.HighAvailability.Enums;
using Sikiro.Dapper.Extension.HighAvailability.Rule;

namespace Sikiro.Dapper.Extension.HighAvailability
{
    /// <summary>
    /// 
    /// </summary>
    public class Balancer
    {
        public static LoadBalanceRule Create(ELoadBalance loadBalance)
        {
            LoadBalanceRule loadBalanceRule;
            switch (loadBalance)
            {
                case ELoadBalance.WeightedRandom:
                    loadBalanceRule = new WeightedRandomRule();
                    break;
                default:
                    loadBalanceRule = new WeightedRoundRobinRule();
                    break;
            }

            return loadBalanceRule;
        }
    }
}
