using UnityEngine;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [Header("Popup Config")]
    public PopupConfigSO config;

    [Header("Popup Parent Canvas")]
    public Transform popupCanvas;

    private Dictionary<string, GameObject> activePopups = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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
        if (popupEntry == null || popupEntry.popupPrefab == null)
        {
            Debug.LogError($"[PopupManager] Popup '{name}' not found in config.");
            return;
        }

        var instance = Instantiate(popupEntry.popupPrefab, popupCanvas);
        activePopups[name] = instance;

        // Auto-register close if IPopup
        if (instance.TryGetComponent(out IPopup popup))
        {
            popup.Show();
        }

        // Optional: auto-remove from dictionary on destroy
        var remover = instance.AddComponent<PopupAutoUnregister>();
        remover.popupName = name;
    }

    public void ClosePopup(string name)
    {
        if (activePopups.TryGetValue(name, out var instance))
        {
            Destroy(instance);
            activePopups.Remove(name);
        }
    }

    public void CloseAllPopups()
    {
        foreach (var popup in activePopups.Values)
        {
            Destroy(popup);
        }
        activePopups.Clear();
    }
}
