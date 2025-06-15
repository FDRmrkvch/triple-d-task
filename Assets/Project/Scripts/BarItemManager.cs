using UnityEngine;
using System.Collections.Generic;

public class BarItemManager : MonoBehaviour
{
    [Header("Configuration")]
    public TabBarConfigSO config;

    [Header("Bar Item Prefab")]
    public GameObject barItemPrefab;

    [Header("Bar Container")]
    public Transform barContainer;

    private readonly List<BarItemController> barItems = new();
    private BarItemController currentlySelected;

    private void Start()
    {
        if (config == null)
        {
            Debug.LogError("[BarItemManager] Config is missing!");
            return;
        }

        GenerateTabs();
    }

    private void GenerateTabs()
    {
        ClearBar();
        Debug.Log($"[BarItemManager] Generating {config.tabCount} tabs...");

        for (int i = 0; i < config.tabCount; i++)
        {
            var tabData = config.tabConfigs[i];
            var tab = CreateTab(tabData, i);
            barItems.Add(tab);

            if (tabData.state == TabState.Selected)
                currentlySelected = tab;
        }

        if (currentlySelected != null)
        {
            Debug.Log("[BarItemManager] Applying selected tab from config...");
            HandleTabSelected(currentlySelected);
        }
    }

    private BarItemController CreateTab(TabConfig tabData, int index)
    {
        GameObject instance = Instantiate(barItemPrefab, barContainer);
        var controller = instance.GetComponent<BarItemController>();

        if (controller == null)
        {
            Debug.LogError($"[BarItemManager] Missing BarItemController on tab prefab (index {index})");
            return null;
        }

        controller.Initialize(ConvertState(tabData.state));
        controller.OnSelected += HandleTabSelected;

        if (string.IsNullOrEmpty(tabData.iconName))
        {
            if (tabData.state != TabState.Locked)
                Debug.LogWarning($"[BarItemManager] Tab {index} has empty icon name but is not Locked.");
            else
                Debug.Log($"[BarItemManager] Tab {index} is Locked with no icon — skipping icon.");
        }
        else
        {
            Sprite icon = FindIconByName(tabData.iconName);
            if (icon != null)
                controller.SetIcon(icon);
            else
                Debug.LogWarning($"[BarItemManager] Icon '{tabData.iconName}' not found for Tab {index}.");
        }

        return controller;
    }

    private void ClearBar()
    {
        foreach (Transform child in barContainer)
            Destroy(child.gameObject);

        barItems.Clear();
        currentlySelected = null;
    }

    private void HandleTabSelected(BarItemController selected)
    {
        if (currentlySelected != null && currentlySelected != selected)
            currentlySelected.Deselect();

        currentlySelected = selected;
        Debug.Log($"[BarItemManager] Tab selected: {selected.name}");
    }

    private Sprite FindIconByName(string name)
    {
        return config.icons.Find(icon => icon.iconName == name)?.iconSprite;
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
