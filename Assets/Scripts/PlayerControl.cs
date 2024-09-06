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
    [SerializeField] private float duration;
    [SerializeField] private bool test;
    [SerializeField] AnimationCurve curve;
    private Rigidbody2D rb;
    private GameObject gun;
    private PlayerInputHandler inputHandler;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private Vector2 smoothMovement;
    private Vector2 smoothVelocity;
    private Vector2 mousePosition;
    private PlayerShooting shootingScript;
    private int bulletCount;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        inputHandler = PlayerInputHandler.Instance;
    }
    void Start()
    {
        gun = GameObject.FindGameObjectWithTag("Gun");
        shootingScript = gun.GetComponent<PlayerShooting>();
        bulletCount = shootingScript.bulletCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && bulletCount > 0)
        {
            bulletCount = shootingScript.bulletCount;
            Debug.Log(bulletCount);
            StartCoroutine(Shake());
        }
        HandleRotation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        //Vector2 directionToMouse = (mousePosition - rb.position).normalized;
        //Vector2 movementDirection = directionToMouse * movementInput.y + new Vector2(movementInput.x, 0);

        smoothMovement = Vector2.SmoothDamp(smoothMovement, movementInput, ref smoothVelocity, 0.1f);
        rb.velocity = smoothMovement * walkSpeed;
    }
    private void HandleRotation()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(lookInput.x, lookInput.y, mainCamera.nearClipPlane));
        transform.up = mousePosition - new Vector2(transform.position.x, transform.position.y);
        Vector3 cameraPosition = new Vector3((mousePosition.x / 2 + transform.position.x) / 2, (mousePosition.y / 2 + transform.position.y) / 2, -10);
        mainCamera.transform.position = cameraPosition;

        //var dir = new Vector3(lookInput.x, lookInput.y, mainCamera.nearClipPlane) - mainCamera.ScreenToWorldPoint(transform.position);
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Debug.Log(lookInput);
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
    IEnumerator Shake()
    {
        Vector3 startPos = mainCamera.transform.position;
        float timer = 0f;
        while (timer < duration)
        {
            Vector3 cameraPosition = new Vector3((mousePosition.x / 2 + transform.position.x) / 2, (mousePosition.y / 2 + transform.position.y) / 2, -10);
            timer += Time.deltaTime;
            float strength = curve.Evaluate(timer / duration);
            mainCamera.transform.position =  cameraPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
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
