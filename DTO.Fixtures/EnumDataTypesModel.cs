using DTO.Fixtures.Enums;

namespace DTO.Fixtures
{
    public class EnumDataTypesModel
    {
        public SimpleEnum TestSimpleEnum { get; set; }
        public List<SimpleEnum> ListOfSimpleEnum { get; set; }
        public SimpleEnum[] ArrayOfSimpleEnum { get; set; }
        public Dictionary<int, SimpleEnum> DictionaryOfSimpleEnum { get; set; }
    }
}
