using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Collections;

public class BackgroundTintController : MonoBehaviour
{
    public static BackgroundTintController Instance { get; private set; }

    [Header("Blur Volume")]
    [SerializeField] private Volume blurVolume;
    [SerializeField] private float targetBlurWeight = 1f;

    [Header("Dimmer Image")]
    [SerializeField] private Image dimmerImage;
    [Tooltip("0–100 percent")] public float targetDimmerOpacityPercent = 80f;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 0.3f;
    [SerializeField] private AnimationCurve slideEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine transitionCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("[BackgroundTintController] Duplicate instance found, destroying.");
            Destroy(gameObject);
        }
    }

    public void ApplyEffects()
    {
        float targetAlpha = Mathf.Clamp01(targetDimmerOpacityPercent / 100f);
        StartTransition(targetBlurWeight, targetAlpha);
    }

    public void ResetEffects()
    {
        StartTransition(0f, 0f);
    }

    private void StartTransition(float toWeight, float toAlpha)
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(TransitionRoutine(toWeight, toAlpha));
    }

    private IEnumerator TransitionRoutine(float toWeight, float toAlpha)
    {
        float fromWeight = blurVolume != null ? blurVolume.weight : 0f;
        float fromAlpha = dimmerImage != null ? dimmerImage.color.a : 0f;
        float time = 0f;

        while (time < transitionDuration)
        {
            float t01 = Mathf.Clamp01(time / transitionDuration);
            float t = slideEase.Evaluate(t01);

            if (blurVolume != null)
                blurVolume.weight = Mathf.Lerp(fromWeight, toWeight, t);

            if (dimmerImage != null)
            {
                Color c = dimmerImage.color;
                c.a = Mathf.Lerp(fromAlpha, toAlpha, t);
                dimmerImage.color = c;
            }

            time += Time.deltaTime;
            yield return null;
        }

        if (blurVolume != null)
            blurVolume.weight = toWeight;

        if (dimmerImage != null)
        {
            Color c = dimmerImage.color;
            c.a = toAlpha;
            dimmerImage.color = c;
        }

        transitionCoroutine = null;
    }
}
