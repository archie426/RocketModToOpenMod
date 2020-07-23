using System;
using System.Xml.Serialization;

namespace RocketToOpenMod.Model.Rocket.Translations
{
    [Serializable]
    [XmlType(AnonymousType = false, IncludeInSchema = true, TypeName = "Translation")]
    public class TranslationListEntry
    {
        [XmlAttribute]
        public string Id;
        [XmlAttribute]
        public string Value;

        public TranslationListEntry(string id, string value)
        {
            Id = id;
            Value = value;
        }
        public TranslationListEntry() { }
    }
}