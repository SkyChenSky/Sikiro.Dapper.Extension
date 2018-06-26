using System;
using System.Data.SqlClient;

namespace Sikiro.DapperLambdaExtension.MsSql.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var con = new SqlConnection(
                " Data Source=192.168.13.50;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789");

            var deleteResult = con.CommandSet<SysUser>().Where(a => a.UserName == "chengong").Delete() > 0;

            Console.WriteLine("删除数{0}", deleteResult);

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

            var getResult = con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com").Get();

            var listResult = con.QuerySet<SysUser>().OrderBy(a => a.CreateDatetime).Select(a => a.Email).ToList();

            var listResult2 = con.QuerySet<SysUser>().OrderBy(a => a.CreateDatetime).Top(2).Select(a => a.Email).ToList();

            var updateResult = con.CommandSet<SysUser>().Where(a => a.Email == "287245177@qq.com")
                .Update(a => new SysUser { UserStatus = 1 });

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

            con.Dispose();
        }
    }
}
