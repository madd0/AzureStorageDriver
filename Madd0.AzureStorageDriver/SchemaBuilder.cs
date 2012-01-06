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
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;
    using System.Data.Services.Client;
    using System.CodeDom.Compiler;
    using Microsoft.CSharp;
    using System.IO;

    /// <summary>
    /// TODO: Provide summary section in the documentation header.
    /// </summary>
    internal static class SchemaBuilder
    {

        public static List<ExplorerItem> GetSchemaAndBuildAssembly(StorageAccountProperties properties, string driverFolder, AssemblyName name, ref string nameSpace, ref string typeName)
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
            BuildAssembly(name, driverFolder, typeName, nameSpace);

            // Use the schema to populate the Schema Explorer:
            //List<ExplorerItem> schema = GetSchema(data, out typeName);
            List<ExplorerItem> schema = GetSchema(properties);

            return schema;
        }

        private static void BuildAssembly(AssemblyName name, string driverFolder, string typeName, string nameSpace)
        {
            var code = @"namespace " + nameSpace + @"
{
    public class " + typeName + @" : Microsoft.WindowsAzure.StorageClient.TableServiceContext
    {
        public " + typeName + @"(string baseAddress, Microsoft.WindowsAzure.StorageCredentials credentials)
            : base(baseAddress, credentials)
        {

        }
    }
}";
            CompilerResults results;
            using (var codeProvider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } }))
            {
                var options = new CompilerParameters(
                    new [] { "System.dll", "System.Core.dll", "System.Xml.dll", "System.Data.Services.Client.dll", Path.Combine(driverFolder, "Microsoft.WindowsAzure.StorageClient.dll") },
                    name.CodeBase,
                    true);
                results = codeProvider.CompileAssemblyFromSource(options, code);
            }
            if (results.Errors.Count > 0)
                throw new Exception
                    ("Cannot compile typed context: " + results.Errors[0].ErrorText + " (line " + results.Errors[0].Line + ")");
        }

        private static List<ExplorerItem> GetSchema(StorageAccountProperties properties)
        {
            var tableClient = properties.GetStorageAccount().CreateCloudTableClient();
            var dataContext = tableClient.GetDataServiceContext();
            dataContext.ReadingEntity += OnReadingEntity;

            return (from tableName in tableClient.ListTables()
                    select new ExplorerItem(tableName, ExplorerItemKind.QueryableObject, ExplorerIcon.Table)
                    {
                        Children = (from columnName in dataContext.CreateQuery<GenericEntity>(tableName).Take(1).First().Properties
                                    select new ExplorerItem(columnName.Key + " (" + columnName.Value + ")", ExplorerItemKind.Property, ExplorerIcon.Column)).ToList()
                    }).ToList();
        }

        static void OnReadingEntity(object sender, ReadingWritingEntityEventArgs e)
        {
            // TODO: Make these statics   
            XNamespace AtomNamespace = "http://www.w3.org/2005/Atom";
            XNamespace AstoriaDataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace AstoriaMetadataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            GenericEntity entity = e.Entity as GenericEntity;

            if (entity == null)
            {
                return;
            }

            entity.TableName = e.Data.Element(AtomNamespace + "link").Attribute("title").Value;

            // read each property, type and value in the payload   
            //var properties = e.Entity.GetType().GetProperties();
            //where properties.All(pp => pp.Name != p.Name.LocalName)
            var q = from p in e.Data.Element(AtomNamespace + "content")
                                    .Element(AstoriaMetadataNamespace + "properties")
                                    .Elements()
                    select new
                    {
                        Name = p.Name.LocalName,
                        IsNull = string.Equals("true", p.Attribute(AstoriaMetadataNamespace + "null") == null ? null : p.Attribute(AstoriaMetadataNamespace + "null").Value, StringComparison.OrdinalIgnoreCase),
                        TypeName = p.Attribute(AstoriaMetadataNamespace + "type") == null ? "Edm.String" : p.Attribute(AstoriaMetadataNamespace + "type").Value,
                        p.Value
                    };

            foreach (var dp in q)
            {
                entity[dp.Name] = dp.TypeName;
            }
        }

        private static Type GetType(string type)
        {
            if (type == null)
                return typeof(string);

            switch (type)
            {
                case "Edm.String": return typeof(string);
                case "Edm.Byte": return typeof(byte);
                case "Edm.SByte": return typeof(sbyte);
                case "Edm.Int16": return typeof(short);
                case "Edm.Int32": return typeof(int);
                case "Edm.Int64": return typeof(long);
                case "Edm.Double": return typeof(double);
                case "Edm.Single": return typeof(float);
                case "Edm.Boolean": return typeof(bool);
                case "Edm.Decimal": return typeof(decimal);
                case "Edm.DateTime": return typeof(DateTime);
                case "Edm.Binary": return typeof(byte[]);
                case "Edm.Guid": return typeof(Guid);

                default: throw new NotSupportedException("Not supported type " + type);
            }
        }
    }
}
