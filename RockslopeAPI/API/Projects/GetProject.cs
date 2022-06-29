using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using SqlKata.Extensions;


namespace RockslopeAPI.Projects;

public static class GetProject
{
    [FunctionName("GetProject")]
    [OpenApiOperation(Description = "Retrieve the project information for a project Id")]
    [OpenApiParameter(name: "projectId", Required = true, Type = typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Project))]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function,  "get", Route = "Projects/{projectId}")] HttpRequest req, string projectId, ILogger log)
    {
        Project project = null;

        try
        {
            await using (SqlConnection connection =  new DatabaseConnector().Connection())
            {
                QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());

                if (int.TryParse(projectId, out int index))
                {
                    project = db.Query(Project.TableName).Where(nameof(Project.Id), index).First<Project>();
                }
                else
                {
                    project = db.Query(Project.TableName).Where(nameof(Project.ProjectId), projectId).First<Project>();
                }
            }
                    
            return new JsonResult(project);
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