using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using ModelGenerator.Models;
using ModelGenerator.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModelGenerator
{
    public class GeneratorApp
    {
        private static GeneratorConfig _config;

        public static Dictionary<string, string> CreateCsToTsMap() =>
            new() { ["string"] = "string", ["int"] = "number", ["long"] = "number", ["bool"] = "boolean" };
        
        public async Task RunAsync(string configPath)
        {
            _config = Newtonsoft.Json.JsonConvert.DeserializeObject<GeneratorConfig>(await File.ReadAllTextAsync(configPath));
            var (compilation, solutionDir) = await LoadCompilationAsync(_config.SolutionPath, _config.TargetProjectName);
            var analyzer = new ClassAnalyzer(compilation, solutionDir);
            var classes = AnalyzeClasses(analyzer);
            GenerateTypeScript(classes);
        }

        public static async Task<(Compilation compilation, string solutionDir)> LoadCompilationAsync(string solutionPath, string projectName)
        {
            using var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (_, e) => { Console.WriteLine($"[MSBuild] {e.Diagnostic.Message}"); };
            var solution = await workspace.OpenSolutionAsync(solutionPath);
            var solutionDir = Path.GetDirectoryName(solution.FilePath)!;
            var project = solution.Projects.First(p => p.Name == projectName);
            var compilation = await project.GetCompilationAsync() ?? throw new InvalidOperationException("Compilation is null");
            return (compilation, solutionDir);
        }
        
        public static void FinishConsoleOutput()
        {
            if (!_config.CloseAfterGeneration)
            {
                Console.ReadLine();
            }
        }

        private static List<AnalyzedClass> AnalyzeClasses(ClassAnalyzer analyzer)
        {
            return analyzer.Analyze().ToList();
        }

        private static void GenerateTypeScript(IEnumerable<AnalyzedClass> classes)
        {
            var cs2tsTypes = CreateCsToTsMap();
            var generator = new TypeScriptGenerator(_config);
            foreach (var analyzedClass in classes)
            {
                var modelContent = generator.GenerateTypeScriptModel(analyzedClass, cs2tsTypes);
                string filePath = _config.OutputPath +
                                  Path.GetRelativePath(_config.TargetProjectName, analyzedClass.GetStringPath()) +
                                  $"\\{Helpers.GetFileName(analyzedClass.Name, _config)}.ts";
                WriteFileWithDirectories(filePath, modelContent);
            }

            FinishConsoleOutput();
        }
        
        private static void WriteFileWithDirectories(string filePath, string content)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter writer = new(filePath, false, Encoding.UTF8))
            {
                writer.Write(content);
            }
            Console.WriteLine("Success: " + filePath);
        }
    }
}
