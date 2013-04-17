using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.ComponentModel;
using LocMvc.Strategies;

namespace LocMvc.Configuration
{
    public class LocMvcConfiguration : ConfigurationSection
    {
        private static LocMvcConfiguration _current;
        /// <summary>
        /// The current LocMvcConfiguration
        /// </summary>
        public static LocMvcConfiguration Current { get { return _current ?? (_current = ConfigurationManager.GetSection("locmvc") as LocMvcConfiguration); } }

        private static ILocalizationStrategy _strategy;
        /// <summary>
        /// The currently configured strategy
        /// </summary>
        public static ILocalizationStrategy Strategy { get { return _strategy ?? (_strategy = Activator.CreateInstance(Current.StrategyType) as ILocalizationStrategy); } }

        private static string _locale;
        /// <summary>
        /// The currently configured locale
        /// </summary>
        public static string Locale { get { return _locale ?? (_locale = Current.LocaleValue); } }

        private static bool? _displayKeys;
        /// <summary>
        /// Specifies if the ungenerated
        /// </summary>
        public static bool DisplayKeys { get { return (bool)(_displayKeys ?? (_displayKeys = Current.DisplayUnlocalizedKeys)); } }

        private static Dictionary<string, string> _settings;
        public static Dictionary<string, string> Settings { get { return _settings ?? (_settings = Current.SettingValues.OfType<LocMvcConfigurationSetting>().ToDictionary(s => s.Name.ToLowerInvariant(), s => s.Value)); } }
            
        [ConfigurationProperty("strategy", IsRequired = true)]
        [TypeConverter(typeof(TypeNameConverter))]
        public Type StrategyType { get { return (Type)base["strategy"]; } set { base["strategy"] = value; } }

        [ConfigurationProperty("locale", IsRequired = true)]
        public string LocaleValue { get { return (string)base["locale"]; } set { base["locale"] = value; } }

        [ConfigurationProperty("displayUnlocalizedKeys", IsRequired = false, DefaultValue = "true")]
        public bool DisplayUnlocalizedKeys { get { return (bool)base["displayUnlocalizedKeys"]; } set { base["displayUnlocalizedKeys"] = value; } }

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        [ConfigurationCollection(typeof(LocMvcConfigurationSettingsCollection), AddItemName="add")]
        public LocMvcConfigurationSettingsCollection SettingValues { get { return (LocMvcConfigurationSettingsCollection)base[""]; } }
    }

    public class LocMvcConfigurationSettingsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LocMvcConfigurationSetting();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var setting = element as LocMvcConfigurationSetting;
            if (setting == null) return element;
            return setting.Name;
        }
    }

    public class LocMvcConfigurationSetting : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name { get { return (string)base["name"]; } set { base["name"] = value; } }

        [ConfigurationProperty("value", IsKey = false, IsRequired = true)]
        public string Value { get { return (string)base["value"]; } set { base["value"] = value; } }
    }
}
