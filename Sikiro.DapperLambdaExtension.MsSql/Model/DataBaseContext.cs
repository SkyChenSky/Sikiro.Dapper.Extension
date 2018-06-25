using Sikiro.DapperLambdaExtension.MsSql.Core.SetC;
using Sikiro.DapperLambdaExtension.MsSql.Core.SetQ;

namespace Sikiro.DapperLambdaExtension.MsSql.Model
{
    public class DataBaseContext<T>
    {
        public QuerySet<T> QuerySet => (QuerySet<T>)Set;

        public CommandSet<T> CommandSet => (CommandSet<T>)Set;

        public Core.Interfaces.ISet<T> Set { get; internal set; }

        internal EOperateType OperateType { get; set; }
    }
}
