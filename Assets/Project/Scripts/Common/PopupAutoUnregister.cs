using UnityEngine;

/// <summary>
/// Automatically unregisters a popup from the PopupManager when destroyed.
/// Useful for keeping the popup dictionary clean in case the popup is closed externally.
/// </summary>
public class PopupAutoUnregister : MonoBehaviour
{
    [Tooltip("The name of the popup as registered in PopupManager.")]
    public string popupName;

    private void OnDestroy()
    {
        // Ensure popup is removed from the manager's tracking dictionary
        if (PopupManager.Instance != null && !string.IsNullOrEmpty(popupName))
        {
            PopupManager.Instance.ClosePopup(popupName);
        }
    }
}
