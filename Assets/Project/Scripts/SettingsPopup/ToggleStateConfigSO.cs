using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToggleStateConfigSO", menuName = "UI/Toggle State Config", order = 4)]
public class ToggleDataSO : ScriptableObject
{
    [System.Serializable]
    public class ToggleEntry
    {
        public string name;           // Unique toggle name (used as key)
        public bool defaultState = false;
    }

    public List<ToggleEntry> entries = new();
}
