using UnityEngine;
using System.Collections.Generic;

public class GridItem : MonoBehaviour
{
    private Animation anim;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public void PlayRandomAnimation()
    {
        if (anim == null || anim.GetClipCount() == 0) return;

        // Собираем список всех клипов из Animation компонента
        var clips = new List<AnimationState>();
        foreach (AnimationState state in anim)
        {
            clips.Add(state);
        }

        if (clips.Count == 0) return;

        var randomClip = clips[Random.Range(0, clips.Count)];
        anim.Play(randomClip.name);
    }
}