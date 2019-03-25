Sikiro.Dapper.Extension - a simple lambda extension for dapper  [中文](https://github.com/SkyChenSky/Sikiro.DapperLambdaExtension.MsSql/blob/master/README.md)
========================================

This is an extension based on dapper, supporting lambda expression, chain style allows developers to use more elegant and intuitive.


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
### 1.base on dapper

Sikiro. Dapper. Extension is a dotNet Standard library hosted in nuget. It can be used in dotNet framework and dotNet Core platform.

The lambda expression encapsulation based on dapper is still an extension of `IDbConnection'Interface, and retains and opens the original `Execute', `Query', etc

### 2.Simple and intuitive chain
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
### 3.Support Async
```c#
ToListAsync
GetAsync
InsertAsync
DeleteAsync
UpdateAsync
```
### 4.Faithful to Native Attribute
```c#
[Table("SYS_USER")]
[Key]
[Required]
[StringLength(32)]
[Display(Name = "主键")]
[Column("SYS_USERID")]
```

Build Entity Tool
-------
[AutoBuildEntity](https://github.com/SkyChenSky/AutoBuildEntity)

![img](https://github.com/SkyChenSky/AutoBuildEntity/blob/master/AutoBuildEntity/Resources/entity.gif "效果图")


Contribution

-------
Welcome to submit Pull Request code changes. If you have any questions, you can submit them to issue for discussion.

License
-------
[MIT](https://github.com/SkyChenSky/Sikiro.Dapper.Extension/blob/master/LICENSE)
