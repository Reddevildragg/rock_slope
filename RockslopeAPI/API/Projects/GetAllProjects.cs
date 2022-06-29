using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
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
using SqlKata.Compilers;
using SqlKata.Execution;

namespace RockslopeAPI.Projects;

public static class GetAllProjects
{
    [FunctionName("GetAllProjects")]
    [OpenApiOperation(Description = "Retrieve all projects")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<Project>))]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Projects")] HttpRequest req, ILogger log)
    {
        try
        {
            IEnumerable<Project> projects;
            await using (SqlConnection connection = new DatabaseConnector().Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());
                projects =db.Query(Project.TableName).Get<Project>();
            }
            return new JsonResult(projects);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new InternalServerErrorResult();
        }
    }
}