#region

using System;
using SqlKata;

#endregion

namespace RockslopeAPI.Models;


public class Project
{
    public const string TableName = "Projects";
    const string IdPrefix = "proj_";
    
    [Ignore]
    public int? Id { get; set; }
    
    public string ProjectId { get; set; } = "";

    public string ProjectName { get; set; }= "";

    public string Description { get; set; }= "";
    
    public string SiteLocation { get; set; }= "";

    public string CreatedBy { get; set; }= "";
    
    public string UpdatedBy { get; set; }= "";
    
    public DateTime CreatedAt { get; set; } = new DateTime(2000,1,1,0,0,0);
    
    public DateTime UpdatedAt { get; set; } = new DateTime(2000,1,1,0,0,0);

    public void SetProjectId(int index)
    {
        ProjectId = IdPrefix + index;
    }
}