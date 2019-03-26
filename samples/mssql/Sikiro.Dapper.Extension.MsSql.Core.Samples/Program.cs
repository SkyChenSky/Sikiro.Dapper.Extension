using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Sikiro.Dapper.Extension.MsSql;
using Sikiro.Dapper.Extension.MsSql.Extension;

namespace Sikiro.Dapper.Extension.Core.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var con = new SqlConnection(
                " Data Source=192.168.13.86;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789");


            var updateResult7 = con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com")
                                                       .Top(2)
                                                       .Select(a => new SysUser { Email = a.Email })
                                                       .UpdateSelect(a => new SysUser { Email = "2530665632@qq.com" });

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
