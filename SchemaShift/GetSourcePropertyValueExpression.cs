namespace SchemaShift;

internal class GetSourcePropertyValueExpression(string sourcePropertyName) : Expression
{
    public string SourcePropertyName { get; } = sourcePropertyName;
}