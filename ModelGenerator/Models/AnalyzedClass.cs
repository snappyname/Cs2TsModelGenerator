using System.Collections.Generic;

namespace ModelGenerator.Models;

public class AnalyzedClass
{
    public string Name { get; init; } = null!;
    public List<AnalyzedField> Fields { get; init; } = new();
    public List<string> Path { get; init; } = new();
    public Dictionary<string, string> Dependencies { get; } = new();
    public string? BaseClass { get; set; }
    public string GetStringPath()
    {
        var res = string.Empty;
        for (int i = 0; i< Path.Count; i++)
        {
            res += Path[i];
            res += "/";
        }
        return res;
    }
}
