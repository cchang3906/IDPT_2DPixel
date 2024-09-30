using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private int bullets = 5;
    private GameObject player;
    private PlayerControl playerControl;
    private Rigidbody2D rb;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerControl = player.GetComponent<PlayerControl>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (collision.GetComponent<EnemyScript>().aggro)
            {
                Vector2 enemyDirection = (transform.position - collision.transform.position).normalized;
                Vector2 knockback = enemyDirection * knockbackForce;
                rb.AddForce(knockback, ForceMode2D.Impulse);
                playerControl.TakeDamage(30);
            }
            //rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            //Debug.Log(knockback);
        }
        else if (collision.tag == "BulletPickup")
        {
            playerControl.bulletCount += bullets;
            Destroy(collision.gameObject);
        }
    }
}
