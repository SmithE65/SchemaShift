namespace SchemaShift;

public class MappingDefinition
{
    public Dictionary<string, string> Meta { get; init; } = [];
    public DocumentMapping[] DocumentMappings { get; init; } = [];
}

public class DocumentMapping
{
    public required string Name { get; init; }
    public Expression? Condition { get; init; }
    public required List<PropertyMapping> PropertyMappings { get; init; }
}

public class PropertyMapping
{
    public required string[] TargetProperties { get; init; }
    public required Expression Expression { get; init; }
}

public class Expression
{

}