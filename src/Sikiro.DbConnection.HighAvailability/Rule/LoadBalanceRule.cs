using System.Collections.Generic;
using System.Data;

namespace Sikiro.DbConnection.HighAvailability.Rule
{
    internal abstract class LoadBalanceRule
    {
        public abstract IDbConnection Select();

        public abstract IDbConnection ReSelect();
    }


    public class WeightedRuleOption
    {
        internal virtual IDbConnection DbConnection { get; set; }

        public int Weight { get; set; }
    }
}