using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool canSwing;
    [SerializeField] private float swingCooldown;
    [SerializeField] private Animator anim;
    [SerializeField] private float damage;
    private EnemyScript enemyScript;
    private GameObject enemy;
    private bool isLeft;
    private float timer;
    void Awake()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyScript = enemy.GetComponent<EnemyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isLeft)
                {
                    isLeft = false;
                    anim.SetTrigger("LeftToRightAttack");
                }
                else
                {
                    isLeft = true;
                    anim.SetTrigger("RightToLeftAttack");
                }
                timer = swingCooldown;
            }
        }
        timer -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemyScript.TakeDamage(50);
        }
    }
}
