namespace SchemaShift.Tests;

public class ParserTests
{
    [Fact]
    public void Test1()
    {
        Assert.Fail("Not implemented");
        var scanner = new Scanner(Data.SimpleMapping.Mapping);
        var parser = new Parser(scanner);
        var result = parser.Parse();
    }
}
