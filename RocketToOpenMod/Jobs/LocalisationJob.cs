using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using RocketToOpenMod.Data;
using RocketToOpenMod.Model.OpenMod.Localisations;
using RocketToOpenMod.Model.Rocket.Translations;

namespace RocketToOpenMod.Jobs
{
    //TODO: Add support for Unturned and eco
    public class LocalisationJob : Job
    {

        public static readonly Dictionary<string, string> ErrorConversions = new Dictionary<string, string>()
        {
            { "command_no_permission", "restricted"}
        }; 
        
        
        public static readonly Dictionary<string, string> OtherConversions = new Dictionary<string, string>()
        {
            
        }; 
        
        
        public LocalisationJob(WriteFileType write) : base(write, "localisation")
        {
            
        }

        public override async Task DoAsync()
        {
            Console.WriteLine("[~] Loading Rocket translations");
            
            TranslationList rocket = await LoadTranslationsAsync();
            
            Console.WriteLine("[~] Preparing OpenMod translations");
            
            Dictionary<string, string> errors = new Dictionary<string, string>();
            foreach (string key in ErrorConversions.Keys)
            {
                string value = rocket.Translate(key);
                string newKey = ErrorConversions[key];
                errors.Add(newKey, value);
            }
            
            Dictionary<string, string> others = new Dictionary<string, string>();
            foreach (string key in OtherConversions.Keys)
            {
                string value = rocket.Translate(key);
                string newKey = OtherConversions[key];
                others.Add(newKey, value);
            }
            
            commands openMod = new commands(others, errors);
            
            Console.WriteLine("[~] Saving OpenMod translations");
            await SaveAsync(openMod);
        }
    }
}