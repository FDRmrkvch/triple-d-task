using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PopupConfig", menuName = "UI/Popup Config")]
public class PopupConfigSO : ScriptableObject
{
    public List<PopupEntry> popups = new();

    [System.Serializable]
    public class PopupEntry
    {
        public string popupName;
        public GameObject popupPrefab;
    }
}