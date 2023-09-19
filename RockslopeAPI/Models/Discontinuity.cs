using System;
using SqlKata;

namespace RockslopeAPI.Models;

public class Discontinuity : BaseModel
{
    public const string TableName = "discontinuities";
    internal const string IdPrefix = "disc_";
    

    public string DiscId { get; set; } = "";

    public int? RockUnitId { get; set; }
    
    [Ignore]
    public RockUnit RockUnit { get; set; } = new RockUnit();
    public string Type { get; set; }= "";
    public string Spacing { get; set; }= "";
    public string Persistance { get; set; }= "";
    public string Termination { get; set; }= "";
    public string Roughness { get; set; }= "";
    public string Amplitude { get; set; }= "";
    public string WaveLength { get; set; }= "";
    public string Jrc { get; set; }= "";
    public string Aperture { get; set; }= "";
    public string Infilling { get; set; }= "";
    public string DiscontinuityWeathering { get; set; }= "";
    public string WallStrength { get; set; }= "";
    public string SeePage { get; set; }= "";
    public string AdditionalNotes { get; set; }= "";
    public string StructureReference { get; set; }= "";
    public int Dip { get; set; } = 0;
    public int DipDirection { get; set; } = 0;
    
    public override void SetId(int index)
    {
        DiscId = IdPrefix + index;
    }
}