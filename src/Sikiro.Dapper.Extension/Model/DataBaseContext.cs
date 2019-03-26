using Sikiro.Dapper.Extension.Core.Interfaces;
using Sikiro.Dapper.Extension.Core.SetC;
using Sikiro.Dapper.Extension.Core.SetQ;

namespace Sikiro.Dapper.Extension.Model
{
    public class DataBaseContext<T> : AbstractDataBaseContext
    {
        public QuerySet<T> QuerySet => (QuerySet<T>)Set;

        public CommandSet<T> CommandSet => (CommandSet<T>)Set;
    }

    public abstract class AbstractDataBaseContext
    {
        public AbstractSet Set { get; internal set; }

        internal EOperateType OperateType { get; set; }
    }
}
