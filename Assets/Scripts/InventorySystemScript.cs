using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystemScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static InventorySystemScript instance { get; private set;}
    private Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    [SerializeField] private List<InventoryItem> inventory = new List<InventoryItem>();
    public List<InventoryItem> inventoryList => inventory; // reference only

    void Awake()
    {
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(inventory.Find(item => item.data.stackable == false));
    }

    public void Add(InventoryItemData referenceData, int uniqueID = -1)
    {
        if (referenceData.stackable)
        {
            if(m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
            {
                value.AddToStack();
            }
            else
            {
                InventoryItem newItem = new InventoryItem(referenceData);
                inventory.Add(newItem);
                m_itemDictionary.Add(referenceData, newItem);
            }
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData, uniqueID);
            inventory.Add(newItem);
        }
        
    }

    public void Remove(InventoryItemData referenceData, int instanceID = -1)
    {
        if (referenceData.stackable)
        {
            if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
            {
                value.RemoveFromStack();
                if (value.stackSize == 0)
                {
                    inventory.Remove(value);
                    m_itemDictionary.Remove(referenceData);
                }
            }
        }
        else
        {
            InventoryItem itemToRemove = inventory.Find(item => item.data == referenceData && item.instanceID == instanceID);
            if (itemToRemove != null)
            {
                inventory.Remove(itemToRemove);
            }
        }
    }

    public bool ContainsItem(InventoryItemData referenceData, int instanceID = -1)
    {
        if (referenceData.stackable)
        {
            return m_itemDictionary.ContainsKey(referenceData);
        }
        else
        {
            InventoryItem itemToRemove = inventory.Find(item => item.data == referenceData && item.instanceID == instanceID);
            if (itemToRemove != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    [System.Serializable]
    public class InventoryItem
    {
        public InventoryItemData data { get; private set; }
        public int stackSize { get; private set; }
        public int instanceID { get; private set; }

        public InventoryItem(InventoryItemData source, int instanceID = -1)
        {
            data = source;
            this.instanceID = instanceID;
            AddToStack();
        }

        public void AddToStack()
        {
            stackSize++;
        }
        public void RemoveFromStack()
        {
            stackSize--;
        }
        public virtual void UseItem()
        {

        }

    }
}
