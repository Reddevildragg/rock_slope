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

namespace RockslopeAPI.Projects;

public static class CreateProject
{
    [FunctionName("CreateProject")]
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

                int projectIndex = await db.Query("projects").InsertGetIdAsync<int>(newProject);
                newProject.SetProjectId(projectIndex);
                await db.Query("projects").Where("Id", projectIndex).UpdateAsync(newProject);

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
