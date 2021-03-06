﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        private readonly Regex _keyStripper = new Regex("\\s+|[^a-zA-Z0-9]");

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
            var resourceTypeString = LocMvcConfiguration.Settings["resource"].Split(",".ToArray(), 2, StringSplitOptions.RemoveEmptyEntries);
            if (resourceTypeString.Length < 2) {
                throw new ArgumentException("Please configure the 'resource' string as \"[namespace].[resource], [assembly]\"");
            }
            var assembly = Assembly.Load(resourceTypeString[1].Trim());
            var manager = new ResourceManager(resourceTypeString[0], assembly);
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

        public virtual string GenerateKey(Dictionary<string, string> context, string unlocalizedText)
        {
            var keyBuilder = new StringBuilder();
            var contextValues = context.Where(kvp => _contextKeys.Contains(kvp.Key, StringComparer.InvariantCultureIgnoreCase)).ToArray();
            foreach (var value in contextValues) {
                keyBuilder.Append(string.Format("{0}_", value.Value));
            }
            keyBuilder.Append(unlocalizedText);
            var key = _keyStripper.Replace(keyBuilder.ToString(), "_");
            key = key.Length > _maxKeyLength
                ? key.Substring(0, _maxKeyLength)
                : key;
            return key;
        }

        public virtual string GetLocalizedString(string key, string locale)
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
