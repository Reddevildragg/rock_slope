#region

using System;

#endregion

namespace RockslopeAPI.Models;

public class Project
{
    public int Id { get; set; }


    public string ProjectId { get; set; } = "";


    public string ProjectName { get; set; }= "";


    public string Description { get; set; }= "";


    public string SiteLocation { get; set; }= "";


    public string CreatedBy { get; set; }= "";


    public string UpdatedBy { get; set; }= "";


    public DateTime CreatedAt { get; set; }


    public DateTime UpdatedAt { get; set; }
}