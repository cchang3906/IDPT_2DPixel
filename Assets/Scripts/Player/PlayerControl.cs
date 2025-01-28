using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public int playerHealth = 100;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private Camera mainCamera;
    public float stamina = 100f;
    [SerializeField] private float staminaCooldown = 1f;
    [SerializeField] private int staminaRecovery = 20;
    [SerializeField] private float dashStamina = 35f;
    private CameraControlScript cameraControl;
    private Vector2 mousePosition;
    private PlayerKnockback playerKnockback;
    private GameObject narrowSpot;
    private GameObject wideSpot;
    private bool isDashing;
    private Rigidbody2D rb;
    private GameObject gun;
    private GameObject sword;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private Vector2 smoothMovement;
    private Vector2 smoothVelocity;
    private float timer;
    [HideInInspector] public bool takenDamage;
    public int swordDmg;
    public int bulletDmg;
    public int bulletCount;
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
        gun = GameObject.FindGameObjectWithTag("Gun");
        sword = GameObject.FindGameObjectWithTag("SwordPivot");
        narrowSpot = GameObject.Find("PlayerNarrow");
        wideSpot = GameObject.Find("PlayerWide");
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
        if (isDashing)
        {
            StartCoroutine(Dash());
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
        if (timer > staminaCooldown && stamina < 100)
        {
            stamina += Time.deltaTime * staminaRecovery;
            if (stamina > 100)
            {
                stamina = 100;
            }
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
        if (stamina >= dashStamina)
        {
            isDashing = true;
            stamina -= dashStamina;
        }
    }
    public void TakeDamage(int damage)
    {
        if (!playerKnockback.invincible)
        {
            playerHealth -= damage;
            takenDamage = true;
        }
    }

    private IEnumerator Dash()
    {
        rb.velocity = new Vector2(movementInput.x * dashSpeed, movementInput.y * dashSpeed);
        playerKnockback.invincible = true;
        yield return new WaitForSeconds(dashDuration);
        playerKnockback.invincible = false;
        rb.velocity = new Vector2(smoothMovement.x, smoothMovement.y);
        isDashing = false;
    }
}
