using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healing = 30;
    private GameObject player;
    private PlayerControl playerControl;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerControl = player.GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox")
        {
            if (playerControl.playerHealth < 100)
            {
                playerControl.playerHealth += healing;
            }
            Destroy(gameObject);
        }
    }
}
