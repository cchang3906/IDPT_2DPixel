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
    [SerializeField] private float runSpeed;
    [SerializeField] private float attentionSpan;
    [SerializeField] private bool isRoaming;
    [SerializeField] private bool willRoam;
    [SerializeField] private int damage;
    private RoamBetweenScript roamScript;
    private GameObject player;
    private float walkSpeed;
    private TimeStateMachineScript stateMachineScript;
    private FlickerControlScript flickerControl;
    private Rigidbody2D rb;
    private PlayerControl playerControl;
    private Vector3 targetPos;
    private Vector3 startPos;
    private float timer;
    private bool roamingState;
    NavMeshAgent agent;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("PlayerHitbox");
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
        stateMachineScript = GetComponent<TimeStateMachineScript>();
        roamScript = GetComponent<RoamBetweenScript>();
        flickerControl = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<FlickerControlScript>();
        if (willRoam)
        {
            isRoaming = true;
        }
        
    }
    private void Start()
    {
        playerControl = PlayerControl.Instance;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        targetPos = transform.position;
        stateMachineScript.state = TimeStateMachineScript.State.frozen;
        walkSpeed = runSpeed / 2;
        flickerControl.FlickeringOff();
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
        {
            agent.SetDestination(targetPos);
            if (willRoam && isRoaming)
            {
                agent.speed = walkSpeed;
                Roaming();
            }
            else
            {
                // Check if we've reached the destination
                agent.speed = runSpeed;
                if (CheckPendingPath())
                {
                    // Done
                    timer -= Time.deltaTime;
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
    private void Roaming()
    {
        if (Vector2.Distance(transform.position, targetPos) <= 1)
        {
            roamingState = !roamingState;
        }
        if (roamingState)   // roamingState = false means its returning
        {
            targetPos = roamScript.returnRoamingPos();
        }
        else
        {
            targetPos = startPos;

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
                PlayerControl.Instance.TakeDamage(damage);
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
                isRoaming = false;
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
                targetPos = player.transform.position;
                flickerControl.FlickeringOn();
            }
            else
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                flickerControl.FlickeringOff();
            }
        }
        else if (ray.collider != null)
        {
            if (stateMachineScript.returnState() == "flowing")
            {
                isRoaming = true;
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

    private bool CheckPendingPath()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
