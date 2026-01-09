using GeneratorAttributes.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModelGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ModelGenerator.Services;

public class ClassAnalyzer
{
    private readonly Compilation _compilation;
    readonly string _solutionDirectory;

    public ClassAnalyzer(Compilation compilation, string solutionDirectory)
    {
        _compilation = compilation;
        _solutionDirectory = solutionDirectory;
    }

    private Dictionary<string, string> GetAllClassNamesWithPath()
    {
        var classNames = new Dictionary<string, string>();
        foreach (var tree in _compilation.SyntaxTrees)
        {
            var model = _compilation.GetSemanticModel(tree);
            var root = tree.GetRoot();
            foreach (var cls in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                var symbol = model.GetDeclaredSymbol(cls);
                if (symbol is INamedTypeSymbol classSymbol)
                {
                    var filePath = tree.FilePath;
                    var relativePath = Path.GetRelativePath(Path.Combine(_solutionDirectory), filePath);
                    var folderPath = Path.Combine(Path.GetDirectoryName(relativePath)!);
                    try
                    {
                        classNames.Add(classSymbol.Name, folderPath.Replace("\\", "/"));
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine("DTO classes is not unique");
                        Console.WriteLine(ex.Message);
                        throw new Exception();
                    }
                }
            }
        }
        return classNames;
    }

    public IEnumerable<AnalyzedClass> Analyze()
    {
        var projectClassNames = GetAllClassNamesWithPath();
        foreach (var tree in _compilation.SyntaxTrees)
        {
            var model = _compilation.GetSemanticModel(tree);
            var root = tree.GetRoot();
            foreach (var cls in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                var symbol = model.GetDeclaredSymbol(cls);
                if (symbol is not INamedTypeSymbol classSymbol)
                    continue;
                if (symbol.GetAttributes().Any(x => x.AttributeClass?.Name == GeneratorIgnore.ToString()))
                    continue;
                yield return AnalyzeClass(classSymbol, tree, projectClassNames);
            }
        }
    }

    private AnalyzedClass AnalyzeClass(INamedTypeSymbol classSymbol, SyntaxTree tree, Dictionary<string, string> projectClassNames)
    {
        var filePath = tree.FilePath;
        var relativePath = Path.GetRelativePath(_solutionDirectory, filePath);
        var folders = Path
            .GetDirectoryName(relativePath)!
            .Split(Path.DirectorySeparatorChar)
            .ToList();
        var analyzedClass = new AnalyzedClass { Name = classSymbol.Name, Path = folders };
        var baseType = classSymbol.BaseType;

        if (baseType != null && baseType.SpecialType != SpecialType.System_Object)
        {
            var baseClassName = baseType.Name;
            if (projectClassNames.TryGetValue(baseClassName, out var importPath) && baseType.TypeKind != TypeKind.Enum)
            {
                analyzedClass.BaseClass = baseClassName;
                analyzedClass.Dependencies.Add(baseClassName, importPath);
            }
        }

        foreach (var prop in classSymbol.GetMembers().OfType<IPropertySymbol>())
        {
            if (prop.GetAttributes().Any(x => x.AttributeClass?.Name == GeneratorIgnore.ToString()))
                continue;
            var field = AnalyzeProperty(prop);
            analyzedClass.Fields.Add(field);

            foreach (var refType in GetReferencedTypes(prop.Type))
            {
                if (refType.TypeKind == TypeKind.Enum)
                    continue;

                var typeName = refType.Name;
                if (projectClassNames.TryGetValue(typeName, out var importPath) && typeName != analyzedClass.Name)
                {
                    analyzedClass.Dependencies.TryAdd(typeName, importPath);
                }
            }
        }
        return analyzedClass;
    }

    private AnalyzedField AnalyzeProperty(IPropertySymbol prop)
    {
        var type = prop.Type;
        var attr = prop.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == CustomFieldGenerationName.ToString());
        var propertyName = prop.Name;
        if (attr != null)
        {
            propertyName = attr.ConstructorArguments[0].Value?.ToString();
        }

        string? defaultValue = null;
        var syntaxRef = prop.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxRef != null)
        {
            var syntax = syntaxRef.GetSyntax() as PropertyDeclarationSyntax;
            var initializer = syntax?.Initializer?.Value;

            if (initializer != null)
            {
                defaultValue = initializer.ToString();
            }
        }

        return new AnalyzedField
        {
            Name = propertyName,
            TypeName = type,
            Accessibility = prop.DeclaredAccessibility.ToString(),
            IsEnum = type.TypeKind == TypeKind.Enum,
            IsNullable = type.NullableAnnotation == NullableAnnotation.Annotated,
            Value = defaultValue
        };
    }

    private static IEnumerable<INamedTypeSymbol> GetReferencedTypes(ITypeSymbol type)
    {
        switch (type)
        {
            case IArrayTypeSymbol array:
                return GetReferencedTypes(array.ElementType);

            case INamedTypeSymbol named when named.IsGenericType:
                return named.TypeArguments.SelectMany(GetReferencedTypes);

            case INamedTypeSymbol named:
                return new[] { named };

            default:
                return Enumerable.Empty<INamedTypeSymbol>();
        }
    }
}
