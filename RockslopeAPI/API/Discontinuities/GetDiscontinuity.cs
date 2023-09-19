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


namespace RockslopeAPI.Discontinuitys;

public static class GetDiscontinuity
{
    [FunctionName("GetDiscontinuity")]
    [OpenApiOperation(Description = "Retrieve the rock Unit information for a rock Unit Id")]
    [OpenApiParameter("DiscontinuityId", In = ParameterLocation.Query, Required = true,Type = typeof(int))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Discontinuity))]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function,  "get", Route = "Discontinuities/{discontinuityId}")] HttpRequest req, string discontinuityId, ILogger log)
    {
        Discontinuity discontinuity = null;

        try
        {
            await using (SqlConnection connection =  new DatabaseConnector().Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());

                if (int.TryParse(discontinuityId, out int index))
                {
                    discontinuity = db.Query(Discontinuity.TableName).Where(nameof(discontinuity.Id), index).First<Discontinuity>();
                }
                else
                {
                    discontinuity = db.Query(Discontinuity.TableName).Where(nameof(discontinuity.DiscId), discontinuityId).First<Discontinuity>();
                }

                if (discontinuity.RockUnitId != null)
                {
                    discontinuity.RockUnit = db.Query(RockUnit.TableName).Where(nameof(RockUnit.Id), discontinuity.RockUnitId).First<RockUnit>();
                }

            }
                    
            return new JsonResult(discontinuity);
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