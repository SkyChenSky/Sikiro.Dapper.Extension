using System.Collections.Generic;
using System.Threading.Tasks;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.Core.Interfaces
{
    public interface IQuery<T>
    {
        T Get();

        Task<T> GetAsync();

        IEnumerable<T> ToList();

        Task<IEnumerable<T>> ToListAsync();

        PageList<T> PageList(int pageIndex, int pageSize);
    }
}
