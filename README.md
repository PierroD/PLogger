## PLogger
This project is similar to ([NLog](https://nlog-project.org) or [Log4Net](https://logging.apache.org/log4net/)) Frameworks.
My goal is to build my own Log system.


## Setup
To run this project, download ...

    
## Log Level

* ````Trace````, ```Debug```, ```Infos```, ```Warns```, ```Error```, ```Fatal``` : you can use those in your application
* ```Internal Error``` : this one is used by PLogger if you do a mistake with the App.config file or database

## Commun usage
This method is static for the moment so to call it
you just have to write ``Logger.*``:
```c
Logger.Trace("your text"); // >> [TRACE]
Logger.Debug("your text"); // ?? [DEBUG]
Logger.Infos("your text"); // I  [INFOS]
Logger.Warns("your text"); // W  [WARNS]
Logger.Error("your text"); // !! [ERROR]
Logger.Fatal("your text"); // F  [FATAL]
```

## File | Database | Json

* File : allow you to save your logs in a .log file
* Database : : allow you to save your log inside a mysql database (you just need to create the table)
* Json : allow you to save your logs in a .json file

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
       <add saveType="mysql" dbHost="localhost" dbName="PLogger" dbUser="root" dbPassword="root" detailMode="true" />
      <add saveType="json" fileName="PLogger" filePath="" detailMode="true"/>
      <add saveType="file" fileName="PLogger" filePath="" detailMode="true"/>
    </targets>
  </PLogger>
</configuration>
```
If you don't specify the filePath it will creat a .log or .json file in the same folder than the solution.
detailMode allow you to see which function your programm went through (really usefull if someone else try to find something in your project which contains 1K+ lines)
You are also able to save your logs inside a MySQL database.


## DetailMode 
 - PLogger is able to give you (where it get called) : 
``` 
the file|the method|this ligne
 ```` 


- To use detail mode set it to "true" in the App.config :
```xml
<add saveType="mysql" dbHost="localhost" dbName="PLogger" dbUser="root" dbPassword="root" detailMode="true" />
```
- To make it works, add the following line to evey method :
```
Logger.setFunctionPassedThrough(); // before any Logger.*
Logger.Infos("this is an informational test");
```
##### Example
```
!! [ERROR] PierroD 14/02/2020 < 20:31:47.9161 > ( Program.cs|Main|ligne.20 ) Error Test
```

## Database (MySQL)
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
   <add saveType="mysql" dbHost="localhost" dbName="PLogger" dbUser="root" dbPassword="root" detailMode="true" />
```
#### Example
```sql
+----+---------+----------+------------+--------------------------+---------------------+
| id | type    | username | message    | passed_through           | created_at          |
+----+---------+----------+------------+--------------------------+---------------------+
| 43 | [INFOS] | PierroD    | Info Test  | NULL                     | 2020-02-14 20:31:47 |
| 44 | [ERROR] | PierroD    | Error Test | Program.cs|Main|ligne.20 | 2020-02-14 20:31:47 |
+----+---------+----------+------------+--------------------------+---------------------+

```

## JSON
- Add this line in your App.config (add it before the file.log one)
```xml
     <add saveType="json" fileName="PLogger" filePath="" detailMode="true"/>
```
##### Example
```json
{
    "PLogger": {
        "Log": [
            {
                "type": "[INFOS]",
                "message": "Info Test",
                "date": "14/02/2020",
                "created_at": "20:28:57.0574",
                "passed_through": null
            },
            {
                "type": "[ERROR]",
                "message": "Error Test",
                "date": "14/02/2020",
                "created_at": "20:28:57.2948",
                "passed_through": "Program.cs|Main|ligne.20"
            },
            {
                "type": "[INFOS]",
                "message": "Info Test",
                "date": "14/02/2020",
                "created_at": "20:31:47.7287",
                "passed_through": null
            },
            {
                "type": "[ERROR]",
                "message": "Error Test",
                "date": "14/02/2020",
                "created_at": "20:31:47.9161",
                "passed_through": "Program.cs|Main|ligne.20"
            }
        ]
    }
}
```

## File.log | Example 

- PLogger_14-02-2020.log
```
I  [INFOS] PierroD 14/02/2020 < 00:59:32.9616 > Info Test
!! [ERROR] PierroD 14/02/2020 < 00:59:33.2109 > ( Program.cs|Main|ligne.20 ) Error Test
```
- ExceptionErrorPLogger_14-02-2020.log
```
IE [INTERNAL ERROR] PierroD  14/02/2020 < 00:36:20.4844 > MySql.Data.MySqlClient.MySqlException (0x80004005): You have an error in your SQL syntax; check the manual that corresponds to your MySQL server version for the right syntax to use near '[INFOS], PierroD, I  [INFOS] PierroD 14/02/2020 < 00:36:20.1456 > Info Test, )' at line 1
   à MySql.Data.MySqlClient.MySqlStream.ReadPacket()
   à MySql.Data.MySqlClient.NativeDriver.GetResult(Int32& affectedRow, Int64& insertedId)
   à MySql.Data.MySqlClient.Driver.GetResult(Int32 statementId, Int32& affectedRows, Int64& insertedId)
   à MySql.Data.MySqlClient.Driver.NextResult(Int32 statementId, Boolean force)
   à MySql.Data.MySqlClient.MySqlDataReader.NextResult()
   à MySql.Data.MySqlClient.MySqlCommand.ExecuteReader(CommandBehavior behavior)
   à MySql.Data.MySqlClient.MySqlCommand.ExecuteNonQuery()
   à PLogger.Class.Logger.writeToDatabase(MySqlConnection connection) dans C:\Users\PierroD\source\repos\PLogger\PLogger\PLogger\Class\Logger.cs:ligne 204
```
