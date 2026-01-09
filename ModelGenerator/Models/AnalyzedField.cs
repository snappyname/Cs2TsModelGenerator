using Microsoft.CodeAnalysis;

namespace ModelGenerator.Models;

public class AnalyzedField
{
    public string? Name { get; init; } = null!;
    public ITypeSymbol TypeName { get; init; } = null!;
    public string Accessibility { get; init; } = null!;
    public string? Value { get; init; } = null!;
    public bool IsEnum { get; init; }
    public bool IsNullable { get; init; }
}
