using System;
using System.Collections.Generic;
using System.Linq;

namespace Madd0.AzureStorageDriver.Model
{
    // From: https://github.com/Azure/azure-libraries-for-net/blob/master/src/ResourceManagement/ResourceManager/AzureEnvironment.cs
    public class AzureEnvironment
    {
        static AzureEnvironment()
        {
            AzureGlobalCloud = new AzureEnvironment()
            {
                Name = "Global Cloud",
                AuthenticationEndpoint = "https://login.microsoftonline.com/",
                ResourceManagerEndpoint = "https://management.azure.com/",
                ManagementEndpoint = "https://management.core.windows.net/",
                GraphEndpoint = "https://graph.windows.net/",
                StorageEndpointSuffix = "core.windows.net",
                KeyVaultSuffix = "vault.azure.net"
            };
            AzureChinaCloud = new AzureEnvironment()
            {
                Name = "China Cloud",
                AuthenticationEndpoint = "https://login.chinacloudapi.cn/",
                ResourceManagerEndpoint = "https://management.chinacloudapi.cn/",
                ManagementEndpoint = "https://management.core.chinacloudapi.cn/",
                GraphEndpoint = "https://graph.chinacloudapi.cn/",
                StorageEndpointSuffix = "core.chinacloudapi.cn",
                KeyVaultSuffix = "vault.azure.cn"
            };
            AzureUSGovernment = new AzureEnvironment()
            {
                Name = "US Government",
                AuthenticationEndpoint = "https://login.microsoftonline.us/",
                ResourceManagerEndpoint = "https://management.usgovcloudapi.net/",
                ManagementEndpoint = "https://management.core.usgovcloudapi.net",
                GraphEndpoint = "https://graph.windows.net/",
                StorageEndpointSuffix = "core.usgovcloudapi.net",
                KeyVaultSuffix = "vault.usgovcloudapi.net"
            };
            AzureGermanCloud = new AzureEnvironment()
            {
                Name = "German Cloud",
                AuthenticationEndpoint = "https://login.microsoftonline.de/",
                ResourceManagerEndpoint = "https://management.microsoftazure.de/",
                ManagementEndpoint = "https://management.core.cloudapi.de",
                GraphEndpoint = "https://graph.cloudapi.de/",
                StorageEndpointSuffix = "core.cloudapi.de",
                KeyVaultSuffix = "vault.microsoftazure.de"
            };

            Environments = new Dictionary<string, AzureEnvironment>
            {
                [AzureGlobalCloud.Name] = AzureGlobalCloud,
                [AzureChinaCloud.Name] = AzureChinaCloud,
                [AzureUSGovernment.Name] = AzureUSGovernment,
                [AzureGermanCloud.Name] = AzureGermanCloud,
            };
        }

        public static AzureEnvironment AzureGlobalCloud
        {
            get; private set;
        }

        public static AzureEnvironment AzureChinaCloud
        {
            get; private set;
        }

        public static AzureEnvironment AzureUSGovernment
        {
            get; private set;
        }

        public static AzureEnvironment AzureGermanCloud
        {
            get; private set;
        }

        public static IEnumerable<AzureEnvironment> KnownEnvironments
        {
            get
            {
                yield return AzureGlobalCloud;
                yield return AzureChinaCloud;
                yield return AzureUSGovernment;
                yield return AzureGermanCloud;
            }
        }

        public static Dictionary<string, AzureEnvironment> Environments
        {
            get; private set;
        }

        /// <summary>
        /// Name for the cloud environment
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Azure active directory service endpoint to get OAuth token to access ARM resource
        /// management endpoint <service cref="ResourceManagerEndpoint" />.
        /// </summary>
        public string AuthenticationEndpoint { get; set; }

        /// <summary>
        /// Azure ARM resource management endpoint.
        /// </summary>
        public string ResourceManagerEndpoint { get; set; }

        /// <summary>
        /// Active Directory graph endpoint.
        /// </summary>
        public string GraphEndpoint { get; set; }

        /// <summary>
        /// Base URL for calls to service management (aka RDFE) and authentications to Active Directory.
        /// </summary>
        public string ManagementEndpoint { get; set; }

        /// <summary>
        /// The storage service url suffix.
        /// </summary>
        public string StorageEndpointSuffix { get; set; }

        /// <summary>
        /// The Key Vault service url suffix.
        /// </summary>
        public string KeyVaultSuffix { get; set; }

        /// <summary>
        /// Parse cloud environment name based on <see cref="KnownEnvironments"/>
        /// </summary>
        /// <param name="name">Cloud environment name</param>
        /// <returns>An AzureEnvironment, or null if no matching environment found</returns>
        public static AzureEnvironment FromName(string name)
            => KnownEnvironments.FirstOrDefault(env => string.Equals(env.Name, name, StringComparison.OrdinalIgnoreCase));
    }
}