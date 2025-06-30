using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

/// <summary>
/// Initializes the game's language on startup based on PlayerPrefs or system language.
/// </summary>
public class LanguageInitializer : MonoBehaviour
{
    private const string LocaleKey = "SelectedLocale";

    private void Awake()
    {
        // If there's no saved locale, try to use the system language
        if (!PlayerPrefs.HasKey(LocaleKey))
        {
            // Try to get a locale that matches the system language
            var systemLocale = LocalizationSettings.AvailableLocales.GetLocale(Application.systemLanguage);
            if (systemLocale != null)
            {
                LocalizationSettings.SelectedLocale = systemLocale;

                // Save the selected locale to PlayerPrefs for next time
                PlayerPrefs.SetString(LocaleKey, systemLocale.Identifier.Code);
                PlayerPrefs.Save();
            }
        }
        else
        {
            // Load the previously selected locale from PlayerPrefs
            string savedCode = PlayerPrefs.GetString(LocaleKey);

            var savedLocale = LocalizationSettings.AvailableLocales.Locales.Find(
                l => l.Identifier.Code == savedCode);

            if (savedLocale != null)
            {
                LocalizationSettings.SelectedLocale = savedLocale;
            }
        }
    }
}
