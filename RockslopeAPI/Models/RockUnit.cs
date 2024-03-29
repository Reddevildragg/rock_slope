using System;
using SqlKata;

namespace RockslopeAPI.Models;

public class RockUnit : BaseModel
{
    public const string TableName = "Rock_Units";
    internal const string IdPrefix = "unit_";
    
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
    
    public override void SetId(int index)
    {
        RockUnitId = IdPrefix + index;
    }
}
