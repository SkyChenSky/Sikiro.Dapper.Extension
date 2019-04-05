using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace Sikiro.Dapper.Extension.HighAvailability.Rule
{
    /// <summary>
    /// /加权随机算法
    /// </summary>
    public class WeightedRandomRule : LoadBalanceRule
    {
        private static readonly ConcurrentDictionary<IList<WeightedRuleOption>, List<int>> Cache =
            new ConcurrentDictionary<IList<WeightedRuleOption>, List<int>>();

        public WeightedRandomRule(IList<WeightedRuleOption> weightedRuleOptionCollection) : base(
            weightedRuleOptionCollection)
        {
        }

        public override IDbConnection Select()
        {
            var indexList = GetIndexList(_weightedRuleOptionCollection);

            var ranValue = new Random(Guid.NewGuid().GetHashCode()).Next(0, indexList.Count);
            var randomIndex = indexList[ranValue];

            return _weightedRuleOptionCollection[randomIndex].DbConnection;
        }

        private static List<int> GetIndexList(IList<WeightedRuleOption> weightedRuleOptionCollection)
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
