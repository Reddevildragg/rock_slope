using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using SqlKata.Extensions;


namespace RockslopeAPI.RockSlopes;

public static class GetRockSlope
{
    [FunctionName("GetRockSlope")]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function,  "get", Route = "RockSlopes/{rockSlopeId}")] HttpRequest req, string rockSlopeId, ILogger log)
    {
        try
        {
            RockSlope rockSlope = null;
            await using (SqlConnection connection =  new DatabaseConnector().Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());

                if (int.TryParse(rockSlopeId, out int index))
                { 
                    rockSlope = db.Query("Rock_Slopes").Where("Rock_Slopes.Id", index).First<RockSlope>();
                    
                    RockSlope rs = db.Query("Rock_Slopes").Where("Rock_Slopes.Id", index).Include("Project",db.Query("Projects")).First();

                }
                else
                {
                    rockSlope = db.Query("Rock_Slopes").Where("Rock_Slopes.Id", rockSlopeId).First<RockSlope>();
                }

                rockSlope.Project = db.Query("Projects").Where(nameof(Project.Id), rockSlope.ProjectId).First<Project>();
            }
                    
            return new JsonResult(rockSlope);
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