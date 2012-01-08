//-----------------------------------------------------------------------
// <copyright file="AzureDriver.cs" company="madd0.com">
//     Copyright (c) 2012 Mauricio DIAZ ORLICH.
//     Code licensed under the MIT X11 license.
// </copyright>
// <author>Mauricio DIAZ ORLICH</author>
//-----------------------------------------------------------------------

namespace Madd0.AzureStorageDriver
{
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Reflection;
    using LINQPad.Extensibility.DataContext;
    using Madd0.AzureStorageDriver.Properties;

    /// <summary>
    /// LINQPad dynamic driver that lets users connect to an Azure Table Storage account.
    /// </summary>
    public class AzureDriver : DynamicDataContextDriver
    {
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
        public override string GetConnectionDescription(IConnectionInfo connectionInfo)
        {
            return new StorageAccountProperties(connectionInfo).DisplayName;
        }

        /// <summary>
        /// Shows the connection dialog.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="isNewConnection">if set to <c>true</c> [is new connection].</param>
        /// <returns></returns>
        public override bool ShowConnectionDialog(IConnectionInfo connectionInfo, bool isNewConnection)
        {
            if (isNewConnection)
            {
                new StorageAccountProperties(connectionInfo).UseLocalStorage = true;
            }

            bool? result = new ConnectionDialog(connectionInfo).ShowDialog();
            return result == true;
        }

        /// <summary>
        /// Determines whether two repositories are equivalent.
        /// </summary>
        /// <param name="connection1">The connection information of the first repository.</param>
        /// <param name="connection2">The connection information of the second repository.</param>
        /// <returns><c>true</c> if both repositories use the same account name; <c>false</c> otherwise.</returns>
        public override bool AreRepositoriesEquivalent(IConnectionInfo connection1, IConnectionInfo connection2)
        {
            var account1 = (string)connection1.DriverData.Element("AccountName") ?? string.Empty;
            var account2 = (string)connection2.DriverData.Element("AccountName") ?? string.Empty;

            return account1.Equals(account2);
        }

        /// <summary>
        /// Gets the assemblies to add.
        /// </summary>
        /// <returns>A list of assembly names to add in order to execute the current driver.</returns>
        public override IEnumerable<string> GetAssembliesToAdd()
        {
            return new string[]
                {
                    "System.Data.Services.Client.dll",
                    "Microsoft.WindowsAzure.StorageClient.dll"
                };
        }

        /// <summary>
        /// Gets the namespaces to add.
        /// </summary>
        /// <returns>A list of namespaces to add in order to execute this driver.</returns>
        public override IEnumerable<string> GetNamespacesToAdd()
        {
            return new string[]
                {
                    "System.Data.Services.Client",
                    "Microsoft.WindowsAzure",
                    "Microsoft.WindowsAzure.StorageClient"
                };
        }

        /// <summary>
        /// Gets the schema and builds the assembly that contains the typed data context.
        /// </summary>
        /// <param name="connectionInfo">The connection information.</param>
        /// <param name="assemblyToBuild">The assembly to build.</param>
        /// <param name="namepace">The namespace to be used in the generated code.</param>
        /// <param name="typeName">Name of the type of the typed data context.</param>
        /// <returns>A list of <see cref="ExplorerItem"/> instaces that describes the current schema.</returns>
        public override List<ExplorerItem> GetSchemaAndBuildAssembly(IConnectionInfo connectionInfo, AssemblyName assemblyToBuild, ref string @namespace, ref string typeName)
        {
            // The helper class SchemaBuilder will do the heavy lifting
            return SchemaBuilder.GetSchemaAndBuildAssembly(
                new StorageAccountProperties(connectionInfo),
                this.GetDriverFolder(),
                assemblyToBuild,
                @namespace,
                typeName);
        }

        /// <summary>
        /// Gets the context constructor arguments.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <returns>An ordered collection of objects to pass to the data context as arguments.</returns>
        public override object[] GetContextConstructorArguments(IConnectionInfo connectionInfo)
        {
            var properties = new StorageAccountProperties(connectionInfo);

            var storageAccount = properties.GetStorageAccount();

            return new object[]
            {
                storageAccount.TableEndpoint.ToString(),
                storageAccount.Credentials,
                storageAccount
            };
        }

        /// <summary>
        /// Gets the context constructor parameters.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <returns>A list of <see cref="ParameterDescriptor"/> objects that describe the parameters
        /// of the typed data context's constructor.</returns>
        public override ParameterDescriptor[] GetContextConstructorParameters(IConnectionInfo connectionInfo)
        {
            return new[] 
            { 
                new ParameterDescriptor("baseAddress", "System.String"),
                new ParameterDescriptor("credentials", "Microsoft.WindowsAzure.StorageCredentials"),
                new ParameterDescriptor("account", "Microsoft.WindowsAzure.CloudStorageAccount")
            };
        }

        /// <summary>
        /// Initializes the data context.
        /// </summary>
        /// <remarks>In this driver, initialization consists of listening to the 
        /// <see cref="DataServiceContext.SendingRequest"/> event in order to extract the requested
        /// URI and display it in the SQL tab.</remarks>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="context">The context.</param>
        /// <param name="executionManager">The execution manager.</param>
        public override void InitializeContext(IConnectionInfo connectionInfo, object context, QueryExecutionManager executionManager)
        {
            var dsContext = (DataServiceContext)context;

            dsContext.SendingRequest += (sender, e) => executionManager.SqlTranslationWriter.WriteLine(e.Request.RequestUri);
        }
    }
}
