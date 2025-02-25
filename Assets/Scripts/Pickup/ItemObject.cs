using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryItemData referenceItem;
    public InventorySystemScript inventorySystemScript;
    private void Awake()
    {
        
    }
    void Start()
    {
        inventorySystemScript = InventorySystemScript.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public virtual void OnHandlePickupItem()
    {
        inventorySystemScript.Add(referenceItem);
        Destroy(gameObject);
    }
    public virtual void UseItem()
    {
        inventorySystemScript.Remove(referenceItem);
    }
}
