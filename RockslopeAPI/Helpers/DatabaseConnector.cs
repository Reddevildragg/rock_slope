using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace RockslopeAPI.Helpers;

public class DatabaseConnector
{
    
    public string DataSource { get; set; }
    public string UserId { get; set; }
    public string Password { get; set; }
    public string InitialCatalog { get; set; }
    public SqlConnectionStringBuilder builder { get; private set; }

    public DatabaseConnector()
    {
        this.DataSource = Environment.GetEnvironmentVariable($"database_source");
        this.UserId = Environment.GetEnvironmentVariable($"database_username");
        this.Password = Environment.GetEnvironmentVariable($"database_password");
        this.InitialCatalog = Environment.GetEnvironmentVariable($"database_initial_catalog");

        var x = Environment.GetEnvironmentVariable($"Waffles");
        builder = new SqlConnectionStringBuilder
        {
            DataSource = DataSource,
            UserID = UserId,
            Password = Password,
            InitialCatalog = InitialCatalog
        };
    }
    

    public SqlConnection Connection()
    {
        return new SqlConnection(builder.ConnectionString);
    }
}