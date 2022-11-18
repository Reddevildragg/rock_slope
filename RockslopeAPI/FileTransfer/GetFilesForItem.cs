using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
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
    public static class GetFilesForItems
    {
        [FunctionName("GetAssetFiles")]
        [OpenApiOperation(Description = "Get all download ids for files associated with an asset")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(AssetData))]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "files/{AssociatedItem}")] HttpRequest req,string AssociatedItem,
                                                         ILogger logger)
        {
            try
            {
                IEnumerable<AssetData> assets;
                if (!DataHelpers.ValidateAsset(AssociatedItem))
                {
                    return new BadRequestObjectResult("Unable to find Item in database");
                }
                
                await using (SqlConnection connection = new DatabaseConnector().Connection())
                {
                    QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());
                    assets = db.Query("assets").Where(nameof(AssetData.AssociatedItem), AssociatedItem).Get<AssetData>();
                }
                return new JsonResult(assets);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new InternalServerErrorResult();
            }
        }
    }
}