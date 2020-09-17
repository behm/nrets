using System.Collections.Generic;

namespace NRets.Models
{
    public class MetadataClass
    {
        public string ClassName { get; set; }
        public string StandardName { get; set; }
        public string VisibleName { get; set; }
        public string Description { get; set; }
        public string TableVersion { get; set; }
        public string TableDate { get; set; }
        public string UpdateVersion { get; set; }
        public string UpdateDate { get; set; }
        public string ClassTimestamp { get; set; }
        public string DeletedFlagField { get; set; }
        public string DeletedFlagValue { get; set; }
        public string HasKeyIndex { get; set; }

        public IEnumerable<MetadataTable> Tables { get; set; }
    }
}
