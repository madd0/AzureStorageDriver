//-----------------------------------------------------------------------
// <copyright file="AzureDriver.cs" company="madd0.com">
//     Copyright (c) 2012 Mauricio DIAZ ORLICH.
//     Code licensed under the MIT X11 license.
// </copyright>
// <author>Mauricio DIAZ ORLICH</author>
//-----------------------------------------------------------------------

namespace Madd0.AzureStorageDriver
{
    using System;
    using System.Collections.Generic;
    using LINQPad.Extensibility.DataContext;
    using Madd0.AzureStorageDriver.Properties;

    /// <summary>
    /// LINQPad dynamic driver that lets users connect to an Azure Table Storage account.
    /// </summary>
    public class AzureDriver : DynamicDataContextDriver
    {
        public override string Author
        {
            get { return Resources.AuthorName; }
        }

        /// <summary>
        /// Gets the name of the driver.
        /// </summary>
        public override string Name
        {
            get { return Resources.DriverName; }
        }

        /// <summary>Gets the text to display in the root Schema Explorer node for a given connection info.</summary>
        /// <param name="cxInfo">The connection information.</param>
        /// <returns>The text to display in the root Schema Explorer node for a given connection info</returns>
        public override string GetConnectionDescription(IConnectionInfo connectionInfo)
        {
            return new StorageAccountProperties(connectionInfo).DisplayName;
        }

        public override bool ShowConnectionDialog(IConnectionInfo connectionInfo, bool isNewConnection)
        {
            if (isNewConnection)
            {
                new StorageAccountProperties(connectionInfo).UseLocalStorage = true;
            }

            bool? result = new ConnectionDialog(connectionInfo).ShowDialog();
            return result == true;
        }

        public override bool AreRepositoriesEquivalent(IConnectionInfo connection1, IConnectionInfo connection2)
        {
            var account1 = (string)connection1.DriverData.Element("AccountName") ?? string.Empty;
            var account2 = (string)connection2.DriverData.Element("AccountName") ?? string.Empty;

            return account1.Equals(account2);
        }

        public override List<ExplorerItem> GetSchemaAndBuildAssembly(IConnectionInfo connectionInfo, System.Reflection.AssemblyName assemblyToBuild, ref string nameSpace, ref string typeName)
        {
            return SchemaBuilder.GetSchemaAndBuildAssembly(
                new StorageAccountProperties(connectionInfo),
                assemblyToBuild,
                ref nameSpace,
                ref typeName);
        }
    }
}
