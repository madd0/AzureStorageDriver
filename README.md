# README #

This is a plug-in for [LINQPad](http://www.linqpad.net) that enables the 
execution of queries against [Azure Table Storage](http://msdn.microsoft.com/en-us/library/windowsazure/dd179463.aspx).

It allows you to add storage accounts as connections in LINQPad. 
It will list all tables for each account and all columns in each table. 
Since Azure tables can have mixed schemas, the columns shown correspond
to all schemas combined. For the same reason, any value types are made
`Nullable<>` since it cannot be guaranteed that they will have a value
for a given row.

## Issues and Feature requests ##

Issues and feature requests can be made in the projects
[issues page on GitHub](https://github.com/madd0/AzureStorageDriver/issues).

## Versions ##

### v1.0.0-beta - 2010-01-08 ###

This is the first public release. Needs real-world testing.