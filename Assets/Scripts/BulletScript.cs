using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector3 mousePos;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (!other.CompareTag("PlayerHitbox"))
        {
            Destroy(gameObject);
            if(other.tag == "Enemy")
            {
                Destroy(other.gameObject);
            }
        }
    }
}
