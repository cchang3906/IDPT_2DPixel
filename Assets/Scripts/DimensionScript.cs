using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject otherDimension;
    private bool firstDimension;
    private GameObject player;
    private PlayerControl playerControl;
    private GameObject[] enemies;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerControl = player.GetComponent<PlayerControl>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        otherDimension = GameObject.FindGameObjectWithTag("Dimension");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            firstDimension = !firstDimension;
        }
        otherDimension.SetActive(firstDimension);
    }
}
