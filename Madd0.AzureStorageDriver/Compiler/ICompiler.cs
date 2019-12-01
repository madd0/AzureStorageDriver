//-----------------------------------------------------------------------
// <copyright file="ICompiler.cs" company="madd0.com">
//     Copyright (c) 2012 Mauricio DIAZ ORLICH.
//     Code licensed under the MIT X11 license.
// </copyright>
// <author>Mauricio DIAZ ORLICH</author>
//-----------------------------------------------------------------------

namespace Madd0.AzureStorageDriver
{
    using System.Collections.Generic;
    using System.Reflection;

    internal interface ICompiler
    {
        void Compile(string code, AssemblyName name, IEnumerable<string> assemblyLocations);
    }
}