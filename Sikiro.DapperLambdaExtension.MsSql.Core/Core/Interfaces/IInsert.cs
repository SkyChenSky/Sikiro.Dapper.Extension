namespace Sikiro.DapperLambdaExtension.MsSql.Core.Core.Interfaces
{
    public interface IInsert<T>
    {
        int Insert(T entity);
    }
}
