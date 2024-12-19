using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private int keyID;
    private GameObject[] doors;
    private GameObject matchingDoor;
    // Start is called before the first frame update
    private void Start()
    {
        doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in doors)
        {
            if (door.GetComponent<DoorScript>().doorID == keyID)
            {
                matchingDoor = door;
                break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox")
        {
            matchingDoor.GetComponent<DoorScript>().open = true;
            Destroy(gameObject);
        }
    }
}
