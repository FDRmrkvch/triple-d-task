using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using System;

/// <summary>
/// UI controller for a single tab bar item with state management, icon, localization, and animation handling.
/// </summary>
[RequireComponent(typeof(Animator), typeof(Button))]
public class BarItemController : MonoBehaviour
{
    public enum State { Locked, Unlocked, Selected }

    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private LocalizeStringEvent localizeStringEvent;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Button")]
    [SerializeField] private Button button;

    [Header("Animator Triggers")]
    [SerializeField] private string trigger_LockedPressed = "Locked_Pressed";
    [SerializeField] private string trigger_SelectedPressed = "Selected_Pressed";
    [SerializeField] private string trigger_Select = "Select";
    [SerializeField] private string trigger_Unselect = "Unselect";

    [Header("Animator States")]
    [SerializeField] private string state_Locked = "Locked";
    [SerializeField] private string state_Unlocked = "Unlocked";
    [SerializeField] private string state_Selected = "Selected";

    public System.Action<BarItemController> OnSelected;

    private State currentState;

    private void Awake()
    {
        // Auto-assign required references if not set
        animator ??= GetComponent<Animator>();
        button ??= GetComponent<Button>();

        // Add click listener
        button.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// Initializes the item with a given visual state.
    /// </summary>
    public void Initialize(State state)
    {
        currentState = state;
        string stateName = GetStateName(state);

        if (!string.IsNullOrEmpty(stateName))
        {
            animator.Play(stateName, 0, 1f); // Set to exact state without playing transition
            Debug.Log($"[BarItem] Initialized with state: {state}");
        }
        else
        {
            Debug.LogWarning($"[BarItem] Unknown state: {state}");
        }
    }

    /// <summary>
    /// Sets the icon sprite for the tab item.
    /// </summary>
    public void SetIcon(Sprite sprite)
    {
        if (iconImage && sprite)
        {
            iconImage.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("[BarItem] Icon or sprite is missing.");
        }
    }

    /// <summary>
    /// Updates the localized string based on key.
    /// </summary>
    public void SetLocalizedKey(string localizationKey)
    {
        if (localizeStringEvent)
        {
            localizeStringEvent.StringReference.TableEntryReference = localizationKey;
            localizeStringEvent.RefreshString();
        }
        else
        {
            Debug.LogWarning("[BarItem] Missing LocalizeStringEvent reference.");
        }
    }

    /// <summary>
    /// Handles button click and state transition logic.
    /// </summary>
    private void OnClick()
    {
        switch (currentState)
        {
            case State.Locked:
                animator.SetTrigger(trigger_LockedPressed);
                break;

            case State.Unlocked:
                animator.SetTrigger(trigger_Select);
                currentState = State.Selected;
                OnSelected?.Invoke(this);
                break;

            case State.Selected:
                animator.SetTrigger(trigger_SelectedPressed);
                break;
        }
    }

    /// <summary>
    /// Deselects the item and returns it to the unlocked state.
    /// </summary>
    public void Deselect()
    {
        animator.SetTrigger(trigger_Unselect);
        currentState = State.Unlocked;
    }

    /// <summary>
    /// Returns the animator state name based on internal enum.
    /// </summary>
    private string GetStateName(State state) => state switch
    {
        State.Locked => state_Locked,
        State.Unlocked => state_Unlocked,
        State.Selected => state_Selected,
        _ => null
    };
}
