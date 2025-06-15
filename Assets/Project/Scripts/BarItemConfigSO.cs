using System.Collections.Generic;
using UnityEngine;

public enum TabState
{
    Locked,
    Unlocked,
    Selected
}

[System.Serializable]
public class TabIconEntry
{
    public string iconName;
    public Sprite iconSprite;
}

[System.Serializable]
public class TabConfig
{
    public TabState state;
    public string iconName;
}

[CreateAssetMenu(fileName = "TabBarConfig", menuName = "UI/Tab Bar Config")]
public class TabBarConfigSO : ScriptableObject
{
    [Header("Available Icons")]
    public List<TabIconEntry> icons = new List<TabIconEntry>();

    [Header("Number of Tabs")]
    public int tabCount = 5;

    [Header("Tab Configurations")]
    public List<TabConfig> tabConfigs = new List<TabConfig>();

    private void OnValidate()
    {
        if (tabCount < 0) tabCount = 0;

        // Sync tabConfigs with tabCount
        while (tabConfigs.Count < tabCount)
            tabConfigs.Add(new TabConfig());

        while (tabConfigs.Count > tabCount)
            tabConfigs.RemoveAt(tabConfigs.Count - 1);

        // Ensure only one Selected tab
        int selectedCount = 0;
        foreach (var config in tabConfigs)
        {
            if (config.state == TabState.Selected)
                selectedCount++;
        }

        if (selectedCount > 1)
        {
            Debug.LogWarning("Only one tab can be in the 'Selected' state. Others will be set to 'Unlocked'.");
            bool firstFound = false;
            for (int i = 0; i < tabConfigs.Count; i++)
            {
                if (tabConfigs[i].state == TabState.Selected)
                {
                    if (!firstFound)
                        firstFound = true;
                    else
                        tabConfigs[i].state = TabState.Unlocked;
                }
            }
        }
    }
}
