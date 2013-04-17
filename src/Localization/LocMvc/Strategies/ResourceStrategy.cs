using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using LocMvc.Configuration;
using System.Text.RegularExpressions;
using System.Resources;

namespace LocMvc.Strategies
{
    public class ResourceStrategy : ILocalizationStrategy
    {
        private readonly ResourceManager _resourceManager;

        private readonly string[] _contextKeys;

        private readonly int _maxKeyLength;

        private readonly Regex _keyStripper = new Regex("[^a-zA-Z0-9]");

        public ResourceStrategy() : this(GetResourceManager(), GetContextKeys(), GetMaxKeyLength()) {}

        public ResourceStrategy(ResourceManager manager, string[] contextKeys, int maxKeyLength)
        {
            _resourceManager = manager;
            _contextKeys = contextKeys;
            _maxKeyLength = maxKeyLength;
        }

        private static ResourceManager GetResourceManager()
        {
            if (!LocMvcConfiguration.Settings.ContainsKey("resource") || string.IsNullOrWhiteSpace(LocMvcConfiguration.Settings["resource"])) {
                throw new ArgumentException("Resource strategy requires a resource setting be added to the 'locmvc' configuration section.", "resource");
            }
            var resourceTypeString = LocMvcConfiguration.Settings["resource"];
            var resourceType = Type.GetType(resourceTypeString);
            if (resourceType == null) {
                throw new ArgumentException(string.Format("The resource type could not be found: {0}", resourceTypeString), "resource");
            }
            var manager = new ResourceManager(resourceType.FullName, resourceType.Assembly);
            if (manager == null) {
                throw new ArgumentException(string.Format("The resource type was invalid: {0}", resourceTypeString), "resource");
            }
            return manager;
        }

        private static string[] GetContextKeys()
        {
            if (LocMvcConfiguration.Settings.ContainsKey("contextkeys") && !string.IsNullOrWhiteSpace(LocMvcConfiguration.Settings["contextkeys"])) {
                var contextString = LocMvcConfiguration.Settings["contextkeys"];
                return contextString.Split(", ".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            return new string[0];
        }

        private static int GetMaxKeyLength()
        {
            var max = int.MaxValue;
            if (LocMvcConfiguration.Settings.ContainsKey("maxkeylength")) {
                int.TryParse(LocMvcConfiguration.Settings["maxkeylength"], out max);
            }
            return max;
        }

        public string GenerateKey(Dictionary<string, string> context, string unlocalizedText)
        {
            var keyBuilder = new StringBuilder();
            var contextValues = context.Where(kvp => _contextKeys.Contains(kvp.Key, StringComparer.InvariantCultureIgnoreCase)).ToArray();
            foreach (var value in contextValues) {
                keyBuilder.Append(string.Format("{0}_", value.Value));
            }
            keyBuilder.Append(_keyStripper.Replace(unlocalizedText, "_"));
            var key = keyBuilder.Length > _maxKeyLength
                ? keyBuilder.ToString().Substring(0, _maxKeyLength)
                : keyBuilder.ToString();
            return key;
        }

        public string GetLocalizedString(string key, string locale)
        {
            try {
                var culture = new CultureInfo(locale);
                return _resourceManager.GetString(key, culture);
            } catch (CultureNotFoundException) {
                throw new ArgumentException("Could not resolve locale to CultureInfo", "locale");
            }
        }
    }
}
