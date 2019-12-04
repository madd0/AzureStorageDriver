//-----------------------------------------------------------------------
// <copyright file="StorageAccountProperties.cs" company="madd0.com">
//     Copyright (c) 2012 Mauricio DIAZ ORLICH.
//     Code licensed under the MIT X11 license.
// </copyright>
// <author>Mauricio DIAZ ORLICH</author>
//-----------------------------------------------------------------------

namespace Madd0.AzureStorageDriver
{
    using System.Xml.Linq;
    using LINQPad.Extensibility.DataContext;
    using Madd0.AzureStorageDriver.Properties;
#if NETCORE
    using Microsoft.Azure.Cosmos.Table;
#else
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Auth;
#endif

    /// <summary>
    /// Wrapper to expose typed properties over ConnectionInfo.DriverData.
    /// </summary>
    public class StorageAccountProperties
    {
        private readonly IConnectionInfo _connectionInfo;
        private readonly XElement _driverData;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageAccountProperties"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        public StorageAccountProperties(IConnectionInfo connectionInfo)
        {
            this._connectionInfo = connectionInfo;
            this._driverData = connectionInfo.DriverData;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this connection should be remembered.
        /// </summary>
        /// <value><c>true</c> if this connection should be remembered; otherwise, <c>false</c>.</value>
        public bool Persist
        {
            get { return this._connectionInfo.Persist; }
            set { this._connectionInfo.Persist = value; }
        }

        /// <summary>
        /// Gets the display name of the connection.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (this.UseLocalStorage)
                {
                    return Resources.LocalStorageDisplayName;
                }
                else
                {
                    return this.AccountName;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether local storage is being used.
        /// </summary>
        /// <value><c>true</c> if local storage is used; otherwise, <c>false</c>.</value>
        public bool UseLocalStorage
        {
            get
            {
                return (bool?)this._driverData.Element("UseLocalStorage") ?? false;
            }

            set
            {
                this._driverData.SetElementValue("UseLocalStorage", value);

                if (value)
                {
                    this.ClearAccountNameAndKey();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use HTTPS.
        /// </summary>
        /// <value><c>true</c> if HTTPS is used; otherwise, <c>false</c>.</value>
        public bool UseHttps
        {
            get
            {
                return (bool?)this._driverData.Element("UseHttps") ?? false;
            }

            set
            {
                this._driverData.SetElementValue("UseHttps", value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the storage account.
        /// </summary>
        public string AccountName
        {
            get { return (string)this._driverData.Element("AccountName") ?? string.Empty; }
            set { this._driverData.SetElementValue("AccountName", value); }
        }

        /// <summary>
        /// Gets or sets the key for the storage account.
        /// </summary>
        public string AccountKey
        {
            get
            {
                var encryptedKey = (string)this._driverData.Element("AccountKey") ?? string.Empty;
                return this._connectionInfo.Decrypt(encryptedKey);
            }

            set
            {
                var encryptedValue = this._connectionInfo.Encrypt(value);
                this._driverData.SetElementValue("AccountKey", encryptedValue);
            }
        }

        /// <summary>
        /// Gets or sets the number of rows to sample to determine a table's schema.
        /// </summary>
        public int NumberOfRows
        {
            get { return (int?)this._driverData.Element("NumberOfRows") ?? 1; }
            set { this._driverData.SetElementValue("NumberOfRows", value); }
        }

        /// <summary>
        /// Returns the maximum number of parallel model loading
        /// operations can occur when loading schema for the azure table storage tables
        /// </summary>
        public int ModelLoadMaxParallelism
        {
            get { return (int?)this._driverData.Element("ModelLoadMaxParallelism") ??
                    (System.Environment.ProcessorCount * 2); }
            set
            {
                this._driverData.SetElementValue("ModelLoadMaxParallelism", value);
            }
        }
        
        /// <summary>
        /// Gets a <see cref="CloudStorageAccount"/> instace for the current connection.
        /// </summary>
        /// <returns>A <see cref="CloudStorageAccount"/> instace configured with the credentials
        /// of the current connection.</returns>
        public CloudStorageAccount GetStorageAccount()
        {
            if (this.UseLocalStorage)
            {
                return CloudStorageAccount.DevelopmentStorageAccount;
            }
            else
            {
                return new CloudStorageAccount(new StorageCredentials(this.AccountName, this.AccountKey), this.UseHttps);
            }
        }

        /// <summary>
        /// Clears the account name and key.
        /// </summary>
        /// <remarks>This method is called when local storage is used.</remarks>
        private void ClearAccountNameAndKey()
        {
            var accountName = this._driverData.Element("AccountName");
            var accountKey = this._driverData.Element("AccountKey");

            if (null != accountName)
            {
                accountName.Remove();
            }

            if (null != accountKey)
            {
                accountKey.Remove();
            }
        }
    }
}
