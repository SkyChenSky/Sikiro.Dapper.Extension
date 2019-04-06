using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sikiro.Dapper.Extension.HighAvailability.Rule;
using Xunit;
using Xunit.Abstractions;

namespace Sikiro.Dapper.Extension.HighAvailability.Test
{
    public class WeightedRoundRobinRuleTest
    {
        [Fact]
        public void Select_Normal_Same()
        {
            var rule = new WeightedRoundRobinRule(new List<WeightedRuleOption>
            {
                new WeightedRuleOption
                {
                    Weight = 3,
                    DbConnection =
                        new SqlConnection(
                            "Data Source=192.168.13.33;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789")
                },
                new WeightedRuleOption
                {
                    Weight = 2,
                    DbConnection =
                        new SqlConnection(
                            "Data Source=192.168.13.11;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789")
                }
            });
            
            var strList = new List<string>();
            for (var i = 0; i < 10; i++)
            {
                var result = rule.Select();
                strList.Add(result.ConnectionString);
            }
            
        }
    }
}