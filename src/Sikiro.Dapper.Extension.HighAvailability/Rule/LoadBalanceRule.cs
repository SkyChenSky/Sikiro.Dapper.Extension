using System.Collections.Generic;
using System.Data;

namespace Sikiro.Dapper.Extension.HighAvailability.Rule
{
    public abstract class LoadBalanceRule
    {
        public abstract IDbConnection Choose(IEnumerable<IDbConnection> dbConnectionList);
    }
}
