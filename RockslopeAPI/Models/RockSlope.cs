using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using SqlKata;

namespace RockslopeAPI.Models;
public class RockSlope : Dictionary<string,object>, IDynamicMetaObjectProvider
{
    const string IdPrefix = "slpe_";

    [Ignore]
    public int? Id { get; set; }


    public string RockSlopeId { get; set; }


    public int ProjectId { get; set; }

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


    public int Vegetation { get; set; }


    public int BenchDetails { get; set; }


    public int RoughnessProfile { get; set; }


    public int ToeDetails { get; set; }


    public int InfrastructureToe { get; set; }


    public int InfrastructureCrest { get; set; }


    public int UpperFace { get; set; }


    public string CreatedBy { get; set; }


    public string UpdatedBy { get; set; }


    public DateTime? CreatedAt { get; set; }= new DateTime(2000,1,1,0,0,0);


    public DateTime? UpdatedAt { get; set; }= new DateTime(2000,1,1,0,0,0);

    public void SetRockSlopeId(int index)
    {
        RockSlopeId = IdPrefix + index;
    }

    public DynamicMetaObject GetMetaObject(Expression parameter)
    {
        throw new NotImplementedException();
    }
}
