using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageInitializer : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("SelectedLocale"))
        {
            var systemLocale = LocalizationSettings.AvailableLocales.GetLocale(Application.systemLanguage);
            if (systemLocale != null)
            {
                LocalizationSettings.SelectedLocale = systemLocale;
                PlayerPrefs.SetString("SelectedLocale", systemLocale.Identifier.Code);
                PlayerPrefs.Save();
            }
        }
        else
        {
            string savedCode = PlayerPrefs.GetString("SelectedLocale");
            var savedLocale = LocalizationSettings.AvailableLocales.Locales.Find(
                l => l.Identifier.Code == savedCode);
            if (savedLocale != null)
            {
                LocalizationSettings.SelectedLocale = savedLocale; 
            }
        }
    }
}
