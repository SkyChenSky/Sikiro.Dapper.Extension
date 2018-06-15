using System.Collections.Generic;
using Sikiro.Tookits.Base;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Interfaces
{
    public interface IQuery<T>
    {
        T Get();

        List<T> ToList();

        PageList<T> PageList(int pageIndex, int pageSize);
    }
}
