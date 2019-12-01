//-----------------------------------------------------------------------
// <copyright file="CodeDomCompiler.cs" company="madd0.com">
//     Copyright (c) 2012 Mauricio DIAZ ORLICH.
//     Code licensed under the MIT X11 license.
// </copyright>
// <author>Mauricio DIAZ ORLICH</author>
//-----------------------------------------------------------------------

#if !NETCORE
namespace Madd0.AzureStorageDriver
{
    using Madd0.AzureStorageDriver.Properties;
    using Microsoft.CSharp;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class CodeDomCompiler : ICompiler
    {
        public void Compile(string code, AssemblyName name, IEnumerable<string> assemblyLocations)
        {
            CompilerResults results;

            var path = name.CodeBase;

            var dependencies = assemblyLocations.ToArray();

            using (var codeProvider = new CSharpCodeProvider())
            {
#if DEBUG
                var options = new CompilerParameters(dependencies, path, true);
#else
                var options = new CompilerParameters(dependencies, path, false);
#endif
                results = codeProvider.CompileAssemblyFromSource(options, code);
            }

            if (results.Errors.Count > 0)
            {
                throw new Exception(string.Format(Exceptions.CannotCompileCode, results.Errors[0].ErrorText, results.Errors[0].Line));
            }
        }
    }
}
#endif