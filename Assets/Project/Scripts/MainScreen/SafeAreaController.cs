using UnityEngine;

/// <summary>
/// Adjusts the RectTransform anchors to match the device's safe area (e.g., accounting for notches, rounded corners).
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class SafeAreaController : MonoBehaviour
{
    private RectTransform _panel;                  // Cached reference to the RectTransform
    private Rect _lastSafeArea = Rect.zero;        // Last applied safe area
    private ScreenOrientation _lastOrientation;    // Last checked screen orientation

    private void Awake()
    {
        _panel = GetComponent<RectTransform>();
        _lastOrientation = Screen.orientation;
        ApplySafeArea();
    }

    private void Update()
    {
        // Re-apply if safe area or orientation has changed
        if (_lastSafeArea != Screen.safeArea || _lastOrientation != Screen.orientation)
        {
            ApplySafeArea();
        }
    }

    /// <summary>
    /// Applies the current screen safe area to the RectTransform anchors.
    /// </summary>
    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        // Convert safe area rect (in pixels) to normalized anchor values (0 to 1)
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // Apply calculated anchors to the panel
        _panel.anchorMin = anchorMin;
        _panel.anchorMax = anchorMax;

        // Cache for next update check
        _lastSafeArea = safeArea;
        _lastOrientation = Screen.orientation;
    }
}
