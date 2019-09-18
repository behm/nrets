using System.Collections.Generic;

namespace NRets.Models
{
    public class MetadataResource
    {
        public string ResourceId { get; set; }
        public string StandardName { get; set; }
        public string VisibleName { get; set; }
        public string Description { get; set; }
        public string KeyField { get; set; }
        public string ClassCount { get; set; }
        public string ClassVersion { get; set; }
        public string ClassDate { get; set; }
        public string ObjectVersion { get; set; }
        public string ObjectDate { get; set; }
        public string SearchHelpVersion { get; set; }
        public string SearchHelpDate { get; set; }
        public string EditMaskVersion { get; set; }
        public string EditMaskDate { get; set; }
        public string LookupVersion { get; set; }
        public string LookupDate { get; set; }
        public string UpdateHelpVersion { get; set; }
        public string UpdateHelpDate { get; set; }
        public string ValidationExpressionVersion { get; set; }
        public string ValidationExpressionDate { get; set; }
        public string ValidationLookupVersion { get; set; }
        public string ValidationLookupDate { get; set; }
        public string ValidationExternalVersion { get; set; }
        public string ValidationExternalDate { get; set; }

        public IEnumerable<MetadataClass> Classes { get; set; }
    }
}
