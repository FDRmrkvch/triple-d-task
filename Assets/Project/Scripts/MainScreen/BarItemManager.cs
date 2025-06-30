using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the dynamic creation and logic for tab bar items based on a configuration ScriptableObject.
/// </summary>
public class BarItemManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private TabBarConfigSO config;

    [Header("Bar Item Prefab")]
    [SerializeField] private GameObject barItemPrefab;

    [Header("Bar Container")]
    [SerializeField] private Transform barContainer;

    private List<BarItemController> barItems = new();
    private BarItemController currentlySelected;

    private void Start()
    {
        if (config == null)
        {
            Debug.LogError("[BarItemManager] TabBarConfigSO is missing.");
            return;
        }

        GenerateTabsFromConfig();
    }

    /// <summary>
    /// Spawns and configures tab bar items using the config data.
    /// </summary>
    private void GenerateTabsFromConfig()
    {
        // Cleanup any existing tabs
        foreach (Transform child in barContainer)
            Destroy(child.gameObject);

        barItems.Clear();
        currentlySelected = null;

        // Generate tabs
        for (int i = 0; i < config.tabCount; i++)
        {
            TabConfig tabData = config.tabConfigs[i];

            GameObject tabGO = Instantiate(barItemPrefab, barContainer);
            if (!tabGO.TryGetComponent(out BarItemController barItem))
            {
                Debug.LogError($"[BarItemManager] Missing BarItemController on prefab at index {i}");
                continue;
            }

            // State
            BarItemController.State itemState = ConvertState(tabData.state);
            barItem.Initialize(itemState);
            barItem.OnSelected += HandleTabSelected;

            // Icon
            if (!string.IsNullOrEmpty(tabData.iconName))
            {
                Sprite iconSprite = FindIconByName(tabData.iconName);
                if (iconSprite != null)
                    barItem.SetIcon(iconSprite);
                else
                    Debug.LogWarning($"[BarItemManager] Icon '{tabData.iconName}' not found (tab {i})");
            }

            // Localization
            if (!string.IsNullOrEmpty(tabData.localizationKey))
                barItem.SetLocalizedKey(tabData.localizationKey);

            barItems.Add(barItem);

            if (tabData.state == TabState.Selected)
                currentlySelected = barItem;
        }

        // Apply selection logic
        if (currentlySelected != null)
            HandleTabSelected(currentlySelected);
    }

    /// <summary>
    /// Handles selection change when a tab is clicked.
    /// </summary>
    private void HandleTabSelected(BarItemController selected)
    {
        if (currentlySelected != null && currentlySelected != selected)
            currentlySelected.Deselect();

        currentlySelected = selected;
    }

    /// <summary>
    /// Finds icon by name from config icon list.
    /// </summary>
    private Sprite FindIconByName(string iconName)
    {
        foreach (var icon in config.icons)
        {
            if (icon.iconName == iconName)
                return icon.iconSprite;
        }
        return null;
    }

    /// <summary>
    /// Converts external TabState to internal BarItemController.State.
    /// </summary>
    private BarItemController.State ConvertState(TabState state) => state switch
    {
        TabState.Locked => BarItemController.State.Locked,
        TabState.Unlocked => BarItemController.State.Unlocked,
        TabState.Selected => BarItemController.State.Selected,
        _ => BarItemController.State.Unlocked
    };
}
