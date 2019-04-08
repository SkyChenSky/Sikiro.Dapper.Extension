using System;

namespace Sikiro.DbConnection.HighAvailability.Exception
{
    public class DbConnectionHighAvailabilityException : ApplicationException
    {
        public DbConnectionHighAvailabilityException(string msg) : base(msg)
        {

        }
    }
}
