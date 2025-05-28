using System.Text.Json;

namespace SchemaShift.Tests;

public class InterpreterTests
{
    [Fact]
    public void Compile_ValidSource_ReturnsInterpreter()
    {
        // Arrange
        var mapping = Data.SimpleMapping.Mapping;
        var interpreter = Interpreter.Compile(mapping);
        var source = """
            {
                "source": "value",
                "unused": "should not affect result"
            }
            """;

        // Act
        var result = interpreter.Interpret(source);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Meta);
        Assert.Equal("Test", result.Meta["name"]);
        Assert.Equal("1", result.Meta["version"]);

        var document = Assert.Single(result.Documents);
        Assert.Equal("output", document.Name);
        var jsonDoc = JsonDocument.Parse(document.Document);
        Assert.True(jsonDoc.RootElement.TryGetProperty("dest", out var destProperty));
        Assert.Equal("value", destProperty.GetString());
        Assert.False(jsonDoc.RootElement.TryGetProperty("source", out _),
            "Source property should not be present in the output document.");
        Assert.False(jsonDoc.RootElement.TryGetProperty("unused", out _),
            "Unused property should not be present in the output document.");
    }
}
