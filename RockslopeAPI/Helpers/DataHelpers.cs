using System.Data.SqlClient;
using RockslopeAPI.Models;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace RockslopeAPI.Helpers;

public static class DataHelpers
{
    public static bool ValidateAsset(string id)
    {
        using (SqlConnection connection = new DatabaseConnector().Connection())
        {
            QueryFactory db = new QueryFactory(connection, new SqlServerCompiler());

            if (id.StartsWith(Project.IdPrefix))
            {
                return db.Query(Project.TableName).Where(nameof(Project.ProjectId), id).Count<int>() == 1;
            }
            else if (id.StartsWith(RockSlope.IdPrefix))
            {
                return db.Query(RockSlope.TableName).Where(nameof(RockSlope.RockSlopeId), id).Count<int>() == 1;
            }
            else if (id.StartsWith(RockUnit.IdPrefix))
            {
                return db.Query(RockUnit.TableName).Where(nameof(RockUnit.RockSlopeId), id).Count<int>() == 1;
            }
            else if (id.StartsWith(Discontinuity.IdPrefix))
            {
                return db.Query(Discontinuity.TableName).Where(nameof(Discontinuity.DiscId), id).Count<int>() == 1;
            }
        }

        return false;
    }
}