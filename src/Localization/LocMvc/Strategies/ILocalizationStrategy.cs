using System.Collections.Generic;

namespace LocMvc.Strategies
{
    public interface ILocalizationStrategy
    {
        string GenerateKey(Dictionary<string, string> context, string unlocalizedText);
        string GetLocalizedString(string key, string locale);
    }
}
