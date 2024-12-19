using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] public int playerHealth = 100;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float stamina = 100f;
    [SerializeField] private int staminaRecovery = 20;
    [SerializeField] private float dashStamina = 35f;
    [SerializeField] public int bulletCount;
    [SerializeField] private float iFrameDuration;
    private CameraControlScript cameraControl;
    public bool inFear;
    private Vector2 mousePosition;
    private PlayerKnockback playerKnockback;
    public int swordDmg;
    public bool takenDamage;
    private GameObject narrowSpot;
    private GameObject wideSpot;
    private bool isDashing;
    private Rigidbody2D rb;
    private GameObject gun;
    private GameObject sword;
    private PlayerInputHandler inputHandler;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private Vector2 smoothMovement;
    private Vector2 smoothVelocity;
    private GameObject narrowSpotMask;
    private GameObject wideSpotMask;
    public static PlayerControl Instance
    {
        get;
        private set;
    }
    private void Awake(){
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        rb = GetComponent<Rigidbody2D>();
        inputHandler = PlayerInputHandler.Instance;
        gun = GameObject.FindGameObjectWithTag("Gun");
        sword = GameObject.FindGameObjectWithTag("SwordPivot");
        narrowSpot = GameObject.Find("PlayerNarrow");
        wideSpot = GameObject.Find("PlayerWide");
        narrowSpotMask = narrowSpot.transform.GetChild(0).gameObject;
        wideSpotMask = narrowSpot.transform.GetChild(0).gameObject;
        playerKnockback = GameObject.FindGameObjectWithTag("PlayerHitbox").GetComponent<PlayerKnockback>();
        cameraControl = mainCamera.GetComponent<CameraControlScript>();
    }
    void Start()
    {
        narrowSpot.SetActive(false);
        gun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(new Vector2(100, 100));
        if (inFear)
        {

        }
        if (playerHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gun.SetActive(true);
            sword.SetActive(false);
            narrowSpot.SetActive(true);
            wideSpot.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gun.SetActive(false);
            sword.SetActive(true);
            narrowSpot.SetActive(false);
            wideSpot.SetActive(true);
        }
        if ((Input.GetMouseButtonDown(0) && bulletCount > 0 && gun.activeSelf) || takenDamage || cameraControl.testShake)
        {
            //Debug.Log(bulletCount);
            StartCoroutine(cameraControl.Shake());
            takenDamage = false;
            cameraControl.testShake = false;
        }
        HandleRotation();
        HandleMovement();
    }

    private void FixedUpdate()
    {
        //Debug.Log(stamina);
        if (isDashing && stamina >= dashStamina)
        {
            StartCoroutine(Dash());
        }
        else if (stamina < 100)
        {
            stamina += Time.deltaTime * staminaRecovery;
        }
    }
    private void HandleMovement()
    {
        //Vector2 directionToMouse = (mousePosition - rb.position).normalized;
        //Vector2 movementDirection = directionToMouse * movementInput.y + new Vector2(movementInput.x, 0);

        smoothMovement = Vector2.SmoothDamp(smoothMovement, movementInput, ref smoothVelocity, 0.1f);
        if (smoothMovement != new Vector2(0, 0))
        {
            rb.AddForce(smoothMovement * walkSpeed);
        }
        //Debug.Log(smoothMovement * walkSpeed);
    }
    private void HandleRotation()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(lookInput.x, lookInput.y, mainCamera.nearClipPlane));
        transform.up = mousePosition - new Vector2(transform.position.x, transform.position.y);
        cameraControl.HandleCameraRotation(lookInput);
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
    public void OnDash(InputValue inputValue)
    {
        isDashing = true;
        if (stamina > dashStamina)
        {
            stamina -= dashStamina;
        }
    }
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        takenDamage = true;
    }

    private IEnumerator Dash()
    {
        rb.velocity = new Vector2(movementInput.x * dashSpeed, movementInput.y * dashSpeed);
        playerKnockback.invincible = true;
        yield return new WaitForSeconds(iFrameDuration);
        playerKnockback.invincible = false;
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = new Vector2(smoothMovement.x, smoothMovement.y);
        isDashing = false;
    }
}
