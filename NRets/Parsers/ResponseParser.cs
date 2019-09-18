using NRets.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NRets.Parsers
{
    public interface IResponseParser
    {
        LoginResponse ParseLoginResponse(string responseText);
        RetsMetadata ParseMetadataSystemResponse(string responseText);
    }

    // NOTE: Reference to RETS spec can be found here - https://www.rapattoni.com/rapdocs/support/mls/retsdocs/RETS_1_7_2.pdf

    public class ResponseParser : IResponseParser
    {
        public LoginResponse ParseLoginResponse(string responseText)
        {
            if (string.IsNullOrWhiteSpace(responseText))
            {
                throw new ArgumentNullException(nameof(responseText));
            }

            var loginResponse = new LoginResponse();

            var lines = responseText.Split(Environment.NewLine.ToCharArray());
            foreach (var line in lines)
            {
                var parts = line.Split("=".ToCharArray());
                if (parts.Length < 2)
                {
                    continue;
                }

                loginResponse.ResponseItems.Add(parts[0], parts[1]);
            }

            return loginResponse;
        }

        public RetsMetadata ParseMetadataSystemResponse(string responseText)
        {
            if (string.IsNullOrWhiteSpace(responseText))
            {
                throw new ArgumentNullException(nameof(responseText));
            }

            var textReader = new StringReader(responseText);
            var document = XDocument.Load(textReader);
            var ns = document.Root.GetDefaultNamespace();
            var root = document.Root;
            var metadataSystemElem = root.Descendants(ns + "METADATA-SYSTEM").FirstOrDefault();
            var resourceElement = metadataSystemElem.Descendants(ns + "METADATA-RESOURCE").FirstOrDefault();
            var resources = ParseResources(ns, resourceElement.Descendants(ns + "Resource"));            

            return new RetsMetadata
            {
                MetadataVersion = metadataSystemElem.Attribute("Version")?.Value,
                MetadataTimestamp = metadataSystemElem.Attribute("Date")?.Value,
                Resources = resources,
            };
        }

        private IEnumerable<MetadataResource> ParseResources(XNamespace ns, IEnumerable<XElement> resourceElements)
        {
            var resources = new List<MetadataResource>();

            foreach (var resourceElem in resourceElements)
            {
                var resource = new MetadataResource
                {
                    ResourceId = resourceElem.Descendants(ns + "ResourceID").FirstOrDefault()?.Value,
                    StandardName = resourceElem.Descendants(ns + "StandardName").FirstOrDefault()?.Value,
                    VisibleName = resourceElem.Descendants(ns + "VisibleName").FirstOrDefault()?.Value,
                    Description = resourceElem.Descendants(ns + "Description").FirstOrDefault()?.Value,
                    KeyField = resourceElem.Descendants(ns + "KeyField").FirstOrDefault()?.Value,
                    ClassCount = resourceElem.Descendants(ns + "ClassCount").FirstOrDefault()?.Value,
                    ClassVersion = resourceElem.Descendants(ns + "ClassVersion").FirstOrDefault()?.Value,
                    ClassDate = resourceElem.Descendants(ns + "ClassDate").FirstOrDefault()?.Value,
                    ObjectVersion = resourceElem.Descendants(ns + "ObjectVersion").FirstOrDefault()?.Value,
                    ObjectDate = resourceElem.Descendants(ns + "ObjectDate").FirstOrDefault()?.Value,
                    SearchHelpVersion = resourceElem.Descendants(ns + "SearchHelpVersion").FirstOrDefault()?.Value,
                    SearchHelpDate = resourceElem.Descendants(ns + "SearchHelpDate").FirstOrDefault()?.Value,
                    EditMaskVersion = resourceElem.Descendants(ns + "EditMaskVersion").FirstOrDefault()?.Value,
                    EditMaskDate = resourceElem.Descendants(ns + "EditMaskDate").FirstOrDefault()?.Value,
                    LookupVersion = resourceElem.Descendants(ns + "LookupVersion").FirstOrDefault()?.Value,
                    LookupDate = resourceElem.Descendants(ns + "LookupDate").FirstOrDefault()?.Value,
                    UpdateHelpVersion = resourceElem.Descendants(ns + "UpdateHelpVersion").FirstOrDefault()?.Value,
                    UpdateHelpDate = resourceElem.Descendants(ns + "UpdateHelpDate").FirstOrDefault()?.Value,
                    ValidationExpressionVersion = resourceElem.Descendants(ns + "ValidationExpressionVersion").FirstOrDefault()?.Value,
                    ValidationExpressionDate = resourceElem.Descendants(ns + "ValidationExpressionDate").FirstOrDefault()?.Value,
                    ValidationLookupVersion = resourceElem.Descendants(ns + "ValidationLookupVersion").FirstOrDefault()?.Value,
                    ValidationLookupDate = resourceElem.Descendants(ns + "ValidationLookupDate").FirstOrDefault()?.Value,
                    ValidationExternalVersion = resourceElem.Descendants(ns + "ValidationExternalVersion").FirstOrDefault()?.Value,
                    ValidationExternalDate = resourceElem.Descendants(ns + "ValidationExternalDate").FirstOrDefault()?.Value
                };

                var metadataClassElem = resourceElem.Descendants(ns + "METADATA-CLASS").FirstOrDefault();
                resource.Classes = ParseClasses(ns, metadataClassElem.Descendants(ns + "Class"));

                resources.Add(resource);
            }

            return resources;
        }

        private static IEnumerable<MetadataClass> ParseClasses(XNamespace ns, IEnumerable<XElement> classElements)
        {
            var classes = new List<MetadataClass>();

            foreach (var classElem in classElements)
            {
                var metadataClass = new MetadataClass();
                metadataClass.ClassName = classElem.Descendants(ns + "ClassName").FirstOrDefault()?.Value;
                metadataClass.StandardName = classElem.Descendants(ns + "StandardName").FirstOrDefault()?.Value;
                metadataClass.VisibleName = classElem.Descendants(ns + "VisibleName").FirstOrDefault()?.Value;
                metadataClass.Description = classElem.Descendants(ns + "Description").FirstOrDefault()?.Value;
                metadataClass.TableVersion = classElem.Descendants(ns + "TableVersion").FirstOrDefault()?.Value;
                metadataClass.TableDate = classElem.Descendants(ns + "TableDate").FirstOrDefault()?.Value;
                metadataClass.UpdateVersion = classElem.Descendants(ns + "UpdateVersion").FirstOrDefault()?.Value;
                metadataClass.UpdateDate = classElem.Descendants(ns + "UpdateDate").FirstOrDefault()?.Value;
                metadataClass.ClassTimestamp = classElem.Descendants(ns + "ClassTimeStamp").FirstOrDefault()?.Value;
                metadataClass.DeletedFlagField = classElem.Descendants(ns + "DeletedFlagField").FirstOrDefault()?.Value;
                metadataClass.DeletedFlagField = classElem.Descendants(ns + "DeletedFlagValue").FirstOrDefault()?.Value;
                metadataClass.HasKeyIndex = classElem.Descendants(ns + "HasKeyIndex").FirstOrDefault()?.Value;

                var tableElements = classElem.Descendants(ns + "METADATA-TABLE");
                metadataClass.Tables = ParseTables(ns, tableElements);

                classes.Add(metadataClass);
            }

            return classes;
        }

        private static IEnumerable<MetadataTable> ParseTables(XNamespace ns, IEnumerable<XElement> tableElements)
        {
            var tables = new List<MetadataTable>();

            foreach (var tableElem in tableElements)
            {
                var table = new MetadataTable
                {
                    System = tableElem.Attribute("System")?.Value,
                    Resource = tableElem.Attribute("Resource")?.Value,
                    Class = tableElem.Attribute("Class")?.Value,
                    Version = tableElem.Attribute("Version")?.Value,
                    Date = tableElem.Attribute("Date")?.Value,
                };

                var fieldElements = tableElem.Descendants(ns + "Field");
                table.Fields = ParseFields(ns, fieldElements);

                tables.Add(table);
            }

            return tables;
        }

        private static IEnumerable<MetadataField> ParseFields(XNamespace ns, IEnumerable<XElement> fieldElements)
        {
            var fields = new List<MetadataField>();

            foreach (var fieldElem in fieldElements)
            {
                var field = new MetadataField
                {
                    MetadataEntryId = fieldElem.Descendants(ns + "ClassName").FirstOrDefault()?.Value,
                    SystemName = fieldElem.Descendants(ns + "SystemName").FirstOrDefault()?.Value,
                    StandardName = fieldElem.Descendants(ns + "StandardName").FirstOrDefault()?.Value,
                    LongName = fieldElem.Descendants(ns + "LongName").FirstOrDefault()?.Value,
                    DBName = fieldElem.Descendants(ns + "DBName").FirstOrDefault()?.Value,
                    ShortName = fieldElem.Descendants(ns + "ShortName").FirstOrDefault()?.Value,
                    MaximumLength = fieldElem.Descendants(ns + "MaximumLength").FirstOrDefault()?.Value,
                    DataType = fieldElem.Descendants(ns + "DataType").FirstOrDefault()?.Value,
                    Precision = fieldElem.Descendants(ns + "Precision").FirstOrDefault()?.Value,
                    Searchable = fieldElem.Descendants(ns + "Searchable").FirstOrDefault()?.Value,
                    Interpretation = fieldElem.Descendants(ns + "Interpretation").FirstOrDefault()?.Value,
                    Alignment = fieldElem.Descendants(ns + "Alignment").FirstOrDefault()?.Value,
                    UseSeparator = fieldElem.Descendants(ns + "UseSeparator").FirstOrDefault()?.Value,
                    EditMaskId = fieldElem.Descendants(ns + "EditMaskId").FirstOrDefault()?.Value,
                    LookupName = fieldElem.Descendants(ns + "LookupName").FirstOrDefault()?.Value,
                    MaxSelect = fieldElem.Descendants(ns + "MaxSelect").FirstOrDefault()?.Value,
                    Units = fieldElem.Descendants(ns + "Units").FirstOrDefault()?.Value,
                    Index = fieldElem.Descendants(ns + "Index").FirstOrDefault()?.Value,
                    Minimum = fieldElem.Descendants(ns + "Minimum").FirstOrDefault()?.Value,
                    Maximum = fieldElem.Descendants(ns + "Maximum").FirstOrDefault()?.Value,
                    Default = fieldElem.Descendants(ns + "Default").FirstOrDefault()?.Value,
                    Required = fieldElem.Descendants(ns + "Required").FirstOrDefault()?.Value,
                    SearchHelpId = fieldElem.Descendants(ns + "SearchHelpId").FirstOrDefault()?.Value,
                    Unique = fieldElem.Descendants(ns + "Unique").FirstOrDefault()?.Value,
                    ModTimeStamp = fieldElem.Descendants(ns + "ModTimeStamp").FirstOrDefault()?.Value,
                    ForeignKeyName = fieldElem.Descendants(ns + "ForeignKeyName").FirstOrDefault()?.Value,
                    ForeignField = fieldElem.Descendants(ns + "ForeignField").FirstOrDefault()?.Value,
                    KeyQuery = fieldElem.Descendants(ns + "KeyQuery").FirstOrDefault()?.Value,
                    KeySelect = fieldElem.Descendants(ns + "KeySelect").FirstOrDefault()?.Value,
                    InKeyIndex = fieldElem.Descendants(ns + "InKeyIndex").FirstOrDefault()?.Value,
                };

                fields.Add(field);
            }

            return fields;
        }
    }
}
