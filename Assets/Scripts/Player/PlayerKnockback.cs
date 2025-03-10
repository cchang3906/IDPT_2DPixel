using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float knockbackForce = 10f;
    public bool invincible;
    private GameObject player;
    private Rigidbody2D rb;
    private ItemObject currentItem;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (currentItem != null && Input.GetKeyDown(KeyCode.E))
        {
            currentItem.OnHandlePickupItem();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invincible && collision.tag == "Enemy")
        {
            Vector2 enemyDirection = (transform.position - collision.transform.position).normalized;
            Vector2 knockback = enemyDirection * knockbackForce;
            rb.AddForce(knockback, ForceMode2D.Impulse);

            //rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            //Debug.Log(knockback);
        }
        else if (collision.TryGetComponent<ItemObject>(out ItemObject item))
        {
            currentItem = item;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ItemObject>(out ItemObject item))
        {
            currentItem = null;
        }
    }
}
