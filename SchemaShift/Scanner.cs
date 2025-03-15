namespace SchemaShift;

internal enum TokenType
{
    // Top level tokens
    Meta, Mapping, Expand, Function, Lookup,

    // Single character tokens
    LeftBrace, RightBrace, LeftParen, RightParen,
    Colon, Comma, Dot, Minus, Plus,
    Slash, Star, LogicalNot,

    // Two character tokens
    LogicalAnd, LogicalOr,

    // Comparison
    EqualEqual, NotEqual, GreaterThan, LessThan,
    GreaterThanOrEqual, LessThanOrEqual,

    // Common tokens
    Identifier, String, Number, Boolean, Null,

    // Mapping tokens
    Map, To, From, Condition,
    With, Select, At, Step, Flatten,

    // Whitespace
    NewLine, EOF
}

internal class Token(string source, int start, int length, int line)
{
    private readonly string _source = source;
    private readonly int _start = start;
    private readonly int _length = length;
    private readonly int _line = line;
}

internal class Scanner(string source) : IEnumerable<Token>, IEnumerator<Token>
{
    private readonly string _source = source;

    public IEnumerable<Token> Scan()
    {
        
    }
}
