Sikiro.Dapper.Extension - a simple lambda extension for dapper  [中文](https://github.com/SkyChenSky/Sikiro.Dapper.Extension/blob/master/README.zh-cn.md)
========================================

This is an extension based on dapper, supporting lambda expression, with chain style which allows developers to use more gracely and innovationally.


Nuget
-----------
| Package | NuGet | 
| ------- | ------| 
| Sikiro.Dapper.Extension |[![Sikiro.Dapper.Extension](https://img.shields.io/badge/nuget-v2.0.0.0-blue.svg)](https://www.nuget.org/packages/Sikiro.Dapper.Extension/)| 
| Sikiro.Dapper.Extension.MsSql | [![Sikiro.Dapper.Extension.MsSql](https://img.shields.io/badge/nuget-v2.0.0.0-blue.svg)](https://www.nuget.org/packages/Sikiro.Dapper.Extension.MsSql/)| 
| Sikiro.Dapper.Extension.MySql | [![Sikiro.Dapper.Extension.MySql](https://img.shields.io/badge/nuget-v2.0.0.0-blue.svg)](https://www.nuget.org/packages/Sikiro.Dapper.Extension.MySql/)| 
| Sikiro.Dapper.Extension.PostgreSql |[![Sikiro.Dapper.Extension.PostgreSql](https://img.shields.io/badge/nuget-v2.0.0.0-blue.svg)](https://www.nuget.org/packages/Sikiro.Dapper.Extension.PostgreSql/)| 

Install
------------
#### MsSql
```
PM> Install-Package Sikiro.Dapper.Extension.MsSql
```
#### MySql
```
PM> Install-Package Sikiro.Dapper.Extension.MySql
```
#### PostgreSql
```
PM> Install-Package Sikiro.Dapper.Extension.PostgreSql
```
Document
---------
https://github.com/SkyChenSky/Sikiro.Dapper.Extension/wiki

Features
---------
### 1.Base On Dapper

Sikiro.Dapper.Extension is a library hosted in nuget. It can be used both on dotNet framework and dotNet Core.


The lambda expression encapsulation based on dapper is still an extension of `IDbConnection`Interface, and retains and opens the original `Execute`, `Query`, etc

### 2.Simple And Intuitive Chain

#### Query
```c#
con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com")
                       .OrderBy(a => a.CreateDatetime)
                       .Select(a => new SysUser { Email = a.Email, CreateDatetime = a.CreateDatetime, SysUserid = a.SysUserid })
                       .PageList(1, 10);
```

#### Command
```c#
con.CommandSet<SysUser>().Where(a => a.Email == "287245177@qq.com").Update(a => new SysUser { Email = "123456789@qq.com" });
```
#### ExpressionBuilder
-----------------
```c#
var where = ExpressionBuilder.Init<SysUser>();

if (string.IsNullOrWhiteSpace(param.Email))
    where = where.And(a => a.Email == "287245177@qq.com");

if (string.IsNullOrWhiteSpace(param.Mobile))
    where = where.And(a => a.Mobile == "18988565556");

con.QuerySet<SysUser>().Where(where).OrderBy(b => b.Email).Top(10).Select(a => a.Email).ToList();
```
### 3.Support Async
```c#
ToListAsync
GetAsync
InsertAsync
DeleteAsync
UpdateSelectAsync
UpdateAsync
```
### 4.Faithful To Native Attribute
```c#
[Table("SYS_USER")]
[Key]
[Required]
[StringLength(32)]
[Display(Name = "主键")]
[Column("SYS_USERID")]
[DatabaseGenerated]
```

Build Entity Tool
-------
[AutoBuildEntity](https://github.com/SkyChenSky/AutoBuildEntity)

![img](https://github.com/SkyChenSky/AutoBuildEntity/blob/master/AutoBuildEntity/Resources/entity.gif "效果图")


Contribution
-------
Welcome to submit Pull Request for code changes. If you have any questions, you can open an issue for further discussion.

License
-------
[MIT](https://github.com/SkyChenSky/Sikiro.Dapper.Extension/blob/master/LICENSE)
