using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject storing mapping between locale codes and flag sprites.
/// Used for displaying the correct flag for each supported language.
/// </summary>
[CreateAssetMenu(fileName = "LocalizationConfig", menuName = "UI/Localization Config", order = 3)]
public class LocaleFlagConfigSO : ScriptableObject
{
    [System.Serializable]
    public class LocaleFlag
    {
        public string localeCode;   // Example: "en", "pl", "de"
        public Sprite flagSprite;   // Corresponding flag sprite
    }

    [Tooltip("List of available locales and their corresponding flag sprites.")]
    public List<LocaleFlag> localeFlags = new();
}
