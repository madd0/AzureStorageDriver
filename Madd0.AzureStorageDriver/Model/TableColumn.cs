//-----------------------------------------------------------------------
// <copyright file="TableColumn.cs" company="madd0.com">
//     Copyright (c) 2012 Mauricio DIAZ ORLICH.
//     Code licensed under the MIT X11 license.
// </copyright>
// <author>Mauricio DIAZ ORLICH</author>
//-----------------------------------------------------------------------

namespace Madd0.AzureStorageDriver
{
    /// <summary>
    /// Holds information about a column of a table in Azure storage.
    /// </summary>
    public class TableColumn
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type name of the property.
        /// </summary>
        public string TypeName
        {
            get;
            set;
        }
    }
}
