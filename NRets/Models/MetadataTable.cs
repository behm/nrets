using System.Collections.Generic;

namespace NRets.Models
{
    public class MetadataTable
    {
        public string System { get; set; }
        public string Resource { get; set; }
        public string Class { get; set; }
        public string Version { get; set; }
        public string Date { get; set; }

        public IEnumerable<MetadataField> Fields { get; set; }
    }
}
