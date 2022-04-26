using System;
using SqlKata;

namespace RockslopeAPI.Models;

public class RockUnit
{
    public const string TableName = "Rock_Units";
    const string IdPrefix = "unit_";

    [Ignore]
    public int? Id { get; set; }

    public string RockUnitId { get; set; }

    public int RockSlopeId { get; set; }
    [Ignore]
    public RockSlope RockSlope { get; set; } = new RockSlope();

    public string StructureReference { get; set; }

    public string RockUnitRef { get; set; }

    public string Strength { get; set; }

    public string Thickness { get; set; }

    public string Spacing { get; set; }
    
    public string Term { get; set; }

    public string Lightness { get; set; }

    public string Chroma { get; set; }

    public string Hue { get; set; }

    public string Texture { get; set; }
    
    public string GrainSize { get; set; }

    public string RockName { get; set; }

    public string MinorConstituents { get; set; }

    public string GeologicalFormation { get; set; }
    
    public string WeatheringDescription { get; set; }
    
    public string AdditionalNotes { get; set; }

    public string WeatherGrades { get; set; }

    public string CreatedBy { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }= new DateTime(2000,1,1,0,0,0);

    public DateTime? UpdatedAt { get; set; }= new DateTime(2000,1,1,0,0,0);

    public void SetRockUnitId(int index)
    {
        RockUnitId = IdPrefix + index;
    }
}
