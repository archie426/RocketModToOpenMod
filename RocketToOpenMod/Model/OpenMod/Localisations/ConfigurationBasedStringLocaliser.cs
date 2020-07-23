using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace RocketToOpenMod.Model.OpenMod.Localisations
{
    public class ConfigurationBasedStringLocalizer : IStringLocalizer
    {
        private readonly IConfiguration _configuration;

        public ConfigurationBasedStringLocalizer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var list = new List<LocalizedString>();
            GetAllStrings(_configuration, list);
            return list;
        }

        private void GetAllStrings(IConfiguration configuration, List<LocalizedString> list)
        {
            foreach (var child in configuration.GetChildren())
            {
                if (child.GetChildren().Any())
                {
                    GetAllStrings(child, list);
                    continue;
                }

                list.Add(new LocalizedString(child.Path, child.Value));
            }
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return this; // no culture support
        }

        public LocalizedString this[string name]
        {
            get
            {
                var configValue = _configuration.GetSection(name);
                if (!configValue.Exists() || string.IsNullOrEmpty(configValue.Value))
                {
                    return new LocalizedString(name, name, true);
                }

                return new LocalizedString(name, configValue.Value);
            }
        }

        //Not needed
        public LocalizedString this[string name, params object[] arguments] => null;
    }
}