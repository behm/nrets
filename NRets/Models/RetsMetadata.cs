using System.Collections.Generic;

namespace NRets.Models
{
    public class RetsMetadata
    {
        public string MetadataVersion { get; set; }
        public string MetadataTimestamp { get; set; }
        public IEnumerable<MetadataResource> Resources { get; set; }
    }
}
