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
    [SerializeField] private float shakeDuration;
    [SerializeField] private bool testShake;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float stamina = 100f;
    [SerializeField] private int staminaRecovery = 20;
    [SerializeField] private float dashStamina = 35f;
    [SerializeField] public int bulletCount;
    [SerializeField] private float iFrameDuration;
    private PlayerKnockback playerKnockback;
    public int swordDmg;
    private Vector3 clampedCameraPosition;
    private bool takenDamage;
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
    private Vector2 mousePosition;
    private PlayerShooting shootingScript;
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
        shootingScript = gun.GetComponent<PlayerShooting>();
        bulletCount = shootingScript.bulletCount;
        narrowSpot = GameObject.FindGameObjectWithTag("NarrowSpotlight");
        wideSpot = GameObject.FindGameObjectWithTag("WideSpotlight");
        playerKnockback = GameObject.FindGameObjectWithTag("PlayerHitbox").GetComponent<PlayerKnockback>();
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
        if ((Input.GetMouseButtonDown(0) && bulletCount > 0 && gun.activeSelf) || takenDamage)
        {
            bulletCount = shootingScript.bulletCount;
            //Debug.Log(bulletCount);
            StartCoroutine(Shake());
            takenDamage = false;
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
        Vector3 cameraPosition = new Vector3((transform.position.x + mousePosition.x) / 2, (transform.position.y + mousePosition.y) / 2, -10);
        float clampedX = Mathf.Clamp(cameraPosition.x, transform.position.x - 5, transform.position.x + 5);
        float clampedY = Mathf.Clamp(cameraPosition.y, transform.position.y - 3, transform.position.y + 3);
        clampedCameraPosition = new Vector3(clampedX, clampedY, -10);
        mainCamera.transform.position = clampedCameraPosition;

        //var dir = new Vector3(lookInput.x, lookInput.y, mainCamera.nearClipPlane) - mainCamera.ScreenToWorldPoint(transform.position);
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Debug.Log(lookInput);
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
    IEnumerator Shake()
    {
        float timer = 0f;
        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;
            float strength = curve.Evaluate(timer / shakeDuration);
            mainCamera.transform.position =  clampedCameraPosition + Random.insideUnitSphere * strength;
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
