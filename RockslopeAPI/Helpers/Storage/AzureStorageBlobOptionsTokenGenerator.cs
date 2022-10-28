using System;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace RockslopeAPI.Helpers
{

    public class AzureStorageBlobOptionsTokenGenerator
    {
        public string AccountName { get; set; }
        public string FilePath { get; set; }
        public string ConnectionString { get; set; }

        public AzureStorageBlobOptionsTokenGenerator()
        {
            this.AccountName =
                Environment.GetEnvironmentVariable(
                    $"{nameof(AzureStorageBlobOptionsTokenGenerator)}:AccountName");
            this.ConnectionString =
                Environment.GetEnvironmentVariable(
                    $"{nameof(AzureStorageBlobOptionsTokenGenerator)}:ConnectionString");
            this.FilePath =
                Environment.GetEnvironmentVariable(
                    $"{nameof(AzureStorageBlobOptionsTokenGenerator)}:FilePath");
        }
        
        public string GenerateSasToken(string containerName)
        {
            return GenerateSasToken(containerName, DateTime.UtcNow.AddSeconds(30));
        }

        public string GenerateSasToken(string containerName, DateTime expiresOn)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

            SharedAccessBlobPermissions permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;

            string sasContainerToken;

            SharedAccessBlobPolicy shareAccessBlobPolicy =
                new SharedAccessBlobPolicy()
                {
                    SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                    SharedAccessExpiryTime = expiresOn,
                    Permissions = permissions
                };

            sasContainerToken = cloudBlobContainer.GetSharedAccessSignature(shareAccessBlobPolicy, null);

            return sasContainerToken;
        }
    }
}