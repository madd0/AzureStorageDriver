//-----------------------------------------------------------------------
// <copyright file="RoslynCompiler.cs" company="madd0.com">
//     Copyright (c) 2012 Mauricio DIAZ ORLICH.
//     Code licensed under the MIT X11 license.
// </copyright>
// <author>Mauricio DIAZ ORLICH</author>
//-----------------------------------------------------------------------

#if NETCORE

namespace Madd0.AzureStorageDriver
{
    using LINQPad.Extensibility.DataContext;
    using Madd0.AzureStorageDriver.Properties;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    internal class RoslynCompiler : ICompiler
    {
        public CSharpCompilationOptions Options { get; } = new CSharpCompilationOptions(
           OutputKind.DynamicallyLinkedLibrary,
           reportSuppressedDiagnostics: true,
#if DEBUG
           optimizationLevel: OptimizationLevel.Debug,
#else
           optimizationLevel: OptimizationLevel.Release,
#endif
           generalDiagnosticOption: ReportDiagnostic.Error
       );

        public void Compile(string code, AssemblyName name, IEnumerable<string> assemblyLocations)
        {
            var platformAssemblies = DataContextDriver.GetCoreFxReferenceAssemblies();

            var references = platformAssemblies.Concat(assemblyLocations).Select(l => MetadataReference.CreateFromFile(l));

            var parseOptions = new CSharpParseOptions().WithPreprocessorSymbols("NETCORE");

            var compilation = CSharpCompilation.Create(
                name.Name,
                references: references,
                syntaxTrees: new SyntaxTree[] { CSharpSyntaxTree.ParseText(code, parseOptions) },
                options: this.Options
            );

            using var fileStream = File.OpenWrite(name.CodeBase);

            var results = compilation.Emit(fileStream);

            if (!results.Success)
            {
                throw new ArgumentException(string.Format(Exceptions.CannotCompileCode, results.Diagnostics[0].GetMessage(), results.Diagnostics[0].Location.GetLineSpan()));
            }
        }
    }
}

#endif