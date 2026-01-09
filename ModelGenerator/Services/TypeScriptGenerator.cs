using ModelGenerator.Models;
using System.Collections.Generic;
using System.IO;

namespace ModelGenerator
{
    public class TypeScriptGenerator
    {
        private readonly GeneratorConfig _config;

        public TypeScriptGenerator(GeneratorConfig config)
        {
            _config = config;
        }
        
        public string GenerateTypeScriptModel(AnalyzedClass analyzedClass, Dictionary<string, string> cs2tsTypes)
        {
            var result = string.Empty;
            result += GenerateImports(analyzedClass);
            result += GenerateTsClass(analyzedClass);
            result += GenerateTsFields(analyzedClass, cs2tsTypes, _config);
            result += GenerateConstructor(analyzedClass);
            return result;
        }
        
        private string GenerateImports(AnalyzedClass analyzedClass)
        {
            var result = string.Empty;
            foreach (var dependence in analyzedClass.Dependencies)
            {
                result += "import { " +
                          (_config.ClassNameLowerCase ? Helpers.GetLowerCaseName(dependence.Key) : dependence.Key) +
                          " } from './" +
                          Path.GetRelativePath(analyzedClass.GetStringPath(), dependence.Value).Replace("\\", "/") + "/" +
                          Helpers.GetFileName(dependence.Key, _config) + "';\n";
            }

            if (analyzedClass.Dependencies.Count > 0)
            {
                result += "\n";
            }
            
            return result;
        }

        private string GenerateTsClass(AnalyzedClass analyzedClass)
        {
            var result = string.Empty;
            result += $"export class {(_config.ClassNameLowerCase ? Helpers.GetLowerCaseName(analyzedClass.Name) : analyzedClass.Name)}";
            if (analyzedClass.BaseClass != null)
            {
                result +=
                    $" extends {(_config.ClassNameLowerCase ? Helpers.GetLowerCaseName(analyzedClass.BaseClass) : analyzedClass.BaseClass)}";
            }

            result += " {\n";
            return result;
        }

        private string GenerateTsFields(AnalyzedClass analyzedClass, Dictionary<string, string> cs2tsTypes, GeneratorConfig config)
        {
            var result = string.Empty;
            foreach (var field in analyzedClass.Fields)
            {
                var tsType = Helpers.MapToTypeScript(field.TypeName, cs2tsTypes, config);
                result += $"    {(_config.FieldLowerCase ? Helpers.GetLowerCaseName(field.Name) : field.Name)}: {tsType};\n";
            }
            result += "\n";
            return result;
        }

        private string GenerateConstructor(AnalyzedClass analyzedClass)
        {
            var result = string.Empty;
            result +=
                $"\tconstructor(partial?: Partial<{(_config.ClassNameLowerCase ? Helpers.GetLowerCaseName(analyzedClass.Name) : analyzedClass.Name)}>) {{\n";
            if (analyzedClass.BaseClass != null)
            {
                result += "\t\tsuper();\n";
            }

            result += "\t\tif (partial) {\n" +
                      "\t\t\tObject.assign(this, partial);\n" +
                      "\t\t}\n" +
                      "\t}\n" +
                      "}\n";
            return result;
        }
    }
}
