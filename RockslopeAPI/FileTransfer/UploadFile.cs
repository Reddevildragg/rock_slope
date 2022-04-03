using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace RockslopeAPI.FileTransfer
{
    public static class UploadFile
    {
        [FunctionName("FileUploadHttpTrigger")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "files")] HttpRequestMessage req,
                                                         ILogger logger)
        {
            /*
            AzureStorageBlobOptionsTokenGenerator azureStorageBlobOptionsTokenGenerator = new AzureStorageBlobOptionsTokenGenerator();

            logger.LogInformation("trigger function processed a request.");

            MultipartMemoryStreamProvider multipartMemoryStreamProvider = new MultipartMemoryStreamProvider();

            await req.Content.ReadAsMultipartAsync(multipartMemoryStreamProvider);

            HttpContent file = multipartMemoryStreamProvider.Contents.First();
            ContentDispositionHeaderValue fileInfo = file.Headers.ContentDisposition;

            logger.LogInformation(JsonConvert.SerializeObject(fileInfo, Formatting.Indented));

            string sasToken = azureStorageBlobOptionsTokenGenerator.GenerateSasToken(azureStorageBlobOptionsTokenGenerator.FilePath);

            StorageCredentials storageCredentials = new StorageCredentials(sasToken);

            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, azureStorageBlobOptionsTokenGenerator.AccountName, null, true);

            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(azureStorageBlobOptionsTokenGenerator.FilePath);

            string blobName = $"{Guid.NewGuid()}{Path.GetExtension(fileInfo.FileName)}";

            blobName = blobName.Replace("\"", "");

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

            cloudBlockBlob.Properties.ContentType = file.Headers.ContentType.MediaType;

            await using (Stream fileStream = await file.ReadAsStreamAsync())
            {
                await cloudBlockBlob.UploadFromStreamAsync(fileStream);
            }

            return new OkObjectResult(new { name = blobName });
            */

            return new OkResult();


        }

    }
}