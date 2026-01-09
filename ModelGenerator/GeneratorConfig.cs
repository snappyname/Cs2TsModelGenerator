namespace ModelGenerator;

public class GeneratorConfig
{
    public string SolutionPath { get; init; }
    public string TargetProjectName { get; init; }
    public string OutputPath { get; init; }
    public bool FieldLowerCase { get; set; } = true;
    public bool ClassNameLowerCase { get; set; } = false;
    public int ClassFileNameType { get; set; } = 3;
    public bool ClassFileNameLowerCase { get; set; } = true;
    public string EnumType { get; set; } = "any";
}
