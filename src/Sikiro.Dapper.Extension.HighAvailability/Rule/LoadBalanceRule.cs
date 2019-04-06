using System.Collections.Generic;
using System.Data;

namespace Sikiro.Dapper.Extension.HighAvailability.Rule
{
    public abstract class LoadBalanceRule
    {
        public abstract IDbConnection Select();     
    }
}


public class WeightedRuleOption
{
    public IDbConnection DbConnection { get; set; }

    public int Weight { get; set; }
}
