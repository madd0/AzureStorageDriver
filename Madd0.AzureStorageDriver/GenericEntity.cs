//-----------------------------------------------------------------------
// <copyright file="GenericEntry.cs" company="">
//     Copyright (c) madd0, . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Madd0.AzureStorageDriver
{
    using System;
    using System.Linq;
    using Microsoft.WindowsAzure.StorageClient;
    using System.Data.Services.Common;
    using System.Collections.Generic;

    [DataServiceKey("PartitionKey", "RowKey")]
    public class GenericEntity : TableServiceEntity
    {
        Dictionary<string, string> properties = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        /// <value>The table name.</value>
        public string TableName
        {
            get;
            set;
        }

        public Dictionary<string, string> Properties
        {
            get { return this.properties; }
        }

        internal string this[string key]
        {
            get
            {
                return this.properties[key];
            }

            set
            {
                this.properties[key] = value;
            }
        }
    }
}
