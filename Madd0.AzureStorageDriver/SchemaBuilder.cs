//-----------------------------------------------------------------------
// <copyright file="SchemaBuilder.cs" company="madd0.com">
//     Copyright (c) 2012 Mauricio DIAZ ORLICH.
//     Code licensed under the MIT X11 license.
// </copyright>
// <author>Mauricio DIAZ ORLICH</author>
//-----------------------------------------------------------------------
namespace Madd0.AzureStorageDriver
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LINQPad.Extensibility.DataContext;
    using System.Reflection;
    using System.Xml.Linq;

    /// <summary>
    /// TODO: Provide summary section in the documentation header.
    /// </summary>
    internal static class SchemaBuilder
    {
        public static List<ExplorerItem> GetSchemaAndBuildAssembly(StorageAccountProperties properties, AssemblyName name, ref string nameSpace, ref string typeName)
        {
            // Read the EDM schema into an XDocument:
            //XDocument data;
            //using (XmlReader reader = GetSchemaReader(props))
            //    data = XDocument.Load(reader);

            // Generate the code using the ADO.NET Data Services classes:
            //string code;
            //using (XmlReader reader = data.CreateReader())
            //    code = GenerateCode(reader, nameSpace);

            // Compile the code into the assembly, using the assembly name provided:
            //BuildAssembly(code, name);

            // Use the schema to populate the Schema Explorer:
            //List<ExplorerItem> schema = GetSchema(data, out typeName);
            List<ExplorerItem> schema = GetSchema(null, out typeName);

            return schema;
        }

        private static List<ExplorerItem> GetSchema(XDocument data, out string typeName)
        {
            typeName = "TypeName";

            return new List<ExplorerItem>()
            {
                new ExplorerItem("TestItem", ExplorerItemKind.Category, ExplorerIcon.Table)
            };
        }
    }
}
