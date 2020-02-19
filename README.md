# PLogger

##### Lastest version 1.1 : <a href="https://raw.githubusercontent.com/PierroD/PLogger/master/bin/Release/PLoggerv1.1.zip">Download link</a>

##### Oldest version 1.0 : <a href="https://raw.githubusercontent.com/PierroD/PLogger/master/bin/Release/PLoggerv1.0.zip">Download link</a>

This project is similar to ([NLog](https://nlog-project.org) or [Log4Net](https://logging.apache.org/log4net/)) Frameworks.
My goal is to build my own Log system.

## Summary

- [Setup](#setup)
- [Log Level](#log-level)
- [Commun usage](#commun-usage)
- [Differents logs type](#differents-logs-type)
- [DetailMode](#detailmode)
- [Minimum Level](#minimum-level)
- [Database (MySQL)](#database-mysql)
- [Json](#json)
- [File.log example](#file-log-example)
- [Release notes](#release-notes)

## <a name="setup"></a>Setup

Click on the download link,
then add it as a reference in your .csproj.
And in the using section of your class (.cs) add :

```c#
using PLogger
```

## <a name="log-level"></a>Log Level

- `Trace`, `Debug`, `Infos`, `Warns`, `Error`, `Fatal` : you can use those in your application
- `Internal Error` : this one is used by PLogger if you do a mistake with the App.config file or database

## Commun usage

This method is static for the moment so to call it
you just have to write `Logger.*`:

[Single Parameter]

```c#
Log.Trace("your text"); // >> [TRACE]
Log.Debug("your text"); // ?? [DEBUG]
Log.Infos("your text"); // I  [INFOS]
Log.Warns("your text"); // W  [WARNS]
Log.Error("your text"); // !! [ERROR]
Log.Fatal("your text"); // F  [FATAL]
```

[Multiple Parameters]

```c#
// you don't need to add a string
Log.Trace(object, ["your text"]); // >> [TRACE]
Log.Debug(object, ["your text"]); // ?? [DEBUG]
Log.Infos(object, ["your text"]); // I  [INFOS]
Log.Warns(object, ["your text"]); // W  [WARNS]
Log.Error(object, ["your text"]); // !! [ERROR]
Log.Fatal(object, ["your text"]); // F  [FATAL]
// example
try{
  ...
}
catch( Exception e){
  Log.Error(e);
  //or
  Log.Error(e, "Your Error is : ");
}
```

## <a name="differents-logs-type"></a>Differents logs type

- File : allow you to save your logs in a .log file
- Database : : allow you to save your log inside a mysql database (you just need to create the table)
- Json : allow you to save your logs in a .json file

App.config for File & Database & Json :

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="PLogger"
             type="PLogger.Configuration.PLoggerConfig, PLogger"/>
  </configSections>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
  </startup>
  <PLogger>
    <targets>
      <add saveType="mysql" dbHost="localhost" dbName="PLogger" dbUser="root" dbPassword="root" minLevel="Trace" detailMode="true"/>
      <add saveType="json" fileName="PLogger" filePath="" minLevel="Infos" detailMode="true"/>
      <add saveType="file" fileName="PLogger" filePath="" minLevel="Warns" detailMode="true"/>
    </targets>
  </PLogger>
</configuration>
```

If you don't specify the filePath it will create a .log or .json file in the same directory than the solution.
detailMode allow you to see which function your programm went through (really usefull if someone else try to find something in your project which contains 1K+ lines)
You are also able to save your logs inside a MySQL database.

## <a name="detailmode"></a>DetailMode

- PLogger is able to give you (where it get called) :

```
the file|the method|this ligne
```

- To use detail mode set it to "true" in the App.config :

```xml
<add ... detailMode="true" />
```

- To make it works, add the following line to evey method :

```c#
Log.setFunctionPassedThrough(); // before any Log.*
Log.Infos("this is an informational test");
```

##### Example

```
!! [ERROR] PierroD 14/02/2020 < 20:31:47.9161 > ( Program.cs|Main|ligne.20 ) Error Test
```

## <a name="minimum-level"></a>Minimum Level

- Allow you to choose which level you want to see :

-- Trace : minimum level used to get more details in your `logs` (use it with Log.setFunctionPassedThrough())

-- Debug : Use it when you are building your application

-- Infos : Use it to let some information in your `logs`

-- Warns : Use it to inform users about an error

-- Error : Use it when there is an important / critical error

-- Fatal : Use it when there is an unexpected error (crashs...)

- To use this minimum level set it to the minimum level of errors you wants to appear in your `logs` in the App.config :

```xml
<add ... minLevel="Debug" />
```

##### Example

```c#
// minLevel = "Trace"
I  [INFOS] PierroD 16/02/2020 < 19:28:40.3554 > Informational Test
?? [DEBUG] PierroD 16/02/2020 < 19:28:40.3883 > ( Program.cs|TestDebugFunction|ligne.21 ) Debug Test
!! [ERROR] PierroD 16/02/2020 < 19:28:40.4392 > ( Program.cs|TestDebugFunction|ligne.21 => Program.cs|TestErrorFunction|ligne.26 ) Error Test
// minLevel = "Warns"
!! [ERROR] PierroD 16/02/2020 < 19:30:36.8681 > ( Program.cs|TestDebugFunction|ligne.21 => Program.cs|TestErrorFunction|ligne.26 ) Error Test
```

## <a name="database-mysql"></a>Database (MySQL)

- To make it work you have to build the same table :

```sql
create database PLogger;
use PLogger;
create table Log(
id int not null auto_increment primary key,
type varchar(7),
username varchar(255),
message varchar(255),
passed_through varchar(255),
created_at timestamp DEFAULT current_timestamp
);
```

- Add this line in your App.config (Add it before the file.log one)

```xml
   <add saveType="mysql" dbHost="localhost" dbName="PLogger" dbUser="root" dbPassword="root" minLevel="Trace" detailMode="true"/>
```

#### Example

```sql
+----+---------+------------+--------------------+--------------------------------------------------------------------------------+---------------------+
| id | type    | username   | message            | passed_through                                                                 | created_at          |
+----+---------+------------+--------------------+--------------------------------------------------------------------------------+---------------------+
| 49 | [INFOS] | PierroD    | Informational Test | NULL                                                                           | 2020-02-15 18:56:19 |
| 50 | [DEBUG] | PierroD    | Debug Test         | Program.cs|TestDebugFunction|ligne.21                                          | 2020-02-15 18:56:20 |
| 51 | [ERROR] | PierroD    | Error Test         | Program.cs|TestDebugFunction|ligne.21 => Program.cs|TestErrorFunction|ligne.26 | 2020-02-15 18:56:20 |
+----+---------+------------+--------------------+--------------------------------------------------------------------------------+---------------------+

```

## <a name="json"></a>JSON

- Add this line in your App.config (add it before the file.log one)

```xml
     <add saveType="json" fileName="PLogger" filePath=""  minLevel="Trace" detailMode="true"/>
```

##### Example

```json
{
  "PLogger": {
    "Logs": [
      {
        "type": "[INFOS]",
        "username": "PierroD",
        "message": "Informational Test",
        "date": "15/02/2020",
        "created_at": "17:28:06.2906",
        "passed_through": null
      },
      {
        "type": "[DEBUG]",
        "username": "PierroD",
        "message": "Debug Test",
        "date": "15/02/2020",
        "created_at": "17:28:06.5249",
        "passed_through": "Program.cs|TestDebugFunction|ligne.21"
      },
      {
        "type": "[ERROR]",
        "username": "PierroD",
        "message": "Error Test",
        "date": "15/02/2020",
        "created_at": "17:28:06.5561",
        "passed_through": "Program.cs|TestDebugFunction|ligne.21 => Program.cs|TestErrorFunction|ligne.26"
      }
    ]
  }
}
```

## <a name="file-log-example"></a>File.log | Example

- PLogger_15-02-2020.log

```
I  [INFOS] PierroD 15/02/2020 < 18:56:20.1491 > Informational Test
?? [DEBUG] PierroD 15/02/2020 < 18:56:20.1970 > ( Program.cs|TestDebugFunction|ligne.21 ) Debug Test
!! [ERROR] PierroD 15/02/2020 < 18:56:20.2428 > ( Program.cs|TestDebugFunction|ligne.21 => Program.cs|TestErrorFunction|ligne.26 ) Error Test

```

- ExceptionErrorPLogger_15-02-2020.log

```
IE [INTERNAL ERROR] PierroD  15/02/2020 < 19:06:49.5432 > MySql.Data.MySqlClient.MySqlException (0x80004005): Authentication to host 'localhost' for user 'rooot' using method 'mysql_native_password' failed with message: Access denied for user 'rooot'@'localhost' (using password: YES) ---> MySql.Data.MySqlClient.MySqlException (0x80004005): Access denied for user 'rooot'@'localhost' (using password: YES)
   à MySql.Data.MySqlClient.MySqlStream.ReadPacket()
   à MySql.Data.MySqlClient.Authentication.MySqlAuthenticationPlugin.ReadPacket()
   à MySql.Data.MySqlClient.Authentication.MySqlAuthenticationPlugin.AuthenticationFailed(Exception ex)
   à MySql.Data.MySqlClient.Authentication.MySqlAuthenticationPlugin.ReadPacket()
   à MySql.Data.MySqlClient.Authentication.MySqlAuthenticationPlugin.ContinueAuthentication(Byte[] data)
   à MySql.Data.MySqlClient.Authentication.MySqlAuthenticationPlugin.HandleAuthChange(MySqlPacket packet)
   à MySql.Data.MySqlClient.Authentication.MySqlAuthenticationPlugin.Authenticate(Boolean reset)
   à MySql.Data.MySqlClient.NativeDriver.Authenticate(String authMethod, Boolean reset)
   à MySql.Data.MySqlClient.NativeDriver.Open()
   à MySql.Data.MySqlClient.Driver.Open()
   à MySql.Data.MySqlClient.Driver.Create(MySqlConnectionStringBuilder settings)
   à MySql.Data.MySqlClient.MySqlPool.CreateNewPooledConnection()
   à MySql.Data.MySqlClient.MySqlPool.GetPooledConnection()
   à MySql.Data.MySqlClient.MySqlPool.TryToGetDriver()
   à MySql.Data.MySqlClient.MySqlPool.GetConnection()
   à MySql.Data.MySqlClient.MySqlConnection.Open()
   à PLogger.Class.Logger.writeToDatabase(MySqlConnection connection, Boolean detail) dans C:\Users\PierroD\source\repos\PLogger\PLogger\PLogger\Class\Logger.cs:ligne 227

```

## <a name="release-notes"></a>Release Notes

```
v1.1
--> Update
- Simplify Logger by Log
- Add multiple parameters message
- Add an "s" in the Json for 'Log'
v1.0 // Release
--> Update
- Change console to dll for the release
- Add Json
- Add Min level
v0.5
--> Rebuild
- Add MySQL
- Add DetailMode (file trace)
v0.4
--> Update
- Improve app config reader
- Add Internal Error
v0.3
--> Update
 - Read a config file
 - Add Warns, and Fatal message type
v0.2
--> Update
- Rolling file
- 4 messages type (Trace, Debug, Infos, Error) // all should be 5 characters long
v0.1
--> Update
- Write a string in a file
v0.0
--> Starting the project
```
