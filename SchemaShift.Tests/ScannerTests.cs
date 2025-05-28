namespace SchemaShift.Tests;

public class ScannerTests
{
    [Fact]
    public void Scan_EmptyString_EOF()
    {
        var source = "";
        var scanner = new Scanner(source);
        var tokens = scanner.Scan();

        var token = Assert.Single(tokens);
        Assert.Equal(TokenType.EOF, token.TokenType);
    }

    [Fact]
    public void Scan_Meta()
    {
        var source = """
            meta {
              key: "value"
            }
            """;
        var scanner = new Scanner(source);
        var tokens = scanner.Scan().ToArray();

        Assert.Equal(7, tokens.Length);
        Assert.Equal(TokenType.Meta, tokens[0].TokenType);
        Assert.Equal(TokenType.LeftBracket, tokens[1].TokenType);
        Assert.Equal(TokenType.Identifier, tokens[2].TokenType);
        Assert.Equal(TokenType.Colon, tokens[3].TokenType);
        Assert.Equal(TokenType.Literal, tokens[4].TokenType);
        Assert.Equal(TokenType.RightBracket, tokens[5].TokenType);
        Assert.Equal(TokenType.EOF, tokens[6].TokenType);
    }

    [Fact]
    public void Scan_BasicMapping()
    {
        var source = """
            mapping name {
              map dest from source
            }
            """;

        var scanner = new Scanner(source);
        var tokens = scanner.Scan().ToArray();

        Assert.Equal(9, tokens.Length);
        Assert.Equal(TokenType.Mapping, tokens[0].TokenType);
        Assert.Equal(TokenType.Identifier, tokens[1].TokenType);
        Assert.Equal(TokenType.LeftBracket, tokens[2].TokenType);
        Assert.Equal(TokenType.Map, tokens[3].TokenType);
        Assert.Equal(TokenType.Identifier, tokens[4].TokenType);
        Assert.Equal(TokenType.From, tokens[5].TokenType);
        Assert.Equal(TokenType.Identifier, tokens[6].TokenType);
        Assert.Equal(TokenType.RightBracket, tokens[7].TokenType);
        Assert.Equal(TokenType.EOF, tokens[8].TokenType);
    }
}
