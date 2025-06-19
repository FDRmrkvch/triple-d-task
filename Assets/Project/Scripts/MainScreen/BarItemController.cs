using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using System;

[RequireComponent(typeof(Animator), typeof(Button))]
public class BarItemController : MonoBehaviour
{
    public enum State { Locked, Unlocked, Selected }

    [Header("UI")]
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

    public Action<BarItemController> OnSelected;

    private State currentState;

    private void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!button) button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);
    }

    public void Initialize(State state)
    {
        currentState = state;
        string stateName = GetStateName(state);

        if (!string.IsNullOrEmpty(stateName))
        {
            animator.Play(stateName, 0, 1f);
            Debug.Log($"[BarItem] Initialized with state: {state}");
        }
        else
        {
            Debug.LogWarning($"[BarItem] Unknown state: {state}");
        }
    }

    public void SetIcon(Sprite sprite)
    {
        if (iconImage && sprite)
        {
            iconImage.sprite = sprite;
            Debug.Log($"[BarItem] Icon set to: {sprite.name}");
        }
        else
        {
            Debug.LogWarning("[BarItem] Icon not set: iconImage or sprite is null.");
        }
    }

    public void SetLocalizedKey(string localizationKey)
    {
        if (localizeStringEvent != null)
        {
            localizeStringEvent.StringReference.TableEntryReference = localizationKey;
            localizeStringEvent.RefreshString();
            Debug.Log($"[BarItem] Localized string key set to: {localizationKey}");
        }
        else
        {
            Debug.LogWarning("[BarItem] LocalizeStringEvent reference is missing.");
        }
    }

    private void OnClick()
    {
        Debug.Log($"[BarItem] Clicked — current state: {currentState}");

        switch (currentState)
        {
            case State.Locked:
                animator.SetTrigger(trigger_LockedPressed);
                break;

            case State.Unlocked:
                OnSelected?.Invoke(this);
                animator.SetTrigger(trigger_Select);
                currentState = State.Selected;
                break;

            case State.Selected:
                animator.SetTrigger(trigger_SelectedPressed);
                break;
        }
    }

    public void Deselect()
    {
        Debug.Log("[BarItem] Deselecting...");
        animator.SetTrigger(trigger_Unselect);
        currentState = State.Unlocked;
    }

    private string GetStateName(State state) => state switch
    {
        State.Locked => state_Locked,
        State.Unlocked => state_Unlocked,
        State.Selected => state_Selected,
        _ => null
    };
}
