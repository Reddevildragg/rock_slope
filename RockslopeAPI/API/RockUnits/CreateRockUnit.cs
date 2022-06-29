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

namespace RockslopeAPI.RockUnits;

public static class CreateRockUnit
{
    [FunctionName("CreateRockUnit")]
    [OpenApiOperation(Description = "Create a Rock Unit within the database")]
    [OpenApiRequestBody("application/json", typeof(RockUnit))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(RockUnit))]
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

                int rockUnitIndex = await db.Query(RockUnit.TableName).InsertGetIdAsync<int>(newRockUnit);
                newRockUnit.SetRockUnitId(rockUnitIndex);
                await db.Query(RockUnit.TableName).Where("Id", rockUnitIndex).UpdateAsync(newRockUnit);

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
