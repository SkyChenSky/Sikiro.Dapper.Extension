namespace Sikiro.Dapper.Extension.Core.Interfaces
{
    public interface IInsert<T>
    {
        int Insert(T entity);
    }
}
