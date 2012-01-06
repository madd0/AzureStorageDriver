//-----------------------------------------------------------------------
// <copyright file="StorageAccountProperties.cs" company="madd0.com">
//     Copyright (c) 2012 Mauricio DIAZ ORLICH.
//     Code licensed under the MIT X11 license.
// </copyright>
// <author>Mauricio DIAZ ORLICH</author>
//-----------------------------------------------------------------------

namespace Madd0.AzureStorageDriver
{
    using System;
    using System.Linq;
    using LINQPad.Extensibility.DataContext;
    using System.Xml.Linq;
    using Madd0.AzureStorageDriver.Properties;
    using Microsoft.WindowsAzure;

    /// <summary>
    /// Wrapper to expose typed properties over ConnectionInfo.DriverData.
    /// </summary>
    public class StorageAccountProperties
    {
        private readonly IConnectionInfo _connectionInfo;
        private readonly XElement _driverData;

        public StorageAccountProperties(IConnectionInfo connectionInfo)
        {
            this._connectionInfo = connectionInfo;
            this._driverData = connectionInfo.DriverData;
        }

        public bool Persist
        {
            get { return this._connectionInfo.Persist; }
            set { this._connectionInfo.Persist = value; }
        }

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

        public bool UseLocalStorage
        {
            get
            {
                var currentValue = (string)this._driverData.Element("UseLocalStorage") ?? string.Empty;
                return Convert.ToBoolean(currentValue);
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

        public bool UseHttps
        {
            get
            {
                var currentValue = (string)this._driverData.Element("UseHttps") ?? string.Empty;
                return Convert.ToBoolean(currentValue);
            }

            set
            {
                this._driverData.SetElementValue("UseHttps", value);
            }
        }

        public string AccountName
        {
            get { return (string)this._driverData.Element("AccountName") ?? string.Empty; }
            set { this._driverData.SetElementValue("AccountName", value); }
        }

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

        public CloudStorageAccount GetStorageAccount()
        {
            if (this.UseLocalStorage)
            {
                return CloudStorageAccount.DevelopmentStorageAccount;
            }
            else
            {
                return new CloudStorageAccount(new StorageCredentialsAccountAndKey(this.AccountName, this.AccountKey), this.UseHttps);
            }
        }
    }
}
