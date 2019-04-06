using System.Collections.Generic;
using System.Data;

namespace Sikiro.Dapper.Extension.HighAvailability.Rule
{
    /// <summary>
    /// 加权算法
    /// </summary>
    public abstract class WeightedRule : LoadBalanceRule
    {
        protected readonly List<WeightedRuleOption> WeightedRuleOptionCollection;

        public WeightedRule(List<WeightedRuleOption> weightedRuleOptionCollection)
        {
            WeightedRuleOptionCollection = ExceptWeighDivisor(weightedRuleOptionCollection) ;
        }

        /// <summary>
        /// 除最大公约数
        /// </summary>
        /// <param name="numList"></param>
        /// <returns></returns>
        private static List<WeightedRuleOption> ExceptWeighDivisor(List<WeightedRuleOption> numList)
        {
            var result = numList[0].Weight;
            for (var i = 1; i < numList.Count; i++)
            {
                if (result == 1)
                    break;

                result = GreatestCommonDivisor(result, numList[i].Weight);
            }

            if (result != 1)
            {
                numList.ForEach(item => { item.Weight = item.Weight / result; });
            }

            return numList;
        }

        /// <summary>
        /// 计算最大公约数（辗转相除）
        /// </summary>
        /// <param name="aNum"></param>
        /// <param name="bNum"></param>
        /// <returns></returns>
        private static int GreatestCommonDivisor(int aNum, int bNum)
        {
            var remainder = aNum % bNum;
            return remainder == 0 ? bNum : GreatestCommonDivisor(bNum, remainder);
        }
    }
}