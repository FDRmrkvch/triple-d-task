using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages showing and closing popups dynamically using a configuration.
/// Supports singleton access and automatic cleanup of destroyed popups.
/// </summary>
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [Header("Popup Configuration")]
    [SerializeField] private PopupConfigSO config;

    [Header("Popup Parent Canvas")]
    [SerializeField] private Transform popupCanvas;

    private readonly Dictionary<string, GameObject> activePopups = new();

    private void Awake()
    {
        // Ensure singleton pattern
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Instantiates and shows a popup by name.
    /// Prevents duplicates and registers it for cleanup.
    /// </summary>
    /// <param name="name">The popup name as defined in the config.</param>
    public void ShowPopup(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("[PopupManager] Popup name is empty.");
            return;
        }

        if (activePopups.ContainsKey(name))
        {
            Debug.Log($"[PopupManager] Popup '{name}' is already open.");
            return;
        }

        var popupEntry = config.popups.Find(p => p.popupName == name);
        if (popupEntry?.popupPrefab == null)
        {
            Debug.LogError($"[PopupManager] Popup '{name}' not found or prefab is missing.");
            return;
        }

        GameObject instance = Instantiate(popupEntry.popupPrefab, popupCanvas);
        activePopups[name] = instance;

        // Call Show() if the popup implements IPopup
        if (instance.TryGetComponent(out IPopup popup))
        {
            popup.Show();
        }

        // Register for automatic removal on destruction
        var remover = instance.AddComponent<PopupAutoUnregister>();
        remover.popupName = name;
    }

    /// <summary>
    /// Closes and removes a popup by name.
    /// </summary>
    public void ClosePopup(string name)
    {
        if (activePopups.TryGetValue(name, out var instance))
        {
            Destroy(instance);
            activePopups.Remove(name);
        }
    }

    /// <summary>
    /// Closes and clears all currently active popups.
    /// </summary>
    public void CloseAllPopups()
    {
        foreach (var popup in activePopups.Values)
        {
            Destroy(popup);
        }

        activePopups.Clear();
    }
}
