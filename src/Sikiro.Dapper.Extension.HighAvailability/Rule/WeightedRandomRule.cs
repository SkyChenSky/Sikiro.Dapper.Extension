using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace Sikiro.Dapper.Extension.HighAvailability.Rule
{
    /// <summary>
    /// /加权随机算法
    /// </summary>
    public class WeightedRandomRule : WeightedRule
    {
        private static readonly ConcurrentDictionary<List<WeightedRuleOption>, List<int>> Cache =
            new ConcurrentDictionary<List<WeightedRuleOption>, List<int>>();

        public WeightedRandomRule(List<WeightedRuleOption> weightedRuleOptionCollection) : base(
            weightedRuleOptionCollection)
        {
        }

        public override IDbConnection Select()
        {
            var indexList = GetIndexList(WeightedRuleOptionCollection);

            var ranValue = new Random(Guid.NewGuid().GetHashCode()).Next(0, indexList.Count - 1);
            var randomIndex = indexList[ranValue];

            return WeightedRuleOptionCollection[randomIndex].DbConnection;
        }

        /// <summary>
        /// 获取数据库连接索引列
        /// </summary>
        /// <param name="weightedRuleOptionCollection"></param>
        /// <returns></returns>
        private static List<int> GetIndexList(List<WeightedRuleOption> weightedRuleOptionCollection)
        {
            return Cache.GetOrAdd(weightedRuleOptionCollection, key =>
            {
                var indexList = new List<int>();
                var collectionCount = weightedRuleOptionCollection.Count;
                for (var i = 0; i < collectionCount; i++)
                {
                    for (var j = 0; j < weightedRuleOptionCollection[i].Weight; j++)
                    {
                        indexList.Add(i);
                    }
                }

                return indexList;
            });
        }
    }
}
