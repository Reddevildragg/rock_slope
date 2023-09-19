using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using SqlKata;

namespace RockslopeAPI.Models;
public class RockSlope : BaseModel
{
    public const string TableName = "Rock_Slopes";
    internal const string IdPrefix = "slpe_";
    
    public string RockSlopeId { get; set; }


    public int ProjectId { get; set; }

    [Ignore]
    public Project Project { get; set; } = new Project();

    public string SlopeReference { get; set; }


    public int StartChainage { get; set; }


    public int EndChainage { get; set; }


    public double Latitude { get; set; }


    public double Longitude { get; set; }


    public int ElevationToa { get; set; }


    public int FaceHeight { get; set; }


    public int FaceAngle { get; set; }


    public int FaceAzimuth { get; set; }


    public int Length { get; set; }


    public string Vegetation { get; set; }


    public string BenchDetails { get; set; }


    public string RoughnessProfile { get; set; }


    public string ToeDetails { get; set; }


    public string InfrastructureToe { get; set; }


    public int InfrastructureCrest { get; set; }


    public string UpperFace { get; set; }
    
    public override void SetId(int index)
    {
        RockSlopeId = IdPrefix + index;
    }
}
