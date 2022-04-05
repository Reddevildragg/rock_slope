using System;
using System.Data.SqlClient;
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

namespace RockslopeAPI.appStart;

public static class GetProject
{
    [FunctionName("GetProject")]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function,  "get", Route = "Projects/{projectId}")] HttpRequest req, string projectId, ILogger log)
    {
        DatabaseConnector dbConnector = new DatabaseConnector();
        Project user;
        await using (SqlConnection connection = dbConnector.Connection())
        {
            const string query = $"SELECT * FROM projects WHERE id = @ProjectId";

            await using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@ProjectId", projectId);
                connection.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                user = reader.DataReaderMapToItem<Project>();
            }
        }
        return new JsonResult(user);
    }
}