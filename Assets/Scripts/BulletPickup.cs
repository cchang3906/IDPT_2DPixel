using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int bullets = 5;
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
        if(collision.tag == "Player")
        {
            playerControl.bulletCount += bullets;
            Destroy(gameObject);
        }
    }
}
