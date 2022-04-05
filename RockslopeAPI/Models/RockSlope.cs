using System;

namespace RockslopeAPI.Models;
public class RockSlope
{

    public int Id { get; set; }


    public string RockslopeId { get; set; }


    public int? ProjectId { get; set; }


    public string SlopeReference { get; set; }


    public int? StartChainage { get; set; }


    public int? EndChainage { get; set; }


    public double? Latitude { get; set; }


    public double? Longitude { get; set; }


    public int? ElevationToa { get; set; }


    public int? FaceHeight { get; set; }


    public int? FaceAngle { get; set; }


    public int? FaceAzimuth { get; set; }


    public int? Length { get; set; }


    public int? Vegetation { get; set; }


    public int? BenchDetails { get; set; }


    public int? RoughnessProfile { get; set; }


    public int? ToeDetails { get; set; }


    public int? InfrastructureToe { get; set; }


    public int? InfrastructureCrest { get; set; }


    public int? UpperFace { get; set; }


    public string CreatedBy { get; set; }


    public string UpdatedBy { get; set; }


    public DateTime? CreatedAt { get; set; }


    public DateTime? UpdatedAt { get; set; }

}
