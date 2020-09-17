namespace NRets.Models
{
    public class MetadataField
    {
        public string MetadataEntryId { get; set; }
        public string SystemName { get; set; }
        public string StandardName { get; set; }
        public string LongName { get; set; }
        public string DBName { get; set; }
        public string ShortName { get; set; }
        public string MaximumLength { get; set; }
        public string DataType { get; set; }
        public string Precision { get; set; }
        public string Searchable { get; set; }
        public string Interpretation { get; set; }
        public string Alignment { get; set; }
        public string UseSeparator { get; set; }
        public string EditMaskId { get; set; }
        public string LookupName { get; set; }
        public string MaxSelect { get; set; }
        public string Units { get; set; }
        public string Index { get; set; }
        public string Minimum { get; set; }
        public string Maximum { get; set; }
        public string Default { get; set; }
        public string Required { get; set; }
        public string SearchHelpId { get; set; }
        public string Unique { get; set; }
        public string ModTimeStamp { get; set; }
        public string ForeignKeyName { get; set; }
        public string ForeignField { get; set; }
        public string KeyQuery { get; set; }
        public string KeySelect { get; set; }
        public string InKeyIndex { get; set; }
    }
}
