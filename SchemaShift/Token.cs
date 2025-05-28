namespace SchemaShift;

internal class Token(TokenType tokenType, string source, int start, int length, int lineNumber)
{
    public TokenType TokenType { get; } = tokenType;
    public int Start { get; } = start;
    public int Length { get; } = length;
    public int LineNumber { get; } = lineNumber;

    public string GetLexeme()
    {
        return TokenType == TokenType.StringLiteral
            ? source.Substring(Start + 1, Length - 2) // Exclude quotes for string literals
            : source.Substring(Start, Length);
    }
}
