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

namespace RockslopeAPI.RockSlopes;

public static class CreateRockSlope
{
    [FunctionName("CreateRockSlope")]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "RockSlopes")] HttpRequest req, ILogger log)
    {
        try
        {
            await using (SqlConnection connection = new DatabaseConnector().Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());
                RockSlope newRockSlope = new RockSlope();

                if (req.HasFormContentType)
                {
                    newRockSlope = (await req.ReadFormAsync()).ToClass<RockSlope>();
                }
                else if (req.Body != null)
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    newRockSlope = JsonConvert.DeserializeObject<RockSlope>(requestBody);
                }

                int rockSlopeIndex = await db.Query("Rock_Slopes").InsertGetIdAsync<int>(newRockSlope);
                newRockSlope.SetRockSlopeId(rockSlopeIndex);
                
                await db.Query("Rock_Slopes").Where("Id", rockSlopeIndex).UpdateAsync(newRockSlope);

                return new JsonResult(newRockSlope);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
