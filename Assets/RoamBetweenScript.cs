using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamBetweenScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float roamDistance;
    [SerializeField] private bool moveXAxis;
    private Vector2 roamingDir;
    void Start()
    {
        if (moveXAxis)
        {
            roamingDir = transform.position + new Vector3(roamDistance, 0, 0);
        }
        else
        {
            roamingDir = transform.position + new Vector3(0, roamDistance, 0);
        }
    }

    // Update is called once per frame
    public Vector2 returnRoamingPos()
    {
        return roamingDir;
    }
}
