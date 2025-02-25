using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int doorID;
    [SerializeField] private Animator anim;
    public bool open;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(PlayerControl.Instance.transform.position, transform.position) <= 2 && Input.GetKeyDown(KeyCode.E))
        {
            InventorySystemScript.InventoryItem matchingKey = InventorySystemScript.instance.inventoryList.Find(item => item.data.stackable == false && item.instanceID == doorID);
            //Debug.Log(matchingKey);
            if (matchingKey != null)
            {
                InventorySystemScript.instance.Remove(matchingKey.data, matchingKey.instanceID);
                open = true;
            }
        }
        if (open)
        {
            anim.SetTrigger("Open");
            open = false;
        }
    }
}

