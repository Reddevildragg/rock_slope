using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
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
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace RockslopeAPI.RockSlopes;

public static class CreateRockSlope
{
    [FunctionName("CreateRockSlope")]
    [OpenApiOperation(Description = "Create a Rockslope within the database")]
    [OpenApiRequestBody("application/json", typeof(RockSlope))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(RockSlope))]
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

                if (newRockSlope.Id == null)
                {
                    int rockSlopeIndex = await db.Query(RockSlope.TableName).InsertGetIdAsync<int>(newRockSlope);
                    newRockSlope.SetId(rockSlopeIndex);
                    await db.Query(RockSlope.TableName).Where("Id", rockSlopeIndex).UpdateAsync(newRockSlope);

                }
                else
                {
                    await db.Query(RockSlope.TableName).Where("Id", newRockSlope.Id).UpdateAsync(newRockSlope);
                }
                
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
