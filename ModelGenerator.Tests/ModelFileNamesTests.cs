using ModelGenerator;

namespace ModelGenerator.Tests
{
    public class ModelFileNamesTests
    {
        
        [Fact]
        public void GetFilename_WhenNameUpperCase_ClassNameType1()
        {
            GeneratorConfig config = new() { ClassFileNameLowerCase = false, ClassFileNameType = 1 };
            var result = Helpers.GetFileName("TestModel", config);
            var expected = "Test.model";
            Assert.Equal(expected, result);
        } 
        
        [Fact]
        public void GetFilename_WhenNameLowerCase_ClassNameType1()
        {
            GeneratorConfig config = new() { ClassFileNameLowerCase = true, ClassFileNameType = 1 };
            var result = Helpers.GetFileName("TestModel", config);
            var expected = "test.model";
            Assert.Equal(expected, result);
        }
        
        [Fact]
        public void GetFilename_WhenNameUpperCase_ClassNameType2()
        {
            GeneratorConfig config = new() { ClassFileNameLowerCase = false, ClassFileNameType = 2 };
            var result = Helpers.GetFileName("TestModel", config);
            var expected = "TestModel";
            Assert.Equal(expected, result);
        } 
        
        [Fact]
        public void GetFilename_WhenNameLowerCase_ClassNameType2()
        {
            GeneratorConfig config = new() { ClassFileNameLowerCase = true, ClassFileNameType = 2 };
            var result = Helpers.GetFileName("TestModel", config);
            var expected = "testModel";
            Assert.Equal(expected, result);
        }    
        
        [Fact]
        public void GetFilename_WhenNameUpperCase_ClassNameType3()
        {
            GeneratorConfig config = new() { ClassFileNameLowerCase = false, ClassFileNameType = 3 };
            var result = Helpers.GetFileName("TestUserModel", config);
            var expected = "Test-User.model";
            Assert.Equal(expected, result);
        } 
        
        [Fact]
        public void GetFilename_WhenNameLowerCase_ClassNameType3()
        {
            GeneratorConfig config = new() { ClassFileNameLowerCase = true, ClassFileNameType = 3 };
            var result = Helpers.GetFileName("TestUserModel", config);
            var expected = "test-user.model";
            Assert.Equal(expected, result);
        }    
        
        [Fact]
        public void GetFilename_WhenNameLowerCase_ClassNameType3_withoutModelOnName()
        {
            GeneratorConfig config = new() { ClassFileNameLowerCase = true, ClassFileNameType = 3 };
            var result = Helpers.GetFileName("TestUser", config);
            var expected = "test-user.model";
            Assert.Equal(expected, result);
        } 
    }
}
