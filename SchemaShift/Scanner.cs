namespace SchemaShift;

internal class Scanner(string source)
{
    private static readonly Dictionary<string, TokenType> _keywords = new()
    {
        { "meta", TokenType.Meta },
        { "mapping", TokenType.Mapping },
        { "map", TokenType.Map },
        { "from", TokenType.From }
    };

    private readonly string _source = source;
    private readonly List<Token> _tokens = [];
    private int _start = 0;
    private int _current = 0;
    private int _line = 0;

    public IEnumerable<Token> Scan()
    {
        while (!IsEndOfFile())
        {
            _start = _current;
            ScanNextToken();
        }

        _tokens.Add(new Token(TokenType.EOF, _source, _source.Length, 0, _line));
        return _tokens;
    }

    private bool IsEndOfFile() => _current >= _source.Length;

    private void ScanNextToken()
    {
        var c = Advance();

        switch (c)
        {
            case '"':
                ScanString();
                break;
            case '{':
                AddToken(TokenType.LeftBracket);
                break;
            case '}':
                AddToken(TokenType.RightBracket);
                break;
            case ':':
                AddToken(TokenType.Colon);
                break;
            case '\n':
                _line++;
                break;
            case ' ':
            case '\t':
            case '\r':
                break;
            default:
                if (char.IsLetter(c))
                {
                    ScanIdentifier();
                    break;
                }
                throw new InvalidOperationException($"Unexpected character '{c}'");
        }
    }

    private char Advance() => _source[_current++];

    private char Peek() => IsEndOfFile() ? '\0' : _source[_current];

    private void AddToken(TokenType tokenType)
    {
        _tokens.Add(new Token(tokenType, _source, _start, _current - _start, _line));
    }

    private void ScanIdentifier()
    {
        while (char.IsLetter(Peek()) && !IsEndOfFile())
        {
            Advance();
        }

        if (_keywords.TryGetValue(_source.AsSpan()[_start.._current].ToString(), out var tokenType))
        {
            AddToken(tokenType);
        }
        else
        {
            AddToken(TokenType.Identifier);
        }
    }

    private void ScanString()
    {
        while (Peek() != '"' && !IsEndOfFile())
        {
            if (Peek() == '\n')
            {
                throw new Exception("Strings can't span multiple lines.");
            }

            Advance();
        }

        if (IsEndOfFile())
        {
            throw new Exception("Unterminated string.");
        }

        // Advance past quote
        Advance();
        AddToken(TokenType.StringLiteral);
    }
}
