
namespace SchemaShift;

internal class TokenStream(Token[] tokens)
{
    private readonly Token[] _tokens = tokens;
    private int _position = 0;

    public Token Current => _position < _tokens.Length 
        ? _tokens[_position] 
        : new Token(TokenType.EOF, string.Empty, 0, 0, 0);

    public void Advance()
    {
        if (_position < _tokens.Length)
        {
            _position++;
        }
    }

    public bool IsEndOfFile() => _position >= _tokens.Length;
}

internal class Parser(Scanner scanner)
{
    private readonly Token[] _tokens = scanner.Scan().ToArray();

    public MapperDefinition Parse()
    {
        var stream = new TokenStream(_tokens);
        Dictionary<string, string>? meta = null;
        var mappings = new List<MappingDefinition>();

        while (!stream.IsEndOfFile())
        {
            var token = stream.Current;
            switch (token.TokenType)
            {
                case TokenType.Meta:
                    meta = ParseMeta(stream);
                    break;
                case TokenType.Mapping:
                    var mapping = ParseMapping(stream);
                    mappings.Add(mapping);
                    break;
                default:
                    stream.Advance();
                    break;
            }
        }

        return new MapperDefinition
        {
            Meta = meta ?? [],
            Mappings = [.. mappings]
        };
    }

    private MappingDefinition ParseMapping(TokenStream stream)
    {
        stream.Advance(); // Skip 'mapping' token

        if (stream.Current.TokenType != TokenType.Identifier)
        {
            throw new Exception("Expected mapping name after 'mapping'");
        }

        var name = stream.Current.GetLexeme();
        stream.Advance(); // Skip mapping name

        if (stream.Current.TokenType != TokenType.LeftBracket)
        {
            throw new Exception("Expected '{' after mapping name");
        }

        stream.Advance(); // Skip '{'
        Dictionary<string, string> meta = [];
        Expression? condition = null;
        var propertyMappings = new List<PropertyMapping>();

        while (stream.Current.TokenType != TokenType.RightBracket)
        {
            if (stream.Current.TokenType == TokenType.EOF)
            {
                throw new Exception("Unexpected end of file while parsing mapping");
            }

            switch (stream.Current.TokenType)
            {
                case TokenType.Meta:
                    meta = ParseMeta(stream) ?? [];
                    break;
                //case TokenType.Condition:
                //    condition = ParseCondition(stream);
                //    break;
                case TokenType.Map:
                    var propertyMapping = ParsePropertyMapping(stream);
                    propertyMappings.Add(propertyMapping);
                    break;
                default:
                    stream.Advance(); // Skip unknown token
                    break;
            }
        }

        if (stream.Current.TokenType != TokenType.RightBracket)
        {
            throw new Exception("Expected '}' to close mapping block");
        }

        stream.Advance(); // Skip '}'

        return new MappingDefinition
        {
            Name = name,
            Meta = meta,
            Condition = condition,
            PropertyMappings = [.. propertyMappings]
        };
    }

    private PropertyMapping ParsePropertyMapping(TokenStream stream)
    {
        stream.Advance(); // Skip 'map' token

        if (stream.Current.TokenType != TokenType.Identifier)
        {
            throw new Exception("Expected property mapping name after 'map'");
        }

        var name = stream.Current.GetLexeme();
        stream.Advance(); // Skip property mapping name

        if (stream.Current.TokenType != TokenType.From)
        {
            throw new Exception("Expected 'from' after property mapping name");
        }

        stream.Advance(); // Skip 'from'

        if (stream.Current.TokenType != TokenType.Identifier)
        {
            throw new Exception("Expected source property after 'from'");
        }

        var sourceProperty = stream.Current.GetLexeme();
        stream.Advance(); // Skip source property

        return new PropertyMapping
        {
            TargetProperties = [name],
            Expression = new GetSourcePropertyValueExpression(sourceProperty)
        };
    }

    private Dictionary<string, string>? ParseMeta(TokenStream stream)
    {
        var meta = new Dictionary<string, string>();
        stream.Advance(); // Skip 'meta' token

        if (stream.Current.TokenType != TokenType.LeftBracket)
        {
            throw new Exception("Expected '{' after 'meta'");
        }

        stream.Advance(); // Skip '{'

        while (stream.Current.TokenType != TokenType.RightBracket)
        {
            if (stream.Current.TokenType == TokenType.EOF)
            {
                throw new Exception("Unexpected end of file while parsing meta");
            }

            if (stream.Current.TokenType != TokenType.Identifier)
            {
                throw new Exception("Expected string key in meta");
            }

            var key = stream.Current.GetLexeme();
            stream.Advance(); // Skip key

            if (stream.Current.TokenType != TokenType.Colon)
            {
                throw new Exception("Expected ':' after meta key");
            }

            stream.Advance(); // Skip ':'

            if (stream.Current.TokenType != TokenType.StringLiteral)
            {
                throw new Exception("Expected string value in meta");
            }

            var value = stream.Current.GetLexeme();
            meta[key] = value;
            stream.Advance(); // Skip value

            if (stream.Current.TokenType == TokenType.RightBracket)
            {
                break;
            }
        }

        if (stream.Current.TokenType != TokenType.RightBracket)
        {
            throw new Exception("Expected '}' to close meta block");
        }

        stream.Advance(); // Skip '}'
        return meta;
    }
}
