using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Abstract base class for all popups. Provides show/close logic, animation trigger support,
/// and background tint handling via BackgroundTintController.
/// </summary>
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
        // Hook up close button
        if (closeButton != null)
            closeButton.onClick.AddListener(Close);

        // Auto-assign animator if not set
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
                Debug.LogWarning($"[{name}] No Animator component assigned or found.");
        }
    }

    /// <summary>
    /// Activates the popup and shows background tint/blur via BackgroundTintController.
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);

        BackgroundTintController.Instance?.ApplyEffects();

        Debug.Log($"[{name}] Show()");
        OnShown?.Invoke();
    }

    /// <summary>
    /// Triggers the closing animation or closes immediately if no animator/trigger is set.
    /// </summary>
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

    /// <summary>
    /// Called by animation event when the closing animation finishes.
    /// </summary>
    public void OnCloseAnimationComplete()
    {
        FinalizeClose();
    }

    /// <summary>
    /// Final cleanup: invokes callback and destroys the popup GameObject.
    /// </summary>
    private void FinalizeClose()
    {
        Debug.Log($"[{name}] Destroying popup");
        OnClosed?.Invoke();
        Destroy(gameObject);
    }
}
