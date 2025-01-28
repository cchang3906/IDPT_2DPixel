using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    private PlayerControl playerControl;
    private int bulletDamage;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector3 mousePos;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControl = PlayerControl.Instance;
        bulletDamage = playerControl.bulletDmg;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        //Debug.Log(direction);
        rb.velocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;
        float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.CompareTag("PlayerHitbox") || other.CompareTag("Spotlight")))
        {
            Destroy(gameObject);
            if(other.tag == "Enemy")
            {
                other.GetComponent<EnemyScript>().TakeDamage(bulletDamage);
            }
        }
    }
}
