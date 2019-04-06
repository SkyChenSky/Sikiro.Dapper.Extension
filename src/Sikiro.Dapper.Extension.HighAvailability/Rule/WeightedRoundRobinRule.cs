using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sikiro.Dapper.Extension.HighAvailability.Rule
{
    /// <summary>
    /// 加权轮询算法
    /// </summary>
    public class WeightedRoundRobinRule : WeightedRule
    {
        private static readonly ConcurrentDictionary<List<WeightedRuleOption>, IEnumerator> Cache =
            new ConcurrentDictionary<List<WeightedRuleOption>, IEnumerator>();
        
        public WeightedRoundRobinRule(List<WeightedRuleOption> weightedRuleOptionCollection) : base(
            weightedRuleOptionCollection)
        {
        }

        public override IDbConnection Select()
        {
            var indexList = GetIndexList(WeightedRuleOptionCollection);
            
            indexList.MoveNext();
            var randomIndex = (int)indexList.Current;

            return WeightedRuleOptionCollection[randomIndex].DbConnection;
        }
        
        private static IEnumerator GetIndexList(List<WeightedRuleOption> weightedRuleOptionCollection)
        {
            return Cache.GetOrAdd(weightedRuleOptionCollection, key =>
            {
                var indexList = new int[weightedRuleOptionCollection.Select(a=>a.Weight).Sum()];
                var collectionCount = weightedRuleOptionCollection.Count;
                var tempIndex = 0;
                for (var i = 0; i < collectionCount; i++)
                {
                    for (var j = 0; j < weightedRuleOptionCollection[i].Weight; j++)
                    {
                        indexList[tempIndex] = i;
                        tempIndex++;
                    }
                }

                return indexList.AsLoopEnumerator();
            });
        }
    }
}
