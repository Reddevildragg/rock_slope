using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace RockslopeAPI.Helpers;

public class FileTransfer
{
    public class DownloadFileData
    {
        public byte[] bytes;
        public string fileContentType;
    }
    public static async Task<string> UploadFile(IFormFile fileInfo, ILogger logger)
    {
        AzureStorageBlobOptionsTokenGenerator azureStorageBlobOptionsTokenGenerator = new AzureStorageBlobOptionsTokenGenerator();

        logger.LogInformation(JsonConvert.SerializeObject(fileInfo, Formatting.Indented));

        string sasToken = azureStorageBlobOptionsTokenGenerator.GenerateSasToken(azureStorageBlobOptionsTokenGenerator.FilePath);

        StorageCredentials storageCredentials = new StorageCredentials(sasToken);

        CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, azureStorageBlobOptionsTokenGenerator.AccountName, null, true);

        CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

        CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(azureStorageBlobOptionsTokenGenerator.FilePath);

        string blobName = $"{Guid.NewGuid()}{Path.GetExtension(fileInfo.FileName)}";

        blobName = blobName.Replace("\"", "");

        CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

        cloudBlockBlob.Properties.ContentType = fileInfo.ContentType;

        await using (Stream fileStream = fileInfo.OpenReadStream())
        {
            await cloudBlockBlob.UploadFromStreamAsync(fileStream);
        }

        return blobName;
    }

    public static async Task<DownloadFileData> DownloadFile(string fileName, ILogger logger)
    {
        AzureStorageBlobOptionsTokenGenerator azureStorageBlobOptionsTokenGenerator = new AzureStorageBlobOptionsTokenGenerator();
            
        logger.LogInformation($"trigger function processed a request.");
        string sasToken = azureStorageBlobOptionsTokenGenerator.GenerateSasToken(azureStorageBlobOptionsTokenGenerator.FilePath);

        StorageCredentials storageCredentials = new StorageCredentials(sasToken);

        CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, azureStorageBlobOptionsTokenGenerator.AccountName, null, true);

        CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

        CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(azureStorageBlobOptionsTokenGenerator.FilePath);

        CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

        MemoryStream ms = new MemoryStream();

        await cloudBlockBlob.DownloadToStreamAsync(ms);

        return new DownloadFileData() { bytes = ms.ToArray(), fileContentType = cloudBlockBlob.Properties.ContentType };
    }
}