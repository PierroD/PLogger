## PLogger
This project is similar to Log Frameworks as like as (Nlog or Log4Net).
My goal is to build my own Log system.
I made eveything during my free time.


## Setup
To run this project, download ...

	
## Log Level

* Trace, Debug, Infos, Warns, Error, Fatal : you can use those in your own application
* Internal Error : this one is used by PLogger if you do a mistake with the app.config file or database

## Commun usage
This method is static for the moment so to call it
you just have to write :
``` 
Logger.Trace("your text");
Logger.Debug("your text");
Logger.Info("your text");
Logger.Warns("your text");
Logger.Error("your text");
Logger.Fatal("your text");
```

## File or Database

* File : give you the opportunity to save it in a file.log 
* Database : : give you the opportunity to save it in a database (you just need to create the table)

App.config for File & Database :
```
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
      <add saveType="file" fileName="PLogger" filePath="" detailMode="true"/>
      <add saveType="mysql" dbHost="localhost" dbName="PLogger" dbUser="root" dbPassword="123+aze" />
    </targets>
  </PLogger>
</configuration>
```
If you don't specify the filePath it will creat a file.log in the same folder of the solution.
detailMode unable the function used section
As you can see you are able to store it in a file and in a MySQL database at the sametime


## Function(s) Used
You are able to see which function your programm go through
if you you write
```
Logger.setFunctionPassedThrough(); // 
```
	

## Database 
used to make it work, feel free to use it
```
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

## File.log example 

Inside PLogger_14-02-2020.log
```
I  [INFOS] Light 14/02/2020 < 00:59:32.9616 > Info Test
⚠  [ERROR] Light 14/02/2020 < 00:59:33.2109 > ( Program.cs|Main|ligne.20 ) Error Test
```
ExceptionErrorPLogger_14_02_2020.log
```
IN [INTERNAL ERROR] Light  14/02/2020 < 00:36:20.4844 > MySql.Data.MySqlClient.MySqlException (0x80004005): You have an error in your SQL syntax; check the manual that corresponds to your MySQL server version for the right syntax to use near '[INFOS], Light, I  [INFOS] Light 14/02/2020 < 00:36:20.1456 > Info Test, )' at line 1
   à MySql.Data.MySqlClient.MySqlStream.ReadPacket()
   à MySql.Data.MySqlClient.NativeDriver.GetResult(Int32& affectedRow, Int64& insertedId)
   à MySql.Data.MySqlClient.Driver.GetResult(Int32 statementId, Int32& affectedRows, Int64& insertedId)
   à MySql.Data.MySqlClient.Driver.NextResult(Int32 statementId, Boolean force)
   à MySql.Data.MySqlClient.MySqlDataReader.NextResult()
   à MySql.Data.MySqlClient.MySqlCommand.ExecuteReader(CommandBehavior behavior)
   à MySql.Data.MySqlClient.MySqlCommand.ExecuteNonQuery()
   à PLogger.Class.Logger.writeToDatabase(MySqlConnection connection) dans C:\Users\Light\source\repos\PLogger\PLogger\PLogger\Class\Logger.cs:ligne 204
```
