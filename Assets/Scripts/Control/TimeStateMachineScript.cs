using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStateMachineScript: MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 currMomentum;
    private Rigidbody2D rb;
    public enum State
    {
        flowing,
        frozen,
        dead,
    }
    public State state;
    void Start()
    {
        
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = State.flowing;
    }
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
            case State.frozen:
                break;
            case State.flowing:
                break;
            case State.dead:
                Destroy(gameObject);
                break;
        }
        //Debug.Log(state);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Spotlight") && state == State.frozen)
        {
            state = State.flowing;
            Debug.Log(currMomentum);
            rb.velocity = currMomentum;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Spotlight"))
        {
            state = State.frozen;
            if (rb.velocity.magnitude >= 1)
            {
                currMomentum = rb.velocity;
            }
            rb.velocity = Vector2.zero;
        }
    }
    public string returnState()
    {
        return state.ToString();
    }
}
