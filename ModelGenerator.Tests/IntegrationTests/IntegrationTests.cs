using ModelGenerator;

namespace ModelGenerator.Tests.IntegrationTests;

public class IntegrationTests: IClassFixture<GeneratorFixture>
{
    private readonly GeneratorFixture _fixture;

    public IntegrationTests(GeneratorFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task SimpleDataTypes()
    {
        var dtoClass = _fixture.Classes.Single(x => x.Name == "SimpleDataTypesModel");
        var result = _fixture.TsGenerator.GenerateTypeScriptModel(dtoClass, GeneratorApp.CreateCsToTsMap());
        var expected = (await File.ReadAllTextAsync("../../../../DTO.Fixtures/SimpleDataTypesModel.txt")).Replace("\r", "");
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task ListAndDictionariesDataTypes()
    {
        var dtoClass = _fixture.Classes.Single(x => x.Name == "ListAndDictionariesDataTypesModel");
        var result = _fixture.TsGenerator.GenerateTypeScriptModel(dtoClass, GeneratorApp.CreateCsToTsMap());
        var expected = (await File.ReadAllTextAsync("../../../../DTO.Fixtures/ListAndDictionariesDataTypesModel.txt")).Replace("\r", "");
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task EnumsDataTypes()
    {
        var dtoClass = _fixture.Classes.Single(x => x.Name == "EnumDataTypesModel");
        var result = _fixture.TsGenerator.GenerateTypeScriptModel(dtoClass, GeneratorApp.CreateCsToTsMap());
        var expected = (await File.ReadAllTextAsync("../../../../DTO.Fixtures/EnumDataTypesModel.txt")).Replace("\r", "");
        Assert.Equal(expected, result);
    }
    
    [Fact] 
    public async Task CustomDataTypes()
    {
        var dtoClass = _fixture.Classes.Single(x => x.Name == "CustomDataTypesModel");
        var result = _fixture.TsGenerator.GenerateTypeScriptModel(dtoClass, GeneratorApp.CreateCsToTsMap());
        var expected = (await File.ReadAllTextAsync("../../../../DTO.Fixtures/CustomDataTypesModel.txt")).Replace("\r", "");
        Assert.Equal(expected, result);
    }  
    
    [Fact]
    public async Task SimpleInheritanceDataTypes()
    {
        var dtoClass = _fixture.Classes.Single(x => x.Name == "SimpleInheritanceDataTypeModel");
        var result = _fixture.TsGenerator.GenerateTypeScriptModel(dtoClass, GeneratorApp.CreateCsToTsMap());
        var expected = (await File.ReadAllTextAsync("../../../../DTO.Fixtures/SimpleInheritanceDataTypeModel.txt")).Replace("\r", "");
        Assert.Equal(expected, result);
    } 
    
    
    [Fact]
    public async Task AdvancedInheritanceDataTypes()
    {
        var dtoClass = _fixture.Classes.Single(x => x.Name == "AdvancedInheritanceDataTypeModel");
        var result = _fixture.TsGenerator.GenerateTypeScriptModel(dtoClass, GeneratorApp.CreateCsToTsMap());
        var expected = (await File.ReadAllTextAsync("../../../../DTO.Fixtures/Folder/AdvancedInheritanceDataTypeModel.txt")).Replace("\r", "");
        Assert.Equal(expected, result);
    }  
    
    [Fact]
    public async Task ReverseAdvancedInheritanceDataTypes()
    {
        var dtoClass = _fixture.Classes.Single(x => x.Name == "ReverseAdvancedInheritanceDataTypeModel");
        var result = _fixture.TsGenerator.GenerateTypeScriptModel(dtoClass, GeneratorApp.CreateCsToTsMap());
        var expected = (await File.ReadAllTextAsync("../../../../DTO.Fixtures/Folder/RelativeFolder/RelativeFolder2/ReverseAdvancedInheritanceDataTypes.txt")).Replace("\r", "");
        Assert.Equal(expected, result);
    }   
}
