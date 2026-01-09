using GeneratorAttributes.Attributes;

namespace DTO.Fixtures.Attributes
{
    public class SimpleDtoWithAttributesModel
    {
        [CustomFieldGenerationName("intProperty")]
        public int SomeIntProperty { get; set; }
        [GeneratorIgnore]
        public string SomeStringProperty { get; set; }
        [GeneratorIgnore]
        public SimpleDtoWithAttributesModel IgnoredDataTypes { get; set; }
    }
}
