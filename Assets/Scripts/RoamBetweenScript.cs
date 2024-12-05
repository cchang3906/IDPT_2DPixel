using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamBetweenScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject targetPosition;
    public bool isRoaming;

    // Update is called once per frame
    public Vector3 returnRoamingPos()
    {
        return targetPosition.transform.position;
    }
}
