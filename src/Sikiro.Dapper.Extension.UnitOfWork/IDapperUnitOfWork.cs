using System;

namespace Sikiro.Dapper.Extension.UnitOfWork
{
    public interface IDapperUnitOfWork : IDisposable
    {
        IDapperUnitOfWork Begin();

        void Commit();

        void Rollback();
    }
}