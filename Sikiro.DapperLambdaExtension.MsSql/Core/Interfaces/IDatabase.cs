
using System.Data;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Interfaces
{
    public interface IDatabase
    {
        Set<T> Set<T>();

        IDbConnection GetConnection();
    }
}
