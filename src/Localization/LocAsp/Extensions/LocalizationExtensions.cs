using System;
using System.Collections.Generic;
using System.Linq;
using LocMvc;
using System.Xml.Linq;

namespace LocMvc.Extensions
{
    public static class LocalizationExtensions
    {
        public static string Localize(this string unlocalizedString, ILocalizationService service, string locale, Dictionary<string, string> context)
        {
            var key = service.GenerateKey(context, unlocalizedString);
            return service.GetLocalizedString(key, locale, unlocalizedString);
        }

        public static string Localize(this string unlocalizedString, ILocalizationService service, string locale, string context)
        {
            var key = service.GenerateKey(new Dictionary<string, string> { { "contextname", context } }, locale);
            return service.GetLocalizedString(key, locale, unlocalizedString);
        }

        public static string Localize(this string unlocalizedString, ILocalizationService service, string locale)
        {
            var key = service.GenerateKey(unlocalizedString);
            return service.GetLocalizedString(key, locale, unlocalizedString);
        }

        public static string Localize(this string unlocalizedString, ILocalizationService service, Dictionary<string, string> context)
        {
            var key = service.GenerateKey(context, unlocalizedString);
            return service.GetLocalizedString(key, unlocalizedString);
        }

        public static string Localize(this string unlocalizedString, ILocalizationService service)
        {
            var key = service.GenerateKey(unlocalizedString);
            return service.GetLocalizedString(key, unlocalizedString);
        }

        public static string GetLocalizedValue(this string key, ILocalizationService service, string locale)
        {
            return service.GetLocalizedString(key, locale, null);
        }

        public static string GetLocalizedValue(this string key, ILocalizationService service)
        {
            return service.GetLocalizedString(key, null);
        }

        public static string Localize(this Enum value, ILocalizationService service, string locale, Dictionary<string, string> context)
        {
            if (!context.ContainsKey("contexttype"))
                context.Add("contexttype", "ENUM");
            if (!context.ContainsKey("contextname"))
                context.Add("contextname", value.GetType().Name);

            var key = service.GenerateKey(context, value.ToString());
            return service.GetLocalizedString(key, locale, value.ToString());
        }

        public static string Localize(this Enum value, ILocalizationService service, string locale)
        {
            var context = new Dictionary<string, string> {
                {"contexttype", "ENUM"},
                {"contextname", value.GetType().Name}
            };

            var key = service.GenerateKey(context, value.ToString());
            return service.GetLocalizedString(key, locale, value.ToString());
        }

        public static string Localize(this Enum value, ILocalizationService service, Dictionary<string, string> context)
        {
            if (!context.ContainsKey("contexttype"))
                context.Add("contexttype", "ENUM");
            if (!context.ContainsKey("contextname"))
                context.Add("contextname", value.GetType().Name);

            var key = service.GenerateKey(context, value.ToString());
            return service.GetLocalizedString(key, value.ToString());
        }

        public static string Localize(this Enum value, ILocalizationService service)
        {
            var context = new Dictionary<string, string> {
                {"contexttype", "ENUM"},
                {"contextname", value.GetType().Name}
            };

            var key = service.GenerateKey(context, value.ToString());
            return service.GetLocalizedString(key, value.ToString());
        }
    }
}

namespace System
{
    public static class LocalizationExtensions
    {
        private static readonly ILocalizationService Service = LocalizationService.Configured;

        /// <summary>
        /// Localizes a string to the specified locale using a generated contextual key
        /// </summary>
        /// <param name="unlocalizedString">The string to localize</param>
        /// <param name="locale">The locale to localize to</param>
        /// <param name="context">A collection of values to base a generated key on</param>
        /// <returns>The localized string or the key if no string was found (based on configuration)</returns>
        public static string Localize(this string unlocalizedString, string locale, Dictionary<string, string> context)
        {
            return LocMvc.Extensions.LocalizationExtensions.Localize(unlocalizedString, Service, locale, context);
        }

