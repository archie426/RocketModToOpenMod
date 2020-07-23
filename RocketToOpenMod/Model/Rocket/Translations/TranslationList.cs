using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace RocketToOpenMod.Model.Rocket.Translations
{
    [XmlRoot("Translations")]
    [XmlType(AnonymousType = false, IncludeInSchema = true, TypeName = "Translation")]
    [Serializable]
    public class TranslationList
    {
        public TranslationList() { }
        
        protected List<TranslationListEntry> translations = new List<TranslationListEntry>();
        
        public string this[string key]
        {
            get
            {
                return translations.Where(k => k.Id == key).Select(k => k.Value).FirstOrDefault();
            }
            set
            {
                translations.ForEach(k => { if (k.Id == key) k.Value = value; });
            }
        }

        public string Translate(string translationKey, params object[] placeholder)
        {
            string value = this[translationKey];
            if (String.IsNullOrEmpty(value)) return translationKey;
                
            if (value.Contains("{") && value.Contains("}") && placeholder != null && placeholder.Length != 0)
            {
                for (int i = 0; i < placeholder.Length; i++)
                {
                    if (placeholder[i] == null) placeholder[i] = "NULL";
                }
                value = String.Format(value, placeholder);
            }
            return value;
        }
        
    }
}