using System;
using MySql.Data.MySqlClient;

namespace Sikiro.Dapper.Extension.MySql.Core.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var con = new MySqlConnection(
                "server = 192.168.13.86;User Id = root;password = 123456789;Database = SkyChen");

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

            var insertResult2 = con.CommandSet<SysUser>().IfNotExists(a => a.Mobile == "18988563222").Insert(new SysUser
            {
                CreateDatetime = DateTime.Now,
                Email = "287245177@qq.com",
                Mobile = "18988563222",
                RealName = "陈珙",
                SysUserid = Guid.NewGuid().ToString("N"),
                UserName = "chengong",
                UserStatus = 1,
                UserType = 1,
                Password = "asdasdad"
            });

            var countResult = con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com").Count();
            Console.WriteLine("查询个数{0}", countResult);

            var getResult = con.QuerySet<SysUser>().Get();
            getResult.Email = "1111113333@qq.com";

            var updateModelResult = con.CommandSet<SysUser>().Update(getResult);
            Console.WriteLine("updateModelResult个数{0}", updateModelResult);

            var list = con.QuerySet<SysUser>().ToList();
            foreach (var item in list)
            {
                con.CommandSet<SysUser>().Where(a => a.SysUserid == item.SysUserid)
                    .Update(a => new SysUser { Email = "287242222@qq.com" });
            }

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

            var updateResult6 = con.QuerySet<SysUser>().Where(a => a.Email == "456465asd@qq.com")
                .OrderBy(a => a.CreateDatetime)
                .Select(a => new SysUser { Email = a.Email, Mobile = a.Mobile, Password = a.Password }).PageList(1, 10);

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
