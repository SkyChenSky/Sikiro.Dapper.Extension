using System.Collections.Generic;
using System.Data;

namespace Sikiro.Dapper.Extension.HighAvailability.Rule
{
    public class WeightedRoundRobinRule : LoadBalanceRule
    {
        public override IDbConnection Choose(IEnumerable<IDbConnection> dbConnectionList)
        {
            throw new System.NotImplementedException();
        }
    }
}
