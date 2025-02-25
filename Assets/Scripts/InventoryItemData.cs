using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "InventoryItemData")]
public class InventoryItemData : ScriptableObject
{
    // Start is called before the first frame update
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
    public bool stackable = true;

}
