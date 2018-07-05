# Sikiro.DapperLambdaExtension.MsSql [English](https://github.com/SkyChenSky/Sikiro.DapperLambdaExtension.MsSql/blob/master/README.md)
这是针对dapper的一个扩展，支持lambda表达式的写法，链式风格让开发者使用起来更加优雅、直观。

## 开始

### Nuget

你可以运行以下下命令在你的项目中安装 Sikiro.DapperLambdaExtension.MsSql。

```
PM> Install-Package Sikiro.DapperLambdaExtension.MsSql
```

### SqlConnection

```c#
var con = new SqlConnection("Data Source=192.168.13.46;Initial Catalog=SkyChen;Persist Security Info=True;User ID=sa;Password=123456789");
```

### 定义User
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
当不存在某条件记录Insert
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
您可以根据某个条件把指定字段更新
```c#
con.CommandSet<SysUser>().Where(a => a.Email == "287245177@qq.com").Update(a => new SysUser { Email = "123456789@qq.com" });
```

也可以根据主键来更新整个实体字段信息
```c#
User.Email = "123456789@qq.com";
condb.CommandSet<SysUser>().Update(User);
```

### DELETE
您可以根据条件来删除数据
```c#
con.CommandSet<SysUser>().Where(a => a.Email == "287245177@qq.com").Delete()
```

### QUERY

#### GET
获取过滤条件的一条数据（第一条）
```c#
con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com").Get()
```
#### TOLIST
当然我们也可以查询出符合条件的数据集
```c#
con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com").OrderBy(b => b.Email).Top(10).Select(a => a.Email).ToList();
```
### PAGELIST
还有分页
```c#
con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com")
                 .OrderBy(a => a.CreateDatetime)
                 .Select(a => new SysUser { Email = a.Email, CreateDatetime = a.CreateDatetime, SysUserid = a.SysUserid })
                 .PageList(1, 10);
```
### UPDATESELECT
先更新再把结果查询出来
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

### 事务功能

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

### 最后来一个完整的DEMO

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

### 其他
除了简单的CURD还有Count、Sum、Exists

## 结束
第一个版本有未完善的地方，如果大家有很好的建议欢迎随时向我提

