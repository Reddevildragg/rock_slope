using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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

namespace RockslopeAPI.RockUnits;

public static class CreateRockUnit
{
    [FunctionName("CreateRockUnit")]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "RockUnits")] HttpRequest req, ILogger log)
    {
        try
        {
            await using (SqlConnection connection = new DatabaseConnector().Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());
                RockUnit newRockUnit = new RockUnit();

                if (req.HasFormContentType)
                {
                    newRockUnit = (await req.ReadFormAsync()).ToClass<RockUnit>();
                }
                else if (req.Body != null)
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    newRockUnit = JsonConvert.DeserializeObject<RockUnit>(requestBody);
                }

                int rockUnitIndex = await db.Query("Rock_Units").InsertGetIdAsync<int>(newRockUnit);
                newRockUnit.SetRockUnitId(rockUnitIndex);
                await db.Query("Rock_Units").Where("Id", rockUnitIndex).UpdateAsync(newRockUnit);

                return new JsonResult(newRockUnit);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
