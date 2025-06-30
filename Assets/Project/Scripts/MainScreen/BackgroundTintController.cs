using UnityEngine;

/// <summary>
/// Controls background tint visual effects using legacy Animation component.
/// Handles show/hide transitions via predefined animation clips.
/// </summary>
public class BackgroundTintController : MonoBehaviour
{
    public static BackgroundTintController Instance { get; private set; }

    [Header("Animation Component")]
    [SerializeField] private Animation backgroundAnimation;

    [Header("Clip Names")]
    [SerializeField] private string showClipName = "BackgroundTint_Show";
    [SerializeField] private string hideClipName = "BackgroundTint_Hide";

    private void Awake()
    {
        // Ensure singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("[BackgroundTintController] Duplicate instance found. Destroying this one.");
            Destroy(gameObject);
            return;
        }

        // Try to get the Animation component if not assigned
        if (backgroundAnimation == null)
        {
            backgroundAnimation = GetComponent<Animation>();
            if (backgroundAnimation == null)
            {
                Debug.LogWarning("[BackgroundTintController] No Animation component assigned or found.");
            }
        }
    }

    /// <summary>
    /// Plays the show animation (e.g., fade in + blur).
    /// </summary>
    public void ApplyEffects()
    {
        PlayClip(showClipName);
    }

    /// <summary>
    /// Plays the hide animation (e.g., fade out + remove blur).
    /// </summary>
    public void ResetEffects()
    {
        PlayClip(hideClipName);
    }

    /// <summary>
    /// Helper method to play a clip by name if it exists.
    /// </summary>
    private void PlayClip(string clipName)
    {
        if (backgroundAnimation == null)
        {
            Debug.LogWarning("[BackgroundTintController] Missing Animation component.");
            return;
        }

        AnimationClip clip = backgroundAnimation.GetClip(clipName);
        if (clip != null)
        {
            backgroundAnimation.Play(clip.name);
        }
        else
        {
            Debug.LogWarning($"[BackgroundTintController] Animation clip '{clipName}' not found.");
        }
    }
}
