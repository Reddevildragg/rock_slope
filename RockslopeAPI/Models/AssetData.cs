namespace RockslopeAPI.Models;

public class AssetData : BaseModel
{
    public const string TableName = "assets";
    public string AssetId { get; set; } = "";
    public string AssociatedItem { get; set; } = "";
        
    public override void SetId(int index)
    {
        //ignore
    }
}