using System.Collections.Generic;

namespace LocMvc
{
    public interface ILocalizationService
    {
        /// <summary>
        /// Generates a key based on context values and an unlocalized chunk of text.
        /// </summary>
        string GenerateKey(Dictionary<string, string> contextValues, string unlocalizedText);
        /// <summary>
        /// Generates a key based on an unlocalized chunk of text.
        /// </summary>
        string GenerateKey(string unlocalizedText);
        /// <summary>
        /// Gets a localized string based on a key
        /// <br />If no value is found for the key, the key will be returned unlocalized
        /// <br />Setting the originalText will alter the fall-back unlocalized value if configured to not show unlocalized keys
        /// </summary>
        string GetLocalizedString(string key, string originalText);
        /// <summary>
        /// Gets a localized string based on a key for a specific locale
        /// <br />If no value is found for the key, the key will be returned unlocalized
        /// <br />Setting the originalText will alter the fall-back unlocalized value if configured to not show unlocalized keys
        /// </summary>
        string GetLocalizedString(string key, string locale, string originalText);
    }
}
