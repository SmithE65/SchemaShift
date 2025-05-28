
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SchemaShift;

public class MapResult
{
    public required Dictionary<string, string> Meta { get; init; }
    public required MappedDocument[] Documents { get; init; }
}

public class MappedDocument
{
    public required Dictionary<string, string> Meta { get; init; }
    public required string Name { get; init; }
    public required string Document { get; init; }
}

public class Interpreter
{
    private readonly MapperDefinition _mapping;

    public static Interpreter Compile(string source)
    {
        Scanner scanner = new Scanner(source);
        var parser = new Parser(scanner);
        var mapping = parser.Parse();
        return new(mapping);
    }

    private Interpreter(MapperDefinition mapping)
    {
        _mapping = mapping;
    }

    public MapResult Interpret(string json)
    {
        using var jsonDoc = JsonDocument.Parse(json);
        var documents = new List<MappedDocument>();

        foreach (var mapping in _mapping.Mappings)
        {
            var mapped = Interpret(mapping, jsonDoc.RootElement);

            var document = new MappedDocument
            {
                Meta = _mapping.Meta,
                Name = mapping.Name,
                Document = mapped
            };

            documents.Add(document);
        }

        return new MapResult
        {
            Meta = _mapping.Meta,
            Documents = [.. documents]
        };
    }

    private string Interpret(MappingDefinition mapping, JsonElement json)
    {
        var result = new JsonObject();

        foreach (var propertyMapping in mapping.PropertyMappings)
        {
            var value = propertyMapping.Expression switch
            {
                ConstantExpression constant => constant.Value,
                GetSourcePropertyValueExpression valueExpress => GetSourcePropertyValue(valueExpress, json),
                _ => throw new NotSupportedException($"Unsupported expression type: {propertyMapping.Expression.GetType().Name}")
            };

            foreach (var targetProperty in propertyMapping.TargetProperties)
            {
                result[targetProperty] = value;
            }
        }
        return result.ToJsonString();
    }

    private JsonNode? GetSourcePropertyValue(GetSourcePropertyValueExpression valueExpress, JsonElement json)
    {
        return json.TryGetProperty(valueExpress.SourcePropertyName, out var property)
            ? JsonNode.Parse(property.GetRawText())
            : null; // Return null if property not found
    }
}

internal class ConstantExpression : Expression
{
    public string? Value { get; set; }
}