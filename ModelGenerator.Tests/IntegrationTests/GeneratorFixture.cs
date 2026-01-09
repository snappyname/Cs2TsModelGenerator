using ModelGenerator;
using ModelGenerator.Models;
using ModelGenerator.Services;

namespace ModelGenerator.Tests.IntegrationTests
{
    public class GeneratorFixture : IAsyncLifetime
    {
        private GeneratorConfig Config { get; set; } = null!;
        public List<AnalyzedClass> Classes { get; private set; } = null!;
        public TypeScriptGenerator TsGenerator { get; private set; } = null!;

        public async Task InitializeAsync()
        {
            Config = new GeneratorConfig
            {
                SolutionPath = @"..\..\..\..\ModelGenerator.sln",
                OutputPath = "",
                TargetProjectName = "DTO.Fixtures",
                FieldLowerCase = false,
                ClassNameLowerCase = false,
                ClassFileNameType = 3,
                ClassFileNameLowerCase = true,
                EnumType = "any"
            };

            var (compilation, solutionDir) = await GeneratorApp.LoadCompilationAsync(Config.SolutionPath, Config.TargetProjectName);
            var analyzer = new ClassAnalyzer(compilation, solutionDir);
            Classes = analyzer.Analyze().ToList();
            TsGenerator = new TypeScriptGenerator(Config);
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }

}
