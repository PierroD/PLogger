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
