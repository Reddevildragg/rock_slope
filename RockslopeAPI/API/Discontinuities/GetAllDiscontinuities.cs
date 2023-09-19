using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RockslopeAPI.Helpers;
using RockslopeAPI.Models;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace RockslopeAPI.Discontinuitys;

public static class GetAllDiscontinuities
{
    [FunctionName("GetAllDiscontinuities")]
    [OpenApiOperation(Description = "Retrieve all Rock Units")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<Discontinuity>))]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Discontinuities")] HttpRequest req, ILogger log)
    {
        try
        {
            DatabaseConnector dbConnector = new DatabaseConnector();
            IEnumerable<Discontinuity> discontinuities;
            await using (SqlConnection connection = dbConnector.Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());
                discontinuities =db.Query(Discontinuity.TableName).Get<Discontinuity>();
                
                foreach (Discontinuity discontinuity in discontinuities)
                {
                    if (discontinuity.RockUnitId != null)
                    {
                    
                        discontinuity.RockUnit = db.Query(RockUnit.TableName).Where(nameof(RockUnit.Id), discontinuity.RockUnitId).First<RockUnit>();
                    }
                }
            }
            return new JsonResult(discontinuities);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new InternalServerErrorResult();
        }
    }
}