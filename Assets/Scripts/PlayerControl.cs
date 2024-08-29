using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintMulti = 2f;
    [SerializeField] private float mouseSense = 2f;
    [SerializeField] private Camera mainCamera;
    private Rigidbody2D rb;
    private PlayerInputHandler inputHandler;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private Vector2 smoothMovement;
    private Vector2 smoothVelocity;


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        inputHandler = PlayerInputHandler.Instance;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        smoothMovement = Vector2.SmoothDamp(smoothMovement, movementInput, ref smoothVelocity, 0.1f);
        rb.velocity = smoothMovement * walkSpeed;
    }
    private void HandleRotation()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(lookInput.x, lookInput.y, mainCamera.nearClipPlane));
        transform.up = mousePosition - new Vector2(transform.position.x, transform.position.y);
        Vector3 cameraPosition = new Vector3((mousePosition.x / 2 + transform.position.x) / 2, (mousePosition.y / 2 + transform.position.y) / 2, -10);
        mainCamera.transform.position = cameraPosition;

        //var dir = new Vector3(lookInput.x, lookInput.y, mainCamera.nearClipPlane) - mainCamera.ScreenToWorldPoint(transform.position);
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Debug.Log(lookInput);
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
    public void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }
    public void OnLook(InputValue inputValue)
    {
        lookInput = inputValue.Get<Vector2>();
    }
}
