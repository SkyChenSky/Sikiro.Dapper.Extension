using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sikiro.DbConnection.HighAvailability.Rule
{
    /// <summary>
    /// 加权算法
    /// </summary>
    internal abstract class WeightedRule : LoadBalanceRule
    {
        protected readonly List<WeightedRuleOption> WeightedRuleOptionCollection;

        protected int CurrentIndex;

        protected WeightedRule(List<WeightedRuleOption> weightedRuleOptionCollection)
        {
            WeightedRuleOptionCollection = ExceptWeighDivisor(weightedRuleOptionCollection);
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

                if (numList[i].Weight == 0)
                    continue;

                result = GreatestCommonDivisor(result, numList[i].Weight);
            }

            if (result != 1 && result != 0)
            {
                numList.ForEach(item => { item.Weight = item.Weight / result; });
            }

            return numList;
        }

        public override IDbConnection ReSelect()
        {
            WeightedRuleOptionCollection.RemoveAt(CurrentIndex);

            return Select();
        }

        /// <summary>
        /// 计算最大公约数（辗转相除-Euclidean algorithm）
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