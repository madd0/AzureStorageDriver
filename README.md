# README #

This is a plug-in for [LINQPad](http://www.linqpad.net) that enables the 
execution of queries against [Azure Table Storage](https://docs.microsoft.com/en-us/azure/cosmos-db/table-storage-overview).

It allows you to add storage accounts as connections in LINQPad. 
It will list all tables for each account and all columns in each table. 
Since Azure tables can have mixed schemas, a property was added in version 1.1.0
allowing users to specify the number of rows that should be scanned to determine 
the schema of each table. For the same reason, any value types are made
`Nullable<>` since it cannot be guaranteed that they will have a value
for a given row.

[![Build Status](https://dev.azure.com/madd0/AzureStorageDriver/_apis/build/status/madd0.AzureStorageDriver?branchName=master)](https://dev.azure.com/madd0/AzureStorageDriver/_build/latest?definitionId=4&branchName=master)

## Issues and Feature requests ##

Issues and feature requests can be made in the projects
[issues page on GitHub](https://github.com/madd0/AzureStorageDriver/issues).

## Versions ##

### v2.0.0 - 2019-12-01 ###

It took a while for a new version to come out, but with [LINQPad 6][3] 
and big changes in the Azure SDK libraries, I thought it was time.
And, although functionally not much has changed, I decided that a new 
distribution channel and total change in the underlying SDK, 
deserved a new major version.

* **New:** Available as [a Nuget package][4] for LINQPad 6.
* Switch to _Azure Cosmos DB Table API_
  * LINQPad 6 plugin uses [Microsoft.Azure.Cosmos.Table 1.0.5][5]
  * LINQPad 5 plugin uses [Microsoft.Azure.CosmosDB.Table 2.1.2][6]
    
    
_Note: the Azure Storage SDK used by the LINQPad 5 version is in maintenance mode 
and it will be deprecated soon according to Microsoft. However, since LINQPad 5 
plugins target .NET Framework 4.6, the switch to the package used by the LINQPad 6
plugin is not possible._

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
[3]: https://www.linqpad.net/LINQPad6.aspx
[4]: https://www.nuget.org/packages/Madd0.AzureStorageDriver/
[5]: https://www.nuget.org/packages/Microsoft.Azure.Cosmos.Table
[6]: https://www.nuget.org/packages/Microsoft.Azure.CosmosDB.Table