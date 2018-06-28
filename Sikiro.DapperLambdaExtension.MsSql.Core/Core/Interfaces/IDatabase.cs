using System.Data;
using Sikiro.DapperLambdaExtension.MsSql.Core.Core.SetC;
using Sikiro.DapperLambdaExtension.MsSql.Core.Core.SetQ;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Core.Interfaces
{
    public interface IDatabase
    {
        QuerySet<T> QuerySet<T>();

        CommandSet<T> CommandSet<T>();

        IDbConnection GetConnection();
    }
}
