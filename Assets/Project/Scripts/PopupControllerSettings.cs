using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour, IPopup
{
    [Header("UI")]
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (closeButton == null)
        {
            Debug.LogError("[SettingsPopup] CloseButton is not assigned.");
            return;
        }

        closeButton.onClick.AddListener(Close);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        BackgroundTintController.Instance?.ApplyEffects();
    }

    public void Close()
    {
        BackgroundTintController.Instance?.ResetEffects();
        Destroy(gameObject);
    }
}
