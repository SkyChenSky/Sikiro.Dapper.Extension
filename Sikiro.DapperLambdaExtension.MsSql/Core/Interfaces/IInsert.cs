namespace Sikiro.DapperLambdaExtension.MsSql.Core.Interfaces
{
    public interface IInsert<T>
    {
        int Insert(T entity);
    }
}
