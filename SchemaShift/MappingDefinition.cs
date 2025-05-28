namespace SchemaShift;

public class MappingDefinition
{
    public required string Name { get; init; }
    public Dictionary<string, string> Meta { get; init; } = [];
    public Expression? Condition { get; init; }
    public required List<PropertyMapping> PropertyMappings { get; init; }
}
