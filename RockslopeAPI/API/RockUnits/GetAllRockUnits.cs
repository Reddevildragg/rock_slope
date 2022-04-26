using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
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

namespace RockslopeAPI.RockUnits;

public static class GetAllRockUnits
{
    [FunctionName("GetAllRockUnits")]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "RockUnits")] HttpRequest req, ILogger log)
    {
        try
        {
            DatabaseConnector dbConnector = new DatabaseConnector();
            IEnumerable<RockUnit> rockUnits;
            await using (SqlConnection connection = dbConnector.Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());
                rockUnits =db.Query(RockUnit.TableName).Get<RockUnit>();
                
                foreach (RockUnit rockUnit in rockUnits)
                {
                    rockUnit.RockSlope = db.Query(RockSlope.TableName).Where(nameof(RockSlope.Id), rockUnit.RockSlopeId).First<RockSlope>();
                }
            }
            return new JsonResult(rockUnits);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new InternalServerErrorResult();
        }
    }
}