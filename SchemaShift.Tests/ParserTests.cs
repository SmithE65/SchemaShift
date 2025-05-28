namespace SchemaShift.Tests;

public class ParserTests
{
    [Fact]
    public void Parse_BasicMapping()
    {
        // Arrange
        var scanner = new Scanner(Data.SimpleMapping.Mapping);
        var parser = new Parser(scanner);

        // Act
        var result = parser.Parse();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Mappings);
        Assert.Equal("Test", result.Meta["name"]);
        Assert.Equal("1", result.Meta["version"]);
        Assert.Single(result.Mappings);
        var mapping = result.Mappings[0];
        var propertyMapping = Assert.Single(mapping.PropertyMappings);
        var target = Assert.Single(propertyMapping.TargetProperties);
        Assert.Equal("dest", target);
    }
}
