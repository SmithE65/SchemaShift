namespace SchemaShift;

public class MapperDefinition
{
    public Dictionary<string, string> Meta { get; init; } = [];
    public MappingDefinition[] Mappings { get; init; } = [];
}
