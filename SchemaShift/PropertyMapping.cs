namespace SchemaShift;

public class PropertyMapping
{
    public required string[] TargetProperties { get; init; }
    public required Expression Expression { get; init; }
}
