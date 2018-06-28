using System.Collections.Generic;
using Sikiro.DapperLambdaExtension.MsSql.Core.Model;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Core.Interfaces
{
    public interface IQuery<T>
    {
        T Get();

        List<T> ToList();

        PageList<T> PageList(int pageIndex, int pageSize);
    }
}
