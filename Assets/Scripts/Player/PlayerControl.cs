using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] public int bulletCount = 10;
    [SerializeField] private float knockbackForce = 1000f;
    private GameObject wideSpotlight;
    private bool takenDamage;
    private GameObject narrowSpotlight;
    private bool isDashing;
    private Rigidbody2D rb;
    private GameObject gun;
    private GameObject sword;
    private PlayerInputHandler inputHandler;
    private Vector2 movementInput;
    public Vector2 lookInput;
    private Vector2 smoothMovement;
    private Vector2 smoothVelocity;
    private Vector2 mousePosition;
    private PlayerShooting shootingScript;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        inputHandler = PlayerInputHandler.Instance;
    }
    void Start()
    {
        gun = GameObject.FindGameObjectWithTag("Gun");
        sword = GameObject.FindGameObjectWithTag("SwordPivot");
        narrowSpotlight = GameObject.FindGameObjectWithTag("NarrowSpotlight");
        wideSpotlight = GameObject.FindGameObjectWithTag("WideSpotlight");
        shootingScript = gun.GetComponent<PlayerShooting>();
        bulletCount = shootingScript.bulletCount;
        gun.SetActive(false);
        narrowSpotlight.SetActive(false);
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
            narrowSpotlight.SetActive(true);
            wideSpotlight.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gun.SetActive(false);
            sword.SetActive(true);
            narrowSpotlight.SetActive(false);
            wideSpotlight.SetActive(true);
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
        Vector3 cameraPosition = new Vector3((mousePosition.x / 2 + transform.position.x) / 2, (mousePosition.y / 2 + transform.position.y) / 2, -10);
        mainCamera.transform.position = cameraPosition;

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
            Vector3 cameraPosition = new Vector3((mousePosition.x / 2 + transform.position.x) / 2, (mousePosition.y / 2 + transform.position.y) / 2, -10);
            timer += Time.deltaTime;
            float strength = curve.Evaluate(timer / shakeDuration);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Vector2 enemyDirection = (transform.position - collision.transform.position).normalized;
            Vector2 knockback = enemyDirection * knockbackForce;
            rb.AddForce(knockback, ForceMode2D.Impulse);
            //rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            Debug.Log(knockback);
        }
    }

    private IEnumerator Dash()
    {
        rb.velocity = new Vector2(movementInput.x * dashSpeed, movementInput.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
}