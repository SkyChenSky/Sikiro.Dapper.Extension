using System;
using System.Linq.Expressions;
using Xunit;
using static Xunit.Assert;

namespace Sikiro.Dapper.Extension.MsSql.Test
{
    public class MsSqlProviderTest
    {
        [Fact]
        public void FormatGet_NoParamter_SameSql()
        {
            var sqlProvider = new MsSqlProvider { SetContext = { TableType = typeof(SysUser) } };

            sqlProvider.FormatGet<SysUser>();

            Equal("SELECT  TOP 1  [SYS_USERID] AS [SysUserid],[CREATE_DATETIME] AS [CreateDatetime],[EMAIL] AS [Email],[MOBILE] AS [Mobile],[PASSWORD] AS [Password],[REAL_NAME] AS [RealName],[USER_NAME] AS [UserName],[USER_STATUS] AS [UserStatus],[USER_TYPE] AS [UserType]   FROM  [SYS_USER]", sqlProvider.SqlString.Trim());
        }

        [Fact]
        public void FormatGet_Selector_SameSql()
        {
            var sqlProvider = new MsSqlProvider { SetContext = { TableType = typeof(SysUser) } };
            sqlProvider.SetContext.SelectExpression = (Expression<Func<SysUser, dynamic>>)(a => a.Email);

            sqlProvider.FormatGet<SysUser>();

            Equal("SELECT  TOP 1  [EMAIL]   FROM  [SYS_USER]", sqlProvider.SqlString.Trim());
        }

        [Fact]
        public void FormatCount_WhereForEmail_SameSql()
        {
            var sqlProvider = new MsSqlProvider { SetContext = { TableType = typeof(SysUser) } };
            sqlProvider.SetContext.WhereExpression =
                (Expression<Func<SysUser, bool>>)(a => a.Email == "287245177@qq.com");
            sqlProvider.SetContext.NoLock = true;

            sqlProvider.FormatCount();

            Equal("SELECT COUNT(1)  FROM  [SYS_USER]  (NOLOCK)  WHERE ([EMAIL] = @Email)", sqlProvider.SqlString.Trim());
        }
    }
}