        /// <summary>
        /// Localizes a string to the specified locale using a generated contextual key
        /// </summary>
        /// <param name="unlocalizedString">The string to localize</param>
        /// <param name="locale">The locale to localize to</param>
        /// <param name="context">A collection of values to base a generated key on</param>
        /// <returns>The localized string or the key if no string was found (based on configuration)</returns>
        public static string Localize(this string unlocalizedString, string locale, string context)
        {
            return LocMvc.Extensions.LocalizationExtensions.Localize(unlocalizedString, Service, locale, context);
        }

        /// <summary>
        /// Localizes a string to the specified locale using a generated contextual key
        /// </summary>
        /// <param name="unlocalizedString">The string to localize</param>
        /// <param name="locale">The locale to localize to</param>
        /// <returns>The localized string or the key if no string was found (based on configuration)</returns>
        public static string Localize(this string unlocalizedString, string locale)
        {
            return LocMvc.Extensions.LocalizationExtensions.Localize(unlocalizedString, Service, locale);
        }

        /// <summary>
        /// Localizes a string using a generated contextual key
        /// </summary>
        /// <param name="unlocalizedString">The string to localize</param>
        /// <param name="context">A collection of values to base a generated key on</param>
        /// <returns>The localized string or the key if no string was found (based on configuration)</returns>
        public static string Localize(this string unlocalizedString, Dictionary<string, string> context)
        {
            return LocMvc.Extensions.LocalizationExtensions.Localize(unlocalizedString, Service, context);
        }

        /// <summary>
        /// Localizes a string using a generated contextual key
        /// </summary>
        /// <param name="unlocalizedString">The string to localize</param>
        /// <returns>The localized string or the key if no string was found (based on configuration)</returns>
        public static string Localize(this string unlocalizedString)
        {
            return LocMvc.Extensions.LocalizationExtensions.Localize(unlocalizedString, Service);
        }

        /// <summary>
        /// Gets the localized value for the specified key and locale
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="locale">The locale</param>
        /// <returns>The localized string or the key if no string was found (based on configuration)</returns>
        public static string GetLocalizedValue(this string key, string locale)
        {
            return LocMvc.Extensions.LocalizationExtensions.GetLocalizedValue(key, Service, locale);
        }


        /// <summary>
        /// Gets the localized value for the specified key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The localized string or the key if no string was found (based on configuration)</returns>
        public static string GetLocalizedValue(this string key)
        {
            return LocMvc.Extensions.LocalizationExtensions.GetLocalizedValue(key, Service);
        }

        /// <summary>
        /// Gets the localized string for the enum value
        /// </summary>
        /// <param name="value">The value to localize</param>
        /// <param name="locale">The locale to localize to</param>
        /// <param name="context">A collection of values to base a generated key on</param>
        /// <returns>The localized string or the key if no string was found (based on configuration)</returns>
        public static string Localize(this Enum value, string locale, Dictionary<string, string> context)
        {
            return LocMvc.Extensions.LocalizationExtensions.Localize(value, Service, locale, context);
        }

        /// <summary>
        /// Gets the localized string for the enum value
        /// </summary>
        /// <param name="value">The value to localize</param>
        /// <param name="locale">The locale to localize to</param>
        /// <returns>The localized string or the key if no string was found (based on configuration)</returns>
        public static string Localize(this Enum value, string locale)
        {
            return LocMvc.Extensions.LocalizationExtensions.Localize(value, Service, locale);
        }

        /// <summary>
        /// Gets the localized string for the enum value
        /// </summary>
        /// <param name="value">The value to localize</param>
        /// <param name="context">A collection of values to base a generated key on</param>
        /// <returns>The localized string or the key if no string was found (based on configuration)</returns>
        public static string Localize(this Enum value, Dictionary<string, string> context)
        {
            return LocMvc.Extensions.LocalizationExtensions.Localize(value, Service, context);
        }

        /// <summary>
        /// Gets the localized string for the enum value
        /// </summary>
        /// <param name="value">The value to localize</param>
        /// <returns>The localized string or the key if no string was found (based on configuration)</returns>
        public static string Localize(this Enum value)
        {
            return LocMvc.Extensions.LocalizationExtensions.Localize(value, Service);
        }
    }
}
