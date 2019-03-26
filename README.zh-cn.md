Sikiro.Dapper.Extension - 基于dapper简单易用的lambda扩展   [英文](https://github.com/SkyChenSky/Sikiro.DapperLambdaExtension.MsSql/blob/master/README.md)
========================================


这是基于dapper的一个扩展，支持lambda表达式的写法，链式风格让开发者使用起来更加优雅、直观。


Nuget
-----------
| Package | NuGet | 
| ------- | ------| 
| Sikiro.Dapper.Extension |[![Sikiro.Dapper.Extension](https://img.shields.io/badge/nuget-v2.0.0.0-blue.svg)](https://www.nuget.org/packages/Sikiro.Dapper.Extension/)| 
| Sikiro.Dapper.Extension.MsSql | [![Sikiro.Dapper.Extension.MsSql](https://img.shields.io/badge/nuget-v2.0.0.0-blue.svg)](https://www.nuget.org/packages/Sikiro.Dapper.Extension.MsSql/)| 
| Sikiro.Dapper.Extension.MySql | [![Sikiro.Dapper.Extension.MySql](https://img.shields.io/badge/nuget-v2.0.0.0-blue.svg)](https://www.nuget.org/packages/Sikiro.Dapper.Extension.MySql/)| 
| Sikiro.Dapper.Extension.PostgreSql |[![Sikiro.Dapper.Extension.PostgreSql](https://img.shields.io/badge/nuget-v2.0.0.0-blue.svg)](https://www.nuget.org/packages/Sikiro.Dapper.Extension.PostgreSql/)| 

安装
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
文档
---------
https://github.com/SkyChenSky/Sikiro.Dapper.Extension/wiki

特性
---------
### 1.基于dapper的扩展
Sikiro.Dapper.Extension是托管于nuget的一个dotNet Standard库。可使用于dotNet framework与dotNet Core平台。

基于dapper基础上做了lambda表达式封装，仍然是`IDbConnection` Interface的扩展，并保留与开放原生的`Execute`、`Query`等方法。
### 2.简单直观的链式写法
#### 查询
```c#
con.QuerySet<SysUser>().Where(a => a.Email == "287245177@qq.com")
                       .OrderBy(a => a.CreateDatetime)
                       .Select(a => new SysUser { Email = a.Email, CreateDatetime = a.CreateDatetime, SysUserid = a.SysUserid })
                       .PageList(1, 10);
```

#### 指令
```c#
con.CommandSet<SysUser>().Where(a => a.Email == "287245177@qq.com").Update(a => new SysUser { Email = "123456789@qq.com" });
```
### 3.支持异步
```c#
ToListAsync
GetAsync
InsertAsync
DeleteAsync
UpdateSelectAsync
UpdateAsync
```
### 4.忠于原生的特性标签
```c#
[Table("SYS_USER")]
[Key]
[Required]
[StringLength(32)]
[Display(Name = "主键")]
[Column("SYS_USERID")]
[DatabaseGenerated]
```

实体生成工具
-------
[AutoBuildEntity](https://github.com/SkyChenSky/AutoBuildEntity)

![img](https://github.com/SkyChenSky/AutoBuildEntity/blob/master/AutoBuildEntity/Resources/entity.gif "效果图")

贡献
-------
欢迎各位提交Pull Request代码变更，如果有问题可提交issue进行讨论。

License
-------
[MIT](https://github.com/SkyChenSky/Sikiro.Dapper.Extension/blob/master/LICENSE)
