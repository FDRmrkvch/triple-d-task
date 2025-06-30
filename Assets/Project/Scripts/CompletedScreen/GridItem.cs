using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Component responsible for playing a random legacy animation.
/// Requires an Animation component with multiple AnimationClips.
/// </summary>
[RequireComponent(typeof(Animation))]
public class GridItem : MonoBehaviour
{
    private Animation anim;

    private void Awake()
    {
        // Cache the Animation component
        anim = GetComponent<Animation>();
    }

    /// <summary>
    /// Plays a random animation clip from the Animation component.
    /// </summary>
    public void PlayRandomAnimation()
    {
        if (anim == null) return;

        List<AnimationState> clips = new();

        // Gather all available AnimationStates (clips)
        foreach (AnimationState state in anim)
        {
            clips.Add(state);
        }

        if (clips.Count == 0) return;

        // Choose and play one randomly
        var randomClip = clips[Random.Range(0, clips.Count)];
        anim.Play(randomClip.name);
    }
}
