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
    private GameObject player;
    private Rigidbody2D rb;
    private Vector3 lastSeenPlayerPos;
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
    }
    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        lastSeenPlayerPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
            case State.frozen:
                agent.SetDestination(transform.position);
                break;
            case State.aggro:
                agent.SetDestination(lastSeenPlayerPos);
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
                TakeDamage(50);
            }
            else if (collision.tag == "PlayerHitbox")
            {
                Vector2 playerDirection = (transform.position - collision.transform.position).normalized;
                Vector2 knockback = playerDirection * knockbackForce;
                rb.AddForce(knockback, ForceMode2D.Impulse);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "WideSpotlight" || collision.tag == "NarrowSpotlight" || collision.tag == "NearSpotlight")
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
                lastSeenPlayerPos = player.transform.position;
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
}
