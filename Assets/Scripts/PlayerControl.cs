using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintMulti = 2f;
    [SerializeField] private float mouseSense = 2f;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private PlayerInputHandler inputHandler;
    private Vector2 currentMovement;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        inputHandler = PlayerInputHandler.Instance;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float speed = walkSpeed * (inputHandler.SprintValue > 0 ? sprintMulti : 1f);
        Vector2 inputDirection = new Vector2(inputHandler.MoveInput.x, inputHandler.MoveInput.y);
        inputDirection.Normalize();

        rb.velocity = new Vector2(inputHandler.MoveInput.x, );


    }
}
