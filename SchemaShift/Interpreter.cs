namespace SchemaShift;

public class Interpreter
{
    private readonly MappingDefinition _mapping;

    public static Interpreter Compile(string source)
    {
        var scanner = new Scanner(source);
        var parser = new Parser(scanner);
        var mapping = parser.Parse();
        return new(mapping);
    }

    private Interpreter(MappingDefinition mapping)
    {
        _mapping = mapping;
    }
}
