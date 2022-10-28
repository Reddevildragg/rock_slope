using System;
using SqlKata;

namespace RockslopeAPI.Models;

public abstract class BaseModel
{
    [Ignore]
    public int? Id { get; set; }
    
    public string CreatedBy { get; set; }= "";
    public string UpdatedBy { get; set; }= "";
    public DateTime CreatedAt { get; set; } = new DateTime(2000,1,1,0,0,0);
    public DateTime UpdatedAt { get; set; } = new DateTime(2000,1,1,0,0,0);

    public abstract void SetId(int index);
}