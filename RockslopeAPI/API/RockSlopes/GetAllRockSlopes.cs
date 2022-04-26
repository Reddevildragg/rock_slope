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

namespace RockslopeAPI.RockSlopes;

public static class GetAllRockSlopes
{
    [FunctionName("GetAllRockSlopes")]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "RockSlopes")] HttpRequest req, ILogger log)
    {
        try
        {
            DatabaseConnector dbConnector = new DatabaseConnector();
            IEnumerable<RockSlope> rockSlopes;
            await using (SqlConnection connection = dbConnector.Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());
                rockSlopes =db.Query("Rock_Slopes").Get<RockSlope>();
            }
            return new JsonResult(rockSlopes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new InternalServerErrorResult();
        }
    }
}