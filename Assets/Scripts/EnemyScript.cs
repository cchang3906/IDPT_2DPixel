using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int enemyHealth;
    [SerializeField] private int knockbackForce;
    [SerializeField] public bool aggro;
    private GameObject player;
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("PlayerHitbox");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (aggro && collision.tag == "Sword")
        {
            Vector2 playerDirection = (transform.position - collision.transform.position).normalized;
            Vector2 knockback = playerDirection * knockbackForce;
            rb.AddForce(knockback, ForceMode2D.Impulse);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "WideSpotlight" || collision.tag == "NarrowSpotlight" || collision.tag == "NearSpotlight")
        {
            aggro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "WideSpotlight" || collision.tag == "NarrowSpotlight" || collision.tag == "NearSpotlight")
        {
            aggro = false;
        }
    }
    private void FindTarget()
    {
        float targetRange = 50f;
        if (Vector3.Distance(transform.position, player.transform.position) <= targetRange)
        {

        }
    }
}
