using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LocalizationConfig", menuName = "UI/Localization Config", order = 3)]
public class LocaleFlagConfigSO : ScriptableObject
{
    [System.Serializable]
    public class LocaleFlag
    {
        public string localeCode; // "en", "pl", etc.
        public Sprite flagSprite;
    }

    public List<LocaleFlag> localeFlags = new();
}

public class LanguageSwitcher : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private LocaleFlagConfigSO config;

    [Header("UI References")]
    [SerializeField] private Image flagImage;
    [SerializeField] private Button switchButton;

    private int currentIndex = 0;

    private void Start()
    {
        if (config == null || config.localeFlags.Count == 0)
        {
            Debug.LogError("[LanguageSwitcher] LocaleFlagConfig is missing or empty.");
            return;
        }

        var currentLocale = LocalizationSettings.SelectedLocale;
        currentIndex = LocalizationSettings.AvailableLocales.Locales.FindIndex(
            l => l.Identifier.Code == currentLocale.Identifier.Code);

        UpdateUI();

        if (switchButton != null)
        {
            switchButton.onClick.AddListener(SwitchToNextLocale);
        }
    }

    public void SwitchToNextLocale()
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        if (locales.Count == 0) return;

        currentIndex = (currentIndex + 1) % locales.Count;
        LocalizationSettings.SelectedLocale = locales[currentIndex];
        PlayerPrefs.SetString("SelectedLocale", locales[currentIndex].Identifier.Code);
        PlayerPrefs.Save();

        UpdateUI();
    }

    private void UpdateUI()
    {
        string currentCode = LocalizationSettings.SelectedLocale.Identifier.Code;
        var match = config.localeFlags.Find(l => l.localeCode == currentCode);

        if (match != null && flagImage != null)
        {
            flagImage.sprite = match.flagSprite;
        }
    }
}
