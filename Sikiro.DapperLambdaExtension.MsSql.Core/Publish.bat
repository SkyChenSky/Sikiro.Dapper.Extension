@echo off
cd %~dp0

nuget push Sikiro.DapperLambdaExtension.MsSql.1.2.3.2.nupkg oy2ltyhtblj27ugxdq5qnzyiojebc4zebcy6khneiyswcm -Source https://api.nuget.org/v3/index.json

pause