#region

using System;
using SqlKata;

#endregion

namespace RockslopeAPI.Models;


public class Project : BaseModel
{
    public const string TableName = "Projects";
    const string IdPrefix = "proj_";
    
    public string ProjectId { get; set; } = "";

    public string ProjectName { get; set; }= "";

    public string Description { get; set; }= "";
    
    public string SiteLocation { get; set; }= "";
    public override void SetId(int index)
    {
        ProjectId = IdPrefix + index;
    }
}