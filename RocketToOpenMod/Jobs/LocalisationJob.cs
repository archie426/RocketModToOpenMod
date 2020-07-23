using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using RocketToOpenMod.Model.OpenMod.Localisations;
using RocketToOpenMod.Model.Rocket.Translations;

namespace RocketToOpenMod.Jobs
{
    //TODO: Add support for Unturned and eco
    public class LocalisationJob : Job
    {

        public static readonly Dictionary<string, string> Conversions = new Dictionary<string, string>()
        {
            { "command_no_permission", "commands:openmod:restricted"}
        }; 
        
        
        public LocalisationJob() : base("localisation")
        {
            
        }

        public override async Task DoAsync()
        {
            TranslationList rocket = await LoadTranslationsAsync();
            
            var translations = new ConfigurationBuilder()
                .SetBasePath("OpenMod")
                .AddYamlFile("translations.yaml", true, reloadOnChange: true)
                .Build();
            
            IStringLocalizer localizer = new ConfigurationBasedStringLocalizer(translations);

            foreach (string rocketKey in Conversions.Keys)
            {
                //TODO: Sort this all out
                localizer[Conversions[rocketKey]] = new LocalizedString(Conversions[rocketKey],rocket.Translate(rocketKey));
            }
            
        }
    }
}