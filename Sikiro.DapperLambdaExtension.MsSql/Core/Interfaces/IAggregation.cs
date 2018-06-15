namespace Sikiro.DapperLambdaExtension.MsSql.Core.Interfaces
{
    public interface IAggregation
    {
        int Count();
        bool Exists();
    }
}
