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
    private Rigidbody2D rb;
    private PlayerControl playerControl;
    private Vector3 targetPos;
    private Vector3 startPos;
    private float timer;
    NavMeshAgent agent;
    private enum State
    {
        aggro,
        frozen,
        dead,
    }
    private State state;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("PlayerHitbox");
        state = State.frozen;
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
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
        switch (state)
        {
            default:
            case State.frozen:
                agent.SetDestination(transform.position);
                if (timer <= 0)
                {
                    timer = attentionSpan;
                }
                break;
            case State.aggro:
                agent.SetDestination(targetPos);
                FaceTarget();
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
                break;
            case State.dead:
                Destroy(gameObject);
                break;
        }
        //Debug.Log(state);
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            state = State.dead;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.aggro)
        {
            if (collision.tag == "Sword")
            {
                Vector2 playerDirection = (transform.position - collision.transform.position).normalized;
                Vector2 knockback = playerDirection * knockbackForce;
                rb.AddForce(knockback, ForceMode2D.Impulse);
                TakeDamage(playerControl.swordDmg);
            }
            else if (collision.tag == "PlayerHitbox")
            {
                Vector2 playerDirection = (transform.position - collision.transform.position).normalized;
                Vector2 knockback = playerDirection * knockbackForce * .5f;
                rb.AddForce(knockback, ForceMode2D.Impulse);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.tag == "WideSpotlight" || collision.tag == "NarrowSpotlight" || collision.tag == "NearSpotlight") && state == State.frozen)
        {
            state = State.aggro;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "WideSpotlight" || collision.tag == "NarrowSpotlight" || collision.tag == "NearSpotlight")
        {
            state = State.frozen;
        }
    }
    private void FixedUpdate()
    {
        LayerMask layerMask = LayerMask.GetMask("Default");
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 20f, layerMask);
        if (ray.collider != null && ray.collider.CompareTag("PlayerHitbox"))
        {
            if (state == State.aggro)
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

    public string returnState()
    {
        return state.ToString();
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
