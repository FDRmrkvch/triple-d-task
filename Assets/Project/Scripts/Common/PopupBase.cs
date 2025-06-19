using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class BasePopup : MonoBehaviour, IPopup
{
    [Header("Base Popup")]
    [SerializeField] protected Button closeButton;
    [SerializeField] protected Animator animator;
    [SerializeField] protected string closeTrigger = "PlayOut";

    protected bool isClosing = false;

    public event Action OnShown;
    public event Action OnClosed;

    protected virtual void Awake()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(Close);

        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
                Debug.LogWarning($"[{name}] Animator not assigned or found.");
        }
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        BackgroundTintController.Instance?.ApplyEffects();

        Debug.Log($"[{name}] Show()");
        OnShown?.Invoke();
    }

    public virtual void Close()
    {
        if (isClosing) return;
        isClosing = true;

        Debug.Log($"[{name}] Close()");
        BackgroundTintController.Instance?.ResetEffects();

        if (animator != null && !string.IsNullOrEmpty(closeTrigger))
        {
            animator.SetTrigger(closeTrigger);
        }
        else
        {
            FinalizeClose();
        }
    }

    // Вызовется из Animation Event
    public void OnCloseAnimationComplete()
    {
        FinalizeClose();
    }

    private void FinalizeClose()
    {
        Debug.Log($"[{name}] Destroying popup");
        OnClosed?.Invoke();
        Destroy(gameObject);
    }
}
