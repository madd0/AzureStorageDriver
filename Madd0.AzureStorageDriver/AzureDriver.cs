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
    using System.Reflection;
    using LINQPad.Extensibility.DataContext;
    using Madd0.AzureStorageDriver.Properties;
#if NETCORE
    using Microsoft.Azure.Cosmos.Table;
#else
    using Microsoft.Azure.CosmosDB.Table;
#endif

    /// <summary>
    /// LINQPad dynamic driver that lets users connect to an Azure Table Storage account.
    /// </summary>
    public class AzureDriver : DynamicDataContextDriver
    {
#if DEBUG
        static AzureDriver()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            {
                if (args.Exception.StackTrace.Contains(typeof(AzureDriver).Namespace))
                {
                    System.Diagnostics.Debugger.Launch();
                }
            };

        }
#endif
        /// <summary>
        /// Gets the name of the driver author.
        /// </summary>
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
        public override string GetConnectionDescription(IConnectionInfo cxInfo)
        {
            return new StorageAccountProperties(cxInfo).DisplayName;
        }

        /// <summary>
        /// Shows the connection dialog.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="isNewConnection">if set to <c>true</c> [is new connection].</param>
        /// <returns></returns>
        public override bool ShowConnectionDialog(IConnectionInfo cxInfo, ConnectionDialogOptions dialogOptions)
        {
            if (dialogOptions.IsNewConnection)
            {
                _ = new StorageAccountProperties(cxInfo)
                {
                    UseHttps = true,
                    NumberOfRows = 100
                };
            }

            bool? result = new ConnectionDialog(cxInfo).ShowDialog();
            return result == true;
        }

        /// <summary>
        /// Determines whether two repositories are equivalent.
        /// </summary>
        /// <param name="c1">The connection information of the first repository.</param>
        /// <param name="c2">The connection information of the second repository.</param>
        /// <returns><c>true</c> if both repositories use the same account name; <c>false</c> otherwise.</returns>
        public override bool AreRepositoriesEquivalent(IConnectionInfo c1, IConnectionInfo c2)
        {
            var account1 = (string)c1.DriverData.Element("AccountName") ?? string.Empty;
            var account2 = (string)c2.DriverData.Element("AccountName") ?? string.Empty;

            return account1.Equals(account2);
        }

#if NETCORE
        public override IEnumerable<string> GetAssembliesToAdd(IConnectionInfo cxInfo)
        {
            return new string[]
                {
                    "Microsoft.Azure.Cosmos.Table.dll"
                };
        }
#else
        public override IEnumerable<string> GetAssembliesToAdd(IConnectionInfo cxInfo)
        {
            return new string[]
                {
                    "Microsoft.Azure.CosmosDB.Table.dll"
                };
        }
#endif
        /// <summary>
        /// Gets the schema and builds the assembly that contains the typed data context.
        /// </summary>
        /// <param name="cxInfo">The connection information.</param>
        /// <param name="assemblyToBuild">The assembly to build.</param>
        /// <param name="namepace">The namespace to be used in the generated code.</param>
        /// <param name="typeName">Name of the type of the typed data context.</param>
        /// <returns>A list of <see cref="ExplorerItem"/> instaces that describes the current schema.</returns>
        public override List<ExplorerItem> GetSchemaAndBuildAssembly(IConnectionInfo cxInfo, AssemblyName assemblyToBuild, ref string nameSpace, ref string typeName)
        {
            // The helper class SchemaBuilder will do the heavy lifting
            return SchemaBuilder.GetSchemaAndBuildAssembly(
                new StorageAccountProperties(cxInfo),
                this.GetDriverFolder(),
                assemblyToBuild,
                nameSpace,
                typeName);
        }

        /// <summary>
        /// Gets the context constructor arguments.
        /// </summary>
        /// <param name="cxInfo">The connection info.</param>
        /// <returns>An ordered collection of objects to pass to the data context as arguments.</returns>
        public override object[] GetContextConstructorArguments(IConnectionInfo cxInfo)
        {
            var properties = new StorageAccountProperties(cxInfo);

            var storageAccount = properties.GetStorageAccount();

            return new object[]
            {
                storageAccount.CreateCloudTableClient()
            };
        }

        /// <summary>
        /// Gets the context constructor parameters.
        /// </summary>
        /// <param name="cxInfo">The connection info.</param>
        /// <returns>A list of <see cref="ParameterDescriptor"/> objects that describe the parameters
        /// of the typed data context's constructor.</returns>
        public override ParameterDescriptor[] GetContextConstructorParameters(IConnectionInfo cxInfo)
        {
            return new[]
            {
                new ParameterDescriptor("client", typeof(CloudTableClient).FullName)
            };
        }
    }
}
