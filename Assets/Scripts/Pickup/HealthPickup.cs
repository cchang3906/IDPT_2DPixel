using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healing = 30;

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox")
        {
            if (PlayerControl.Instance.playerHealth < 100)
            {
                PlayerControl.Instance.playerHealth += healing;
            }
            Destroy(gameObject);
        }
    }
}
