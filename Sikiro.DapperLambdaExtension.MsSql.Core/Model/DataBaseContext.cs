using Sikiro.DapperLambdaExtension.MsSql.Core.Core.Interfaces;
using Sikiro.DapperLambdaExtension.MsSql.Core.Core.SetC;
using Sikiro.DapperLambdaExtension.MsSql.Core.Core.SetQ;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Model
{
    public class DataBaseContext<T>
    {
        public QuerySet<T> QuerySet => (QuerySet<T>)Set;

        public CommandSet<T> CommandSet => (CommandSet<T>)Set;

        public ISet<T> Set { get; internal set; }

        internal EOperateType OperateType { get; set; }
    }
}
