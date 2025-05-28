namespace SchemaShift.Tests.Data;

public static class SimpleMapping
{
    public const string Mapping = """
        meta {
            name: "Test"
            version: "1"
        }

        mapping output {
            map dest from source
        }
        """;
}
