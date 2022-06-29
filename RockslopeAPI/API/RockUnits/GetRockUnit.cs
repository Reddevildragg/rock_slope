using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RockslopeAPI.Helpers;
using RockslopeAPI.Models;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using SqlKata.Extensions;


namespace RockslopeAPI.RockUnits;

public static class GetRockUnit
{
    [FunctionName("GetRockUnit")]
    [OpenApiOperation(Description = "Retrieve the rock Unit information for a rock Unit Id")]
    [OpenApiParameter("rockUnitId", In = ParameterLocation.Query, Required = true,Type = typeof(int))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(RockUnit))]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function,  "get", Route = "RockUnits/{rockUnitId}")] HttpRequest req, string rockUnitId, ILogger log)
    {
        RockUnit rockUnit = null;

        try
        {
            await using (SqlConnection connection =  new DatabaseConnector().Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());

                if (int.TryParse(rockUnitId, out int index))
                {
                    rockUnit = db.Query(RockUnit.TableName).Where(nameof(RockUnit.Id), index).First<RockUnit>();
                }
                else
                {
                    rockUnit = db.Query(RockUnit.TableName).Where(nameof(RockUnit.RockUnitId), rockUnitId).First<RockUnit>();
                }
                
                rockUnit.RockSlope = db.Query(RockSlope.TableName).Where(nameof(RockSlope.Id), rockUnit.RockSlopeId).First<RockSlope>();

            }
                    
            return new JsonResult(rockUnit);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            if (e.GetType() == typeof(InvalidOperationException))
            {
                return new NotFoundResult();
            }

            return new InternalServerErrorResult();
        }
    }
}