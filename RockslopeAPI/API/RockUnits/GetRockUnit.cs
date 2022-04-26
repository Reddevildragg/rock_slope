﻿using System;
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


namespace RockslopeAPI.RockUnits;

public static class GetRockUnit
{
    [FunctionName("GetRockUnit")]
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
                    rockUnit = db.Query("Rock_Units").Where(nameof(RockUnit.Id), index).First<RockUnit>();
                }
                else
                {
                    rockUnit = db.Query("Rock_Units").Where(nameof(RockUnit.RockUnitId), rockUnitId).First<RockUnit>();
                }
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