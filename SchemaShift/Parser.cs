namespace SchemaShift;

internal class Parser(Scanner scanner)
{
    public MappingDefinition Parse()
    {
        var tokens = scanner.Scan();
        foreach (var token in tokens)
        {

        }

        return new MappingDefinition
        {
            Meta = [],
        };
    }
}
