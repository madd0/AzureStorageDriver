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
    using System.Linq;
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

        public override string Name
        {
            get { return Resources.DriverName; }
        }

        public override System.Collections.Generic.List<ExplorerItem> GetSchemaAndBuildAssembly(IConnectionInfo cxInfo, System.Reflection.AssemblyName assemblyToBuild, ref string nameSpace, ref string typeName)
        {
            throw new NotImplementedException();
        }

        public override string GetConnectionDescription(IConnectionInfo cxInfo)
        {
            throw new NotImplementedException();
        }

        public override bool ShowConnectionDialog(IConnectionInfo cxInfo, bool isNewConnection)
        {
            throw new NotImplementedException();
        }
    }
}
