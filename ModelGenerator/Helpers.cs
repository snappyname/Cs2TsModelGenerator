using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ModelGenerator
{
    public static class Helpers
    {
        public static string GetFileName(string fileName, GeneratorConfig config)
        {
            if (config.ClassFileNameType == 1) //TestModel => Test.model.ts or Test => Test.model.ts
            {
                if (fileName.Length > 5 && fileName.Substring(fileName.Length - 5).ToLower() == "model")
                {
                    fileName = fileName.Substring(0, fileName.Length - 5);
                }

                return (config.ClassFileNameLowerCase ? GetLowerCaseName(fileName) : fileName) + ".model";
            }

            if (config.ClassFileNameType == 2) //TestModel => TestModel.ts or Test => Test.ts
            {
                return (config.ClassFileNameLowerCase ? GetLowerCaseName(fileName) : fileName);
            }

            if (config.ClassFileNameType == 3) //TestUserModel => Test-User.model.ts
            {
                if (fileName.Length > 5 && fileName.Substring(fileName.Length - 5).ToLower() == "model")
                {
                    fileName = fileName.Substring(0, fileName.Length - 5);
                }

                var result = Regex.Replace(fileName, @"(\p{Lu})", "-$1").TrimStart('-') + ".model";
                return config.ClassFileNameLowerCase ? result.ToLower() : result;
            }

            throw new Exception();
        }

        public static string GetLowerCaseName(string fieldName)
        {
            return fieldName[0].ToString().ToLower() + fieldName.Substring(1);
        }

        public static string MapToTypeScript(ITypeSymbol type, Dictionary<string, string> map, GeneratorConfig config)
        {
            // enum
            if (type.TypeKind == TypeKind.Enum)
                return config.EnumType;

            // array
            if (type is IArrayTypeSymbol array)
            {
                return $"{MapToTypeScript(array.ElementType, map, config)}[]";
            }

            // List<T>
            if (type is INamedTypeSymbol list &&
                list.OriginalDefinition.ToDisplayString() ==
                "System.Collections.Generic.List<T>")
            {
                return $"{MapToTypeScript(list.TypeArguments[0], map, config)}[]";
            }

            // Dictionary<K,V>
            if (type is INamedTypeSymbol dict && dict.OriginalDefinition.ToDisplayString() == "System.Collections.Generic.Dictionary<TKey, TValue>")
            {
                var key = MapToTypeScript(dict.TypeArguments[0], map, config);
                var value = MapToTypeScript(dict.TypeArguments[1], map, config);
                return $"Record<{key}, {value}>";
            }

            var fullName = type.ToDisplayString();
            if (map.TryGetValue(fullName, out var ts))
                return ts;

            return type.Name;
        }
    }
}
