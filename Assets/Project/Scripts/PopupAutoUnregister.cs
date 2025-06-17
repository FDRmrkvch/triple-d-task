using UnityEngine;

public class PopupAutoUnregister : MonoBehaviour
{
    public string popupName;

    private void OnDestroy()
    {
        if (PopupManager.Instance != null && !string.IsNullOrEmpty(popupName))
        {
            PopupManager.Instance.ClosePopup(popupName);
        }
    }
}