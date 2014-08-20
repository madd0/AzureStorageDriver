# README #

This is a plug-in for [LINQPad](http://www.linqpad.net) that enables the 
execution of queries against [Azure Table Storage](http://msdn.microsoft.com/en-us/library/windowsazure/dd179463.aspx).

It allows you to add storage accounts as connections in LINQPad. 
It will list all tables for each account and all columns in each table. 
Since Azure tables can have mixed schemas, a property was added in version 1.1.0
allowing users to specify the number of rows that should be scanned to determine 
the schema of each table. For the same reason, any value types are made
`Nullable<>` since it cannot be guaranteed that they will have a value
for a given row.

## Issues and Feature requests ##

Issues and feature requests can be made in the projects
[issues page on GitHub](https://github.com/madd0/AzureStorageDriver/issues).

## Versions ##

### v1.1.0 - 2014-08-20 ###

* Uses latest version of [Azure Storage Client Library](https://github.com/Azure/azure-storage-net/) (4.2.0)
* Provides a solution to issue [#1][2] by adding a configuration parameter that allows users to specify
  the number of rows to be scanned to determine the schema of a table (100 by default).

### v1.0.1 - 2013-08-15 ###

Corrects issue [#4][1].

### v1.0.0-beta - 2010-01-08 ###

This is the first public release. Needs real-world testing.

[1]: https://github.com/madd0/AzureStorageDriver/issues/4
[2]: https://github.com/madd0/AzureStorageDriver/issues/1