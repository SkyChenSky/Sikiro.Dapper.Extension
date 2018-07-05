# Sikiro.DapperLambdaExtension.MsSql                                         [中文](https://github.com/SkyChenSky/Sikiro.DapperLambdaExtension.MsSql/blob/master/README.zh-cn.md)
This is an lambda extension of dapper, Chain style makes developers more elegant and intuitive.

## Getting Started

### Nuget

You can run the following command to install the Sikiro.DapperLambdaExtension.MsSql in your project。

```
PM> Install-Package Sikiro.DapperLambdaExtension.MsSql
```

### SqlConnection

```c#
var con = new SqlConnection("Data Source=192.168.13.46;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789");
```

### Defining User Entity
```c#
[Table("SYS_USER")]
public class SysUser
{
    /// <summary>
    /// 主键
    /// </summary>    
    [Key]
    [Required]
    [StringLength(32)]
    [Display(Name = "主键")]
    [Column("SYS_USERID")]
    public string SysUserid { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>    
    [Required]
    [Display(Name = "创建时间")]
    [Column("CREATE_DATETIME")]
    public DateTime CreateDatetime { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>    
    [Required]
    [StringLength(32)]
    [Display(Name = "邮箱")]
    [Column("EMAIL")]
    public string Email { get; set; }

    /// <summary>
    /// USER_STATUS
    /// </summary>    
    [Required]
    [Display(Name = "USER_STATUS")]
    [Column("USER_STATUS")]
    public int UserStatus { get; set; }
}
```

### Insert
```c#
con.CommandSet<SysUser>().Insert(new SysUser
{
    CreateDatetime = DateTime.Now,
    Email = "287245177@qq.com",
    SysUserid = Guid.NewGuid().ToString("N"),
    UserName = "chengong",
});
```
If not exists...insert...
```c#
con.CommandSet<SysUser>().IfNotExists(a => a.Email == "287245177@qq.com").Insert(new SysUser
{
    CreateDatetime = DateTime.Now,
    Email = "287245177@qq.com",
    SysUserid = Guid.NewGuid().ToString("N"),
    UserName = "chengong",
});
```

### UPDATE
Update according to the condition part field
```c#
con.CommandSet<SysUser>().Where(a => a.Email == "287245177@qq.com").Update(a => new SysUser { Email = "123456789@qq.com" });
```

You can also update the entity field information based on the primary key 
```c#
User.Email = "123456789@qq.com";
condb.CommandSet<SysUser>().Update(User);
```

### DELETE
Delete according to the condition

```c#
con.CommandSet<SysUser>().Where(a => a.Email == "287245177@qq.com").Delete()
```

### QUERY

#### GET
Get the first data by filtering condition

```c#
con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com").Get()
```
#### TOLIST
You can also query qualified data list.
```c#
con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com").OrderBy(b => b.Email).Top(10).Select(a => a.Email).ToList();
```
### PAGELIST
```c#
con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com")
                 .OrderBy(a => a.CreateDatetime)
                 .Select(a => new SysUser { Email = a.Email, CreateDatetime = a.CreateDatetime, SysUserid = a.SysUserid })
                 .PageList(1, 10);
```
### UPDATESELECT
First update then select
```c#
con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com")
                .OrderBy(a => a.CreateDatetime)
                .Select(a => new SysUser { Email = a.Email })
                .UpdateSelect(a => new SysUser { Email = "2530665632@qq.com" });
```

### ExpressionBuilder
```c#
var where = ExpressionBuilder.Init<SysUser>();

if (string.IsNullOrWhiteSpace(param.Email))
    where = where.And(a => a.Email == "287245177@qq.com");

if (string.IsNullOrWhiteSpace(param.Mobile))
    where = where.And(a => a.Mobile == "18988565556");

con.QuerySet<SysUser>().Where(where).OrderBy(b => b.Email).Top(10).Select(a => a.Email).ToList();
```

### Transaction

```c#
con.Transaction(tc =>
{
    var sysUserid = tc.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com").Select(a => a.SysUserid).Get();

    tc.CommandSet<SysUser>().Where(a => a.SysUserid == sysUserid).Delete();

    tc.CommandSet<SysUser>().Insert(new SysUser
    {
         CreateDatetime = DateTime.Now,
         Email = "2530665632@qq.com",
         SysUserid = Guid.NewGuid().ToString("N"),
         UserName = "xiaobenzhen",
    });
});
```

### Finally a complete Demo

```c#
using (var con = new SqlConnection("Data Source=192.168.13.46;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789"))
{
    con.CommandSet<SysUser>().Insert(new SysUser
    {
        CreateDatetime = DateTime.Now,
        Email = "287245177@qq.com",
        SysUserid = Guid.NewGuid().ToString("N"),
        UserName = "chengong",
    });

    var model = con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com").Get();

    con.CommandSet<SysUser>().Where(a => a.SysUserid == model.SysUserid)
        .Update(a => new SysUser { Email = "2548987@qq.com" });

    con.CommandSet<SysUser>().Where(a => a.SysUserid == model.SysUserid).Delete();
}
```

### Others
In addition to the above functions, there are aggregated queries.Such as Count、Sum、Exists

## End
If you have good suggestions, please feel free to mention to me.

