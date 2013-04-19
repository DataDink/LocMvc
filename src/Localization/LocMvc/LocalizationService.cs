using System;
using System.Collections.Generic;
using LocMvc.Configuration;
using LocMvc.Strategies;

namespace LocMvc
{
    public class LocalizationService : ILocalizationService
    {
        private static ILocalizationService _configured;
        public static ILocalizationService Configured
        {
            get
            {
                try {
                    return _configured ?? (_configured = new LocalizationService(LocMvcConfiguration.Strategy, LocMvcConfiguration.Locale, LocMvcConfiguration.DisplayKeys));
                } catch {
                    return null;
                }
            }
        }

        private readonly ILocalizationStrategy _strategy;
        private readonly string _defaultLocale;
        private readonly bool _displayKeys;

        public LocalizationService(ILocalizationStrategy strategy, string defaultLocale, bool displayUnlocalizedKeys)
        {
            if (strategy == null) throw new ArgumentNullException("strategy");
            if (defaultLocale == null) throw new ArgumentNullException("defaultLocale");
            _strategy = strategy;
            _defaultLocale = defaultLocale;
            _displayKeys = displayUnlocalizedKeys;
        }

        public string GenerateKey(Dictionary<string, string> contextValues, string unlocalizedText)
        {
            return _strategy.GenerateKey(contextValues ?? new Dictionary<string, string>(), unlocalizedText);
        }

        public string GenerateKey(string unlocalizedText)
        {
            return _strategy.GenerateKey(new Dictionary<string, string>(), unlocalizedText);
        }

        public string GetLocalizedString(string key, string unlocalizedText)
        {
            return GetLocalizedString(key, _defaultLocale, unlocalizedText);
        }

        public string GetLocalizedString(string key, string locale, string unlocalizedText)
        {
            var text = _strategy.GetLocalizedString(key, locale ?? _defaultLocale);
            var fallbackText = _displayKeys
                ? string.Format("(UNLOCALIZED){0}", key)
                : unlocalizedText ?? key;
            return text ?? fallbackText;
        }
    }
}
