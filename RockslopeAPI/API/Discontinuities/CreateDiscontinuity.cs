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

namespace RockslopeAPI.Discontinuitys;

public static class CreateDiscontinuity
{
    [FunctionName("CreateDiscontinuity")]
    [OpenApiOperation(Description = "Create a Discontinuity within the database")]
    [OpenApiRequestBody("application/json", typeof(Discontinuity))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Discontinuity))]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Discontinuities")] HttpRequest req, ILogger log)
    {
        try
        {
            await using (SqlConnection connection = new DatabaseConnector().Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());
                Discontinuity newDiscontinuity = new Discontinuity();

                if (req.HasFormContentType)
                {
                    newDiscontinuity = (await req.ReadFormAsync()).ToClass<Discontinuity>();
                }
                else if (req.Body != null)
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    newDiscontinuity = JsonConvert.DeserializeObject<Discontinuity>(requestBody);
                }
                
                if (newDiscontinuity.Id == null)
                {

                        int discontinuityIndex = await db.Query(Discontinuity.TableName).InsertGetIdAsync<int>(newDiscontinuity);
                        newDiscontinuity.SetId(discontinuityIndex);
                        await db.Query(Discontinuity.TableName).Where(nameof(Discontinuity.Id), discontinuityIndex).UpdateAsync(newDiscontinuity);
                }
                else
                {
                    await db.Query(Discontinuity.TableName).Where(nameof(Discontinuity.Id), newDiscontinuity.Id).UpdateAsync(newDiscontinuity);
                }
                
                return new JsonResult(newDiscontinuity);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
