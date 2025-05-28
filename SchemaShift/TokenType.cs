namespace SchemaShift;

internal enum TokenType
{
    EOF,
    Meta,
    Mapping,
    Map,
    From,
    Identifier,
    Literal,
    StringLiteral,
    NumberLiteral,
    BooleanLiteral,
    NullLiteral,
    LeftBracket,
    RightBracket,
    Colon
}
