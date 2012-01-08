//-----------------------------------------------------------------------
// <copyright file="CloudTable.cs" company="madd0.com">
//     Copyright (c) 2012 Mauricio DIAZ ORLICH.
//     Code licensed under the MIT X11 license.
// </copyright>
// <author>Mauricio DIAZ ORLICH</author>
//-----------------------------------------------------------------------

namespace Madd0.AzureStorageDriver
{
    using System.Collections.Generic;

    /// <summary>
    /// Holds information about a table from Azure storage.
    /// </summary>
    public class CloudTable
    {
        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the table's properties.
        /// </summary>
        public IEnumerable<TableColumn> Columns
        {
            get;
            set;
        }
    }
}
