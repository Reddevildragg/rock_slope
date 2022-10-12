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
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RockslopeAPI.Helpers;
using RockslopeAPI.Models;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace RockslopeAPI.Projects;

public static class CreateProject
{
    //https://devkimchi.com/2019/02/02/introducing-swagger-ui-on-azure-functions/
    [FunctionName("CreateProject")]
    [OpenApiOperation(Description = "Create a project within the database")]
    [OpenApiRequestBody("application/json", typeof(Project))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Project))]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Projects")] HttpRequest req, ILogger log)
    {
        try
        {
            await using (SqlConnection connection = new DatabaseConnector().Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());
                Project newProject = new Project();

                if (req.HasFormContentType)
                {
                    newProject = (await req.ReadFormAsync()).ToClass<Project>();
                }
                else if (req.Body != null)
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    newProject = JsonConvert.DeserializeObject<Project>(requestBody);
                }

                if (newProject.Id == null)
                {
                    int projectIndex = await db.Query(Project.TableName).InsertGetIdAsync<int>(newProject);
                    newProject.SetId(projectIndex);
                    await db.Query(Project.TableName).Where(nameof(Project.Id), projectIndex).UpdateAsync(newProject);
                }
                else
                {
                    await db.Query(Project.TableName).Where(nameof(Project.Id), newProject.Id).UpdateAsync(newProject);
                }

                return new JsonResult(newProject);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
