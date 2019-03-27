using System;
using System.Data;
using System.Linq;
using Npgsql;
using Sikiro.Dapper.Extension.PostgreSql.Extension;

namespace Sikiro.Dapper.Extension.PostgreSql.Core.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var con = new NpgsqlConnection(
                "Host=127.0.0.1;Port=5432;Username=postgres;Password=123456789;Database=skychen");

            var user = new SysUser
            {
                CreateDatetime = DateTime.Now,
                Email = "287245177@qq.com",
                Mobile = "18988563330",
                RealName = "陈珙",
                SysUserid = Guid.NewGuid().ToString("N"),
                UserName = "chengong",
                UserStatus = 1,
                UserType = EUserType.Super,
                Password = "487c9dac0c094a31a89fef1a98bc1111"
            };

            var insertResult = con.CommandSet<SysUser>().Insert(user);
            Console.WriteLine("Insert添加数{0}", insertResult);

            user.Email = "287245188@qq.com";
            user.SysUserid = Guid.NewGuid().ToString("N");
            var ifNotExistsResult = con.CommandSet<SysUser>().IfNotExists(a => a.Email == "287245188@qq.com").Insert(user);
            Console.WriteLine("IfNotExists添加数{0}", ifNotExistsResult);

            user.SysUserid = Guid.NewGuid().ToString("N");
            var ifNotExistsResult2 = con.CommandSet<SysUser>().IfNotExists(a => a.Email == "287245188@qq.com").Insert(user);
            Console.WriteLine("IfNotExists2添加数{0}", ifNotExistsResult2);

            var getResult = con.QuerySet<SysUser>().WithNoLock().Get();
            getResult.Email = "1111113333@qq.com";
            var updateModelResult = con.CommandSet<SysUser>().Update(getResult);
            Console.WriteLine("Update添加数{0}", updateModelResult);

            var countResult = con.QuerySet<SysUser>().WithNoLock().Where(a => a.Email == "287245177@qq.com").Count();
            Console.WriteLine("Count数{0}", countResult);

            var toListResult = con.QuerySet<SysUser>().WithNoLock().Where(a => a.Email == "287245177@qq.com")
                .OrderBy(a => a.CreateDatetime).Top(2).Select(a => a.Email).ToList();
            Console.WriteLine("ToList数{0}", toListResult.Count());

            var listResult2 = con.QuerySet<SysUser>().WithNoLock().Where(a => a.Email == "287245177@qq.com")
                .OrderBy(a => a.CreateDatetime).Select(a => a.Email).PageList(2, 2);
            Console.WriteLine("PageList:{0}", listResult2.TotalPage);

            var updateResult4 = con.QuerySet<SysUser>().WithNoLock().Sum(a => a.UserStatus);
            Console.WriteLine("Sum:{0}", updateResult4);

            var updateResult7 = con.QuerySet<SysUser>().WithNoLock().Where(a => a.Email == "287245177@qq.com")
                .Top(2)
                .Select(a => new SysUser { Email = a.Email })
                .UpdateSelect(a => new SysUser { Email = "2530665632@qq.com" });
            Console.WriteLine("UpdateSelect:{0}", updateResult7.Count);

            var deleteResult = con.CommandSet<SysUser>().Delete();
            Console.WriteLine("Delete:{0}", deleteResult);

            if (con.State == ConnectionState.Closed)
                con.Open();

            using (var transaction = con.BeginTransaction())
            {
                con.CommandSet<SysUser>(transaction).Insert(new SysUser
                {
                    CreateDatetime = DateTime.Now,
                    Email = "111111111@qq.com",
                    Mobile = "11111111111",
                    RealName = "陈珙",
                    SysUserid = Guid.NewGuid().ToString("N"),
                    UserName = "chengong111",
                    UserStatus = 1,
                    UserType = EUserType.Super,
                    Password = "487c9dac0c094a31a89fef1a98bc1111"
                });

                con.CommandSet<SysUser>(transaction).Insert(new SysUser
                {
                    CreateDatetime = DateTime.Now,
                    Email = "2222222@qq.com",
                    Mobile = "22222222222",
                    RealName = "陈珙",
                    SysUserid = Guid.NewGuid().ToString("N"),
                    UserName = "chengong222",
                    UserStatus = 1,
                    UserType = EUserType.Super,
                    Password = "487c9dac0c094a31a89fef1a98bc1111"
                });

                transaction.Commit();
            }

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
                    UserType = EUserType.Super,
                    Password = "asdasdad"
                });
            }, ex =>
            {
                //do something 
            });

            con.Dispose();

        }
    }
}
