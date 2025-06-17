using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CustomToggle : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public RectTransform handle;
    public GameObject backgroundDisabled;
    public GameObject backgroundActive;

    [Header("Animation")]
    public float animationDuration = 0.25f;
    public AnimationCurve slideEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Handle Movement")]
    public Vector2 handleOffPosition;
    public Vector2 handleOnPosition;

    [Header("Events")]
    public UnityEvent OnToggleOn;
    public UnityEvent OnToggleOff;

    private bool isOn = false;
    private Coroutine moveRoutine;

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    public void Toggle()
    {
        isOn = !isOn;

        backgroundActive.SetActive(isOn);
        backgroundDisabled.SetActive(!isOn);

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(AnimateHandle(isOn));

        if (isOn) OnToggleOn?.Invoke();
        else OnToggleOff?.Invoke();
    }

    private IEnumerator AnimateHandle(bool turnOn)
    {
        Vector2 startPos = handle.anchoredPosition;
        Vector2 targetPos = turnOn ? handleOnPosition : handleOffPosition;

        float time = 0f;
        while (time < animationDuration)
        {
            float t = slideEase.Evaluate(time / animationDuration);
            handle.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            time += Time.deltaTime;
            yield return null;
        }

        handle.anchoredPosition = targetPos;
    }

    // Вызов из других скриптов
    public void SetState(bool value, bool instant = false)
    {
        isOn = value;
        backgroundActive.SetActive(isOn);
        backgroundDisabled.SetActive(!isOn);

        if (instant)
        {
            handle.anchoredPosition = isOn ? handleOnPosition : handleOffPosition;
        }
        else
        {
            if (moveRoutine != null)
                StopCoroutine(moveRoutine);
            moveRoutine = StartCoroutine(AnimateHandle(isOn));
        }
    }

    public bool GetState()
    {
        return isOn;
    }
}
