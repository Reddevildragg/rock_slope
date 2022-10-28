using System;
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
        Helpers.FileTransfer.DownloadFileData data = await Helpers.FileTransfer.DownloadFile(fileName, logger);
        return new FileContentResult(data.bytes, data.fileContentType);
    }

}