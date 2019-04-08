using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Sikiro.DbConnection.HighAvailability.Rule;

namespace Sikiro.DbConnection.HighAvailability.MsSql
{
    public sealed class MsSqlDbConnectionClusterOption : DbConnectionClusterOption
    {
        public string MasterConnectionStr { get; set; }

        public List<MsSqlWeightedRuleOption> SlaveConnectionStr { get; set; }

        public MsSqlDbConnectionClusterOption()
        {
            MasterConnection = new SqlConnection(MasterConnectionStr);
            SlaveConnection = SlaveConnectionStr.Select(a => new WeightedRuleOption
            { DbConnection = new SqlConnection(a.DbConnectionStr), Weight = a.Weight }).ToList();
        }
    }

    public sealed class MsSqlWeightedRuleOption : WeightedRuleOption
    {
        public string DbConnectionStr { get; set; }

        public MsSqlWeightedRuleOption()
        {
            DbConnection = new SqlConnection(DbConnectionStr);
        }
    }
}
