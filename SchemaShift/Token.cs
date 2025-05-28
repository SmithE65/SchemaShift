namespace SchemaShift;

internal class Token(TokenType tokenType, int start, int length, int lineNumber)
{
    public TokenType TokenType { get; } = tokenType;
    public int Start { get; } = start;
    public int Length { get; } = length;
    public int LineNumber { get; } = lineNumber;
}
