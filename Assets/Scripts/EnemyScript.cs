using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int enemyHealth;
    [SerializeField] private int knockbackForce;
    [SerializeField] public bool aggro;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float attentionSpan;
    private GameObject player;
    private TimeStateMachineScript stateMachineScript;
    private Rigidbody2D rb;
    private PlayerControl playerControl;
    private Vector3 targetPos;
    private Vector3 startPos;
    private float timer;
    NavMeshAgent agent;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("PlayerHitbox");
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
        stateMachineScript = GetComponent<TimeStateMachineScript>();
    }
    private void Start()
    {
        playerControl = PlayerControl.Instance;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        FaceTarget();
        if (stateMachineScript.returnState() == "frozen")
        {
            agent.SetDestination(transform.position);
            if (timer <= 0)
            {
                timer = attentionSpan;
            }
        }
        else if (stateMachineScript.returnState() == "flowing")
                agent.SetDestination(targetPos);
                // Check if we've reached the destination
                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            // Done
                            timer -= Time.deltaTime;
                            //transform.rotation =
                            Debug.Log(timer);
                            if (timer <= 0)
                            {
                                targetPos = startPos;
                                timer = attentionSpan;
                            }
                        }
                    }
                }
        else if (stateMachineScript.returnState() == "dead")
        {
            Destroy(gameObject);
        }
        //Debug.Log(state);
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            stateMachineScript.state = TimeStateMachineScript.State.dead;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (stateMachineScript.returnState() == "flowing")
        {
            if (collision.CompareTag("Sword"))
            {
                Vector2 playerDirection = (transform.position - collision.transform.position).normalized;
                Vector2 knockback = playerDirection * knockbackForce;
                rb.AddForce(knockback, ForceMode2D.Impulse);
                TakeDamage(playerControl.swordDmg);
                
            }
            else if (collision.CompareTag("PlayerHitbox"))
            {
                Vector2 playerDirection = (transform.position - collision.transform.position).normalized;
                Vector2 knockback = playerDirection * knockbackForce * .5f;
                rb.AddForce(knockback, ForceMode2D.Impulse);
                Debug.Log(collision.tag);
            }
        }
    }
    private void FixedUpdate()
    {
        LayerMask layerMask = LayerMask.GetMask("Default");
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 20f, layerMask);
        if (ray.collider != null && ray.collider.CompareTag("PlayerHitbox"))
        {
            if (stateMachineScript.returnState() == "flowing")
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
                targetPos = player.transform.position;
            }
            else
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
            }
        }
    }
    private void FaceTarget()
    {
        var vel = agent.velocity;
        vel.z = 0;

        if (vel != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(
                                    Vector3.forward,
                                    vel
            );
        }
    }
}
