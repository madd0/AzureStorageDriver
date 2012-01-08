//-----------------------------------------------------------------------
// <copyright file="GenericEntity.cs" company="madd0.com">
//     Copyright (c) 2012 Mauricio DIAZ ORLICH.
//     Code licensed under the MIT X11 license.
// </copyright>
// <author>Mauricio DIAZ ORLICH</author>
//-----------------------------------------------------------------------

namespace Madd0.AzureStorageDriver
{
    using System.Collections.Generic;
    using Microsoft.WindowsAzure.StorageClient;

    /// <summary>
    /// Represents a generic entity from table storage.
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="TableServiceEntity"/>, which gives it the
    /// <see cref="PartitionKey"/>, <see cref="RowKey"/> and <see cref="Timestamp"/> properties,
    /// but all other properties that are obtained from table storage are simply stored in its
    /// <see cref="Properties"/> dictionary as string values, where the property name is the property in
    /// the dictionary.
    /// </remarks>
    public class GenericEntity : TableServiceEntity
    {
        /// <summary>
        /// Holds the property data.
        /// </summary>
        private Dictionary<string, string> properties = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        /// <value>The table name.</value>
        public string TableName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of properties of a table entity (except for PartitionKey, RowKey and
        /// Timestamp) and their string values.
        /// </summary>
        public Dictionary<string, string> Properties
        {
            get { return this.properties; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> with the specified property.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        public string this[string property]
        {
            get
            {
                return this.properties[property];
            }

            set
            {
                this.properties[property] = value;
            }
        }
    }
}
