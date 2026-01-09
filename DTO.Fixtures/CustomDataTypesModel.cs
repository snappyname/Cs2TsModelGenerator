namespace DTO.Fixtures
{
    public class CustomDataTypesModel
    {
        public SimpleDataTypesModel SimpleDataTypesModel { get; set; }
        public List<SimpleDataTypesModel> SimpleDataTypesModelList { get; set; }
        public SimpleDataTypesModel[] SimpleDataTypesModelArray { get; set; }
        public Dictionary<string, SimpleDataTypesModel> SimpleDataTypesModelDictionary { get; set; }   
        
        public SimpleInheritanceDataTypeModel SimpleInheritanceDataTypeModel { get; set; }
        public List<SimpleInheritanceDataTypeModel> SimpleInheritanceDataTypeModelList { get; set; }
        public SimpleInheritanceDataTypeModel[] SimpleInheritanceDataTypeModelArray { get; set; }
        public Dictionary<string, SimpleInheritanceDataTypeModel> SimpleInheritanceDataTypeModelDictionary { get; set; }
        
    }
}
