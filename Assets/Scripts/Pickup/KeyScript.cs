using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class KeyScript : ItemObject
{
    public int keyID;
    //private DoorManagerScript doorManager;
    //public override void UseItem()
    //{
    //    doorManager = DoorManagerScript.instance;
    //    if (doorManager == null)
    //    {
    //        Debug.LogWarning("No DoorManager found in the scene");
    //        return;
    //    }

    //    foreach (GameObject doorObj in doorManager.doors)
    //    {
    //        DoorScript door = doorObj.GetComponent<DoorScript>();
    //        if (door.doorID == keyID)
    //        {
    //            Debug.Log(keyID);
    //            //}
    //        }
    //    }
       
    //}
    public override void OnHandlePickupItem()
    {
        inventorySystemScript.Add(referenceItem, keyID);
        Destroy(gameObject);
    }
}

