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
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using LINQPad.Extensibility.DataContext;
    using Madd0.AzureStorageDriver.Properties;
    using Microsoft.CSharp;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Provides the methods necessary to determining the storage account's schema and to building 
    /// the typed data context .
    /// </summary>
    internal static class SchemaBuilder
    {
        // Names of columns that should be marked as table keys.
        private static readonly List<string> keyColumns = new List<string> { "PartitionKey", "RowKey" };

        /// <summary>
        /// Gets the schema and builds the assembly.
        /// </summary>
        /// <param name="properties">The current configuration.</param>
        /// <param name="driverFolder">The driver folder. Used to resolve dependencies.</param>
        /// <param name="name">The <see cref="AssemblyName"/> instace of the assembly being created.</param>
        /// <param name="namepace">The namespace to be used in the generated code.</param>
        /// <param name="typeName">Name of the type of the typed data context.</param>
        /// <returns>A list of <see cref="ExplorerItem"/> instaces that describes the current schema.</returns>
        public static List<ExplorerItem> GetSchemaAndBuildAssembly(StorageAccountProperties properties, string driverFolder, AssemblyName name, string @namepace, string typeName)
        {
            // Get the model from Azure storage
            var model = GetModel(properties);

            // Generate C# code
            var code = GenerateCode(typeName, @namepace, model);

            // And compile the code into the assembly
            BuildAssembly(name, driverFolder, code);

            // Generate the schema for LINQPad
            List<ExplorerItem> schema = GetSchema(model);

            return schema;
        }

        /// <summary>
        /// Build a model of the current Azure storage account. This model will be used to generate
        /// the typed code as well as the schema needed by LINQPad.
        /// </summary>
        /// <param name="properties">The current configuration.</param>
        /// <returns>A list of <see cref="CloudTable"/> instances that describe the current Azure
        /// storage model.</returns>
        private static IEnumerable<CloudTable> GetModel(StorageAccountProperties properties)
        {
            var tableClient = properties.GetStorageAccount().CreateCloudTableClient();

            // First get a list of all tables
            var model = (from tableName in tableClient.ListTables()
                         select new CloudTable
                         {
                             Name = tableName.Name
                         }).ToList();

            // Then go through them
            foreach (var table in model)
            {
                var tableColumns = tableClient.GetTableReference(table.Name).ExecuteQuery(new TableQuery().Take(properties.NumberOfRows))
                    .SelectMany(row => row.Properties)
                    .GroupBy(column => column.Key)
                    .Select(grp => new TableColumn
                     {
                         Name = grp.Key,
                         TypeName = GetType(grp.First().Value.PropertyType)
                     });

                var baseColumns = new List<TableColumn>
                {
                    new TableColumn { Name = "PartitionKey", TypeName = GetType(EdmType.String) },
                    new TableColumn { Name = "RowKey", TypeName = GetType(EdmType.String) },
                    new TableColumn { Name = "Timestamp", TypeName = GetType(EdmType.DateTime) },
                    new TableColumn { Name = "ETag", TypeName = GetType(EdmType.String) }
                };

                table.Columns = tableColumns.Concat(baseColumns).ToArray();
            }

            return model;
        }

        /// <summary>
        /// Generates the code to build the typed data context.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="namespace">The namespace.</param>
        /// <param name="model">The model.</param>
        /// <returns>The code to be compiled as a string.</returns>
        private static string GenerateCode(string typeName, string @namespace, IEnumerable<CloudTable> model)
        {
            // We use a T4-generated class as the template
            var codeGenerator = new DataContextTemplate();

            codeGenerator.Namespace = @namespace;
            codeGenerator.TypeName = typeName;
            codeGenerator.Tables = model;

            return codeGenerator.TransformText();
        }

        /// <summary>
        /// Builds the assembly described by the <see cref="AssemblyName"/>.
        /// </summary>
        /// <param name="name">The <see cref="AssemblyName"/> instace of the assembly being created.</param>
        /// <param name="driverFolder">The driver folder. Used to resolve dependencies.</param>
        /// <param name="code">The code of the typed data context.</param>
        private static void BuildAssembly(AssemblyName name, string driverFolder, string code)
        {
            CompilerResults results;

            var dependencies = new[] 
            { 
                "System.dll",
                "System.Core.dll",
                "System.Xml.dll",
                Path.Combine(driverFolder, "Madd0.AzureStorageDriver.dll"), 
                Path.Combine(driverFolder, "Microsoft.WindowsAzure.Storage.dll"), 
                Path.Combine(driverFolder, "Microsoft.Data.Services.Client.dll") 
            };

            // Use the CSharpCodeProvider to compile. Since the driver is .NET 4.0, the typed assembly should be also
            using (var codeProvider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } }))
            {
#if DEBUG
                var options = new CompilerParameters(dependencies, name.CodeBase, true);
#else
                var options = new CompilerParameters(dependencies, name.CodeBase, false);
#endif
                results = codeProvider.CompileAssemblyFromSource(options, code);
            }

            if (results.Errors.Count > 0)
            {
                throw new Exception(string.Format(Exceptions.CannotCompileCode, results.Errors[0].ErrorText, results.Errors[0].Line));
            }
        }

        /// <summary>
        /// Transforms the model based on <see cref="CloudTable"/> instances into a schema based on
        /// <see cref="ExplorerItem"/> instances for LINQPad.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A schema for LINQPad.</returns>
        private static List<ExplorerItem> GetSchema(IEnumerable<CloudTable> model)
        {
            return (from table in model
                    select new ExplorerItem(table.Name, ExplorerItemKind.QueryableObject, ExplorerIcon.Table)
                    {
                        Children = (from column in table.Columns
                                    select new ExplorerItem(column.Name + " (" + column.TypeName + ")", ExplorerItemKind.Property, ExplorerIcon.Column)
                                    {
                                        Icon = keyColumns.Contains(column.Name) ? ExplorerIcon.Key : ExplorerIcon.Column,
                                        DragText = column.Name
                                    }).ToList(),
                        DragText = table.Name,
                        IsEnumerable = true
                    }).ToList();
        }

        /// <summary>
        /// Gets the C# type equivalent of an entity data model (Edm) type.
        /// </summary>
        /// <param name="type">The Edm type.</param>
        /// <returns>The C# type.</returns>
        private static string GetType(EdmType type)
        {
            switch (type)
            {
                case EdmType.Binary:
                    return "byte[]";
                case EdmType.Boolean:
                    return "bool?";
                case EdmType.DateTime:
                    return "DateTime?";
                case EdmType.Double:
                    return "double?";
                case EdmType.Guid:
                    return "Guid?";
                case EdmType.Int32:
                    return "int?";
                case EdmType.Int64:
                    return "long?";
                case EdmType.String:
                    return "string";
                default:
                    throw new NotSupportedException(string.Format(Exceptions.TypeNotSupported, type));
            }
        }
    }
}
