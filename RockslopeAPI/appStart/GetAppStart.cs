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

public static class GetAppStart
{
    [FunctionName("GetAppStart")]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function,  "get", Route = "GetAppStart/{userId}")] HttpRequest req, string userId, ILogger log)
    {
        
        DatabaseConnector dbConnector = new DatabaseConnector();
        AppStart user;
        await using (SqlConnection connection = dbConnector.Connection())
        {
            const string query = $"SELECT id, appid FROM [dbo].[appStart] WHERE id LIKE @UserID";

            await using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                connection.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                user = reader.DataReaderMapToItem<AppStart>();
            }
        }
        return new JsonResult(user);
    }
}