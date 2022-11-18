using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using RockslopeAPI.Helpers;
using RockslopeAPI.Models;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace RockslopeAPI.FileTransfer
{
    class UploadData
    {
        public string ItemId { get; set; }
    }
    
    public static class UploadFile
    {
        [FunctionName("FileUploadHttpTrigger")]
        [OpenApiOperation(Description = "Upload file to storage and return a unique GUID for the file")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(AssetData))]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "files/upload")] HttpRequest req,
                                                         ILogger logger)
        {
            logger.LogInformation("trigger function processed a request.");
            
            IFormCollection x = await req.ReadFormAsync();
            IFormFile fileInfo = x.Files[0];
            UploadData data = x.ToClass<UploadData>();

            if (!DataHelpers.ValidateAsset(data.ItemId))
            {
                return new BadRequestObjectResult("Unable to find Item in database");
            }
            
            string blobName = await Helpers.FileTransfer.UploadFile(fileInfo, logger);
            
            AssetData ad = new AssetData();
            await using (SqlConnection connection = new DatabaseConnector().Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());

                ad.AssetId = blobName;
                ad.AssociatedItem =  data.ItemId;
                
                int id = await db.Query(AssetData.TableName).InsertGetIdAsync<int>(ad);
                ad.SetId(id);
                await db.Query("assets").Where("Id", id).UpdateAsync(ad);
            }

            return new OkObjectResult(ad);
        }
    }
}