using System.Data;

namespace Sikiro.DbConnection.HighAvailability.Helper
{
    internal static class DbConnectionExtension
    {
        public static bool TryConnection(this IDbConnection dbCon)
        {
            try
            {
                dbCon.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                dbCon?.Close();
            }
        }
    }
}
