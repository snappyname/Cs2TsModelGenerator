using GeneratorAttributes.Attributes;

namespace DTO.Fixtures.Attributes
{
    [GeneratorIgnore]
    public class IgnoredSimpleDtoModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
