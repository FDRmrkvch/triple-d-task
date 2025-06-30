using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button), typeof(Animator))]
public class CustomToggleWithAnimator : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private string toggleName; // Unique key for SO and PlayerPrefs
    [SerializeField] private ToggleDataSO toggleData; // Reference to SO with default states

    private Animator animator;
    private Button button;
    private bool isOn = false;

    private const string TriggerOn = "On";
    private const string TriggerOff = "Off";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(ToggleState);

        LoadInitialState();
    }

    // Loads initial state from PlayerPrefs or default from SO
    private void LoadInitialState()
    {
        if (PlayerPrefs.HasKey(toggleName))
        {
            isOn = PlayerPrefs.GetInt(toggleName) == 1;
        }
        else if (toggleData != null)
        {
            var entry = toggleData.entries.Find(e => e.name == toggleName);
            if (entry != null)
                isOn = entry.defaultState;
        }

        ApplyState(isOn, instant: true);
    }

    // Toggles the state and saves to PlayerPrefs
    private void ToggleState()
    {
        isOn = !isOn;
        ApplyState(isOn);
        PlayerPrefs.SetInt(toggleName, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Applies animator trigger based on state
    private void ApplyState(bool state, bool instant = false)
    {
        if (animator == null) return;

        string targetState = state ? "On" : "Off";
        string trigger = state ? TriggerOn : TriggerOff;

        if (instant)
        {
            animator.Play(targetState, 0, 1f); // Jump to frame 1 of target state
        }
        else
        {
            animator.SetTrigger(trigger); // Use transition
        }
    }

    // Optional external setter
    public void SetState(bool value, bool save = true)
    {
        isOn = value;
        ApplyState(isOn);

        if (save)
        {
            PlayerPrefs.SetInt(toggleName, isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool GetState() => isOn;
}
