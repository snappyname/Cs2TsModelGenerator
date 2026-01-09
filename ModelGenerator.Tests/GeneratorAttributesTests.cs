using ModelGenerator.Tests.IntegrationTests;
using ModelGenerator;

namespace ModelGenerator.Tests
{
    public class GeneratorAttributesTests : IClassFixture<GeneratorFixture>
    {
        private readonly GeneratorFixture _fixture;

        public GeneratorAttributesTests(GeneratorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SimpleDtoWithAttributesModel()
        {
            var dtoClass = _fixture.Classes.Single(x => x.Name == "SimpleDtoWithAttributesModel");
            var result = _fixture.TsGenerator.GenerateTypeScriptModel(dtoClass, GeneratorApp.CreateCsToTsMap());
            var expected = (await File.ReadAllTextAsync("../../../../DTO.Fixtures/Attributes/SimpleDtoWithAttributesModel.txt")).Replace("\r", "");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IgnoredSimpleDtoModel()
        {
            var dtoClass = _fixture.Classes.FirstOrDefault(x => x.Name == "IgnoredSimpleDtoModel");
            Assert.Null(dtoClass);
        }
    }
}
