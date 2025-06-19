using UnityEngine;
using System.Collections.Generic;

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
            Debug.LogError("[BarItemManager] Config is missing!");
            return;
        }

        GenerateTabsFromConfig();
    }

    private void GenerateTabsFromConfig()
    {
        Debug.Log($"[BarItemManager] Generating {config.tabCount} tabs...");

        // Clear previous
        foreach (Transform child in barContainer)
            Destroy(child.gameObject);
        barItems.Clear();
        currentlySelected = null;

        // Instantiate tabs
        for (int i = 0; i < config.tabCount; i++)
        {
            var tabData = config.tabConfigs[i];
            GameObject tabGO = Instantiate(barItemPrefab, barContainer);
            BarItemController barItem = tabGO.GetComponent<BarItemController>();

            if (barItem == null)
            {
                Debug.LogError($"[BarItemManager] BarItemController not found on tab prefab (index {i})");
                continue;
            }

            // Set state
            BarItemController.State itemState = ConvertState(tabData.state);
            barItem.Initialize(itemState);
            barItem.OnSelected += HandleTabSelected;

            // Set icon
            if (string.IsNullOrEmpty(tabData.iconName))
            {
                if (tabData.state != TabState.Locked)
                {
                    Debug.LogWarning($"[BarItemManager] Tab {i} has empty icon name but state is '{tabData.state}'. Icon likely missing.");
                }
                else
                {
                    Debug.Log($"[BarItemManager] Tab {i} is Locked and has no icon assigned — skipping icon setup.");
                }
            }
            else
            {
                Sprite iconSprite = FindIconByName(tabData.iconName);
                if (iconSprite != null)
                {
                    barItem.SetIcon(iconSprite);
                }
                else
                {
                    Debug.LogWarning($"[BarItemManager] Icon not found for name '{tabData.iconName}' in Tab {i}");
                }
            }

            // Set localized text
            if (!string.IsNullOrEmpty(tabData.localizationKey))
            {
                barItem.SetLocalizedKey(tabData.localizationKey);
            }

            barItems.Add(barItem);

            // Store selected reference
            if (tabData.state == TabState.Selected)
            {
                currentlySelected = barItem;
            }
        }

        // Simulate click on preselected tab
        if (currentlySelected != null)
        {
            Debug.Log("[BarItemManager] Applying selected tab from config...");
            HandleTabSelected(currentlySelected);
        }
    }

    private void HandleTabSelected(BarItemController selected)
    {
        Debug.Log($"[BarItemManager] Tab selected: {selected.name}");

        if (currentlySelected != null && currentlySelected != selected)
        {
            currentlySelected.Deselect();
        }

        currentlySelected = selected;
    }

    private Sprite FindIconByName(string iconName)
    {
        foreach (var icon in config.icons)
        {
            if (icon.iconName == iconName)
                return icon.iconSprite;
        }
        return null;
    }

    private BarItemController.State ConvertState(TabState state)
    {
        return state switch
        {
            TabState.Locked => BarItemController.State.Locked,
            TabState.Unlocked => BarItemController.State.Unlocked,
            TabState.Selected => BarItemController.State.Selected,
            _ => BarItemController.State.Unlocked
        };
    }
}
