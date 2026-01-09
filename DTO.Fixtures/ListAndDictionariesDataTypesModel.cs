namespace DTO.Fixtures
{
    public class ListAndDictionariesDataTypesModel
    {
        public List<int> Test1DIntList { get; set; }
        public List<List<int>> Test2DIntList { get; set; }
        public string[] Test1DStringArray { get; set; }
        public bool[][] Test2DBoolArray { get; set; }
        public Dictionary<int, string> TestDictionary { get; set; }
    }
}
