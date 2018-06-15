using Sikiro.DapperLambdaExtension.MsSql.Core;

namespace Sikiro.DapperLambdaExtension.MsSql.Model
{
    public class DataBaseContext<T>
    {
        public Set<T> Set { get; internal set; }
    }
}
