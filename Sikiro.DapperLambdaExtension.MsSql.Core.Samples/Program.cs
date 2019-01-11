using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var con = new SqlConnection(
                " Data Source=192.168.13.53;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789");


            Enumerable.Range(0, 10000).ToList().AsParallel().ForAll(a =>
            {
                var con2 = new SqlConnection(
                    " Data Source=192.168.13.53;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789");
                var count = con2.Execute(@"UPDATE  dbo.SYS_USER
            SET     USER_TYPE = USER_TYPE - 10
            WHERE   USER_TYPE >= 10
            AND SYS_USERID = '2009b778a6bb426cbbb5e96b4d87ccea'; ");
                Console.WriteLine(count);
            });

            con.Query<SysUser>(@"UPDATE  dbo.SYS_USER
            SET     USER_TYPE = USER_TYPE - 10
            WHERE   USER_TYPE >= 10
            AND SYS_USERID = '2009b778a6bb426cbbb5e96b4d87ccea'; ");
            var qwe = new SysUser
            {
                CreateDatetime = DateTime.Now,
                Email = "287245177@qq.com",
                Mobile = "18988563330",
                RealName = "陈珙",
                SysUserid = "487c9dac0c094a31a89fef1a98bc4204",
                UserName = "chengong",
                UserStatus = 1,
                UserType = 1,
                Password = "asdasdad"
            };
            con.CommandSet<SysUser>().Where(a => a.SysUserid == qwe.SysUserid)
                .Update(a => new SysUser { Email = "287245177112@qq.com" });

            var list = con.QuerySet<SysUser>().ToList();
            foreach (var VARIABLE in list)
            {
                con.CommandSet<SysUser>().Where(a => a.SysUserid == VARIABLE.SysUserid)
                    .Update(a => new SysUser { Email = "287242222@qq.com" });
            }


            var insertResult2 = con.CommandSet<SysUser>().IfNotExists(a => a.Mobile == "18988563330").Insert(new SysUser
            {
                CreateDatetime = DateTime.Now,
                Email = "287245177@qq.com",
                Mobile = "18988563330",
                RealName = "陈珙",
                SysUserid = Guid.NewGuid().ToString("N"),
                UserName = "chengong",
                UserStatus = 1,
                UserType = 1,
                Password = "asdasdad"
            });

            var insertResult = con.CommandSet<SysUser>().Insert(new SysUser
            {
                CreateDatetime = DateTime.Now,
                Email = "287245177@qq.com",
                Mobile = "18988561111",
                RealName = "陈珙",
                SysUserid = Guid.NewGuid().ToString("N"),
                UserName = "chengong",
                UserStatus = 1,
                UserType = 1,
                Password = "asdasdad"
            });
            Console.WriteLine("添加数{0}", insertResult);

            var countResult = con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com").Count();
            Console.WriteLine("查询个数{0}", insertResult);

            var getResult = con.QuerySet<SysUser>().Get();
            getResult.Email = "1111113333@qq.com";
            var updateModelResult = con.CommandSet<SysUser>().Update(getResult);

            var listResult = con.QuerySet<SysUser>().OrderBy(a => a.CreateDatetime).Select(a => a.Email).ToList();

            var listResult2 = con.QuerySet<SysUser>().OrderBy(a => a.CreateDatetime).Top(2).Select(a => a.Email).ToList();

            var updateResult = con.CommandSet<SysUser>().Where(a => a.SysUserid == "487c9dac0c094a31a89fef1a98bc4204")
                .Update(a => new SysUser { Email = "287245177@qq.com" });

            getResult.Email = "287245145666@qq.com";
            var updateResult2 = con.CommandSet<SysUser>().Where(a => a.Email == "287245177@qq.com").Update(getResult);

            var updateResult3 = con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com").OrderBy(b => b.Email)
                .Top(10).Select(a => a.Email).ToList();

            var updateResult8 = con.QuerySet<SysUser>().OrderBy(b => b.Email).Top(10).ToList();

            var updateResult4 = con.QuerySet<SysUser>().Sum(a => a.UserStatus);

            var updateResult5 = con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com")
                .Select(a => new SysUser { Email = a.Email })
                .UpdateSelect(a => new SysUser { Email = "2530665632@qq.com" });

            var updateResult6 = con.QuerySet<SysUser>().Where(a => a.Email == "456465asd@qq.com")
                .OrderBy(a => a.CreateDatetime)
                .Select(a => new SysUser { Email = a.Email, Mobile = a.Mobile, Password = a.Password }).PageList(1, 10);

            var updateResult7 = con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com")
                .OrderBy(a => a.CreateDatetime)
                .Select(a => new SysUser { Email = a.Email })
                .UpdateSelect(a => new SysUser { Email = "2530665632@qq.com" });

            con.Transaction(tc =>
            {
                var SysUserid1 = tc.QuerySet<SysUser>().Where(a => a.Mobile == "18988561110").Select(a => a.SysUserid).Get();

                var SysUserid2 = tc.QuerySet<SysUser>().Where(a => a.Mobile == "18988561111").Select(a => a.SysUserid).Get();

                tc.CommandSet<SysUser>().Where(a => a.SysUserid == SysUserid1).Delete();

                tc.CommandSet<SysUser>().Where(a => a.SysUserid == SysUserid2).Delete();

                tc.CommandSet<SysUser>().Insert(new SysUser
                {
                    CreateDatetime = DateTime.Now,
                    Email = "287245177@qq.com",
                    Mobile = "13536059332",
                    RealName = "大笨贞",
                    SysUserid = Guid.NewGuid().ToString("N"),
                    UserName = "fengshuzhen",
                    UserStatus = 1,
                    UserType = 1,
                    Password = "asdasdad"
                });
            });

            con.Dispose();

        }
    }
}
