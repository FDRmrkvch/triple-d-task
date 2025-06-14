using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaController : MonoBehaviour
{
    private RectTransform _panel;
    private Rect _lastSafeArea = new Rect(0, 0, 0, 0);
    private ScreenOrientation _lastOrientation = ScreenOrientation.AutoRotation;

    void Awake()
    {
        _panel = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void Update()
    {
        if (_lastSafeArea != Screen.safeArea || _lastOrientation != Screen.orientation)
            ApplySafeArea();
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _panel.anchorMin = anchorMin;
        _panel.anchorMax = anchorMax;

        _lastSafeArea = safeArea;
        _lastOrientation = Screen.orientation;
    }
}
