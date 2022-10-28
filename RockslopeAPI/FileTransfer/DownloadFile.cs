﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
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
using RockslopeAPI.Helpers;

namespace RockslopeAPI.FileTransfer;

public static class DownloadFile
{
    
    [FunctionName("FileDownloadHttpTrigger")]
    [OpenApiOperation(Description = "Download file from its guid")]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "files/download/{fileName}")] HttpRequest req, string fileName, ILogger logger)
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

        return new FileContentResult(ms.ToArray(), cloudBlockBlob.Properties.ContentType);
    }

}