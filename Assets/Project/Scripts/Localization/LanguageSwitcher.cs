using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

/// <summary>
/// Handles UI language switching and flag display based on LocaleFlagConfig.
/// </summary>
public class LanguageSwitcher : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private LocaleFlagConfigSO config;

    [Header("UI References")]
    [SerializeField] private Image flagImage;        // Image showing the current locale's flag
    [SerializeField] private Button switchButton;    // Button that cycles through locales

    private int currentIndex = 0;

    private void Start()
    {
        if (config == null || config.localeFlags.Count == 0)
        {
            Debug.LogError("[LanguageSwitcher] Config is missing or contains no flags.");
            return;
        }

        // Find index of currently selected locale
        var currentLocale = LocalizationSettings.SelectedLocale;
        currentIndex = LocalizationSettings.AvailableLocales.Locales.FindIndex(
            l => l.Identifier.Code == currentLocale.Identifier.Code);

        UpdateUI();

        if (switchButton != null)
            switchButton.onClick.AddListener(SwitchToNextLocale);
    }

    /// <summary>
    /// Switches to the next locale in the list and updates UI and PlayerPrefs.
    /// </summary>
    public void SwitchToNextLocale()
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        if (locales.Count == 0) return;

        currentIndex = (currentIndex + 1) % locales.Count;

        var selected = locales[currentIndex];
        LocalizationSettings.SelectedLocale = selected;

        // Save selected locale for next launch
        PlayerPrefs.SetString("SelectedLocale", selected.Identifier.Code);
        PlayerPrefs.Save();

        UpdateUI();
    }

    /// <summary>
    /// Updates the flag image based on the currently selected locale.
    /// </summary>
    private void UpdateUI()
    {
        string currentCode = LocalizationSettings.SelectedLocale.Identifier.Code;
        var match = config.localeFlags.Find(l => l.localeCode == currentCode);

        if (match != null && flagImage != null)
        {
            flagImage.sprite = match.flagSprite;
        }
        else
        {
            Debug.LogWarning($"[LanguageSwitcher] No flag found for locale '{currentCode}'.");
        }
    }
}
