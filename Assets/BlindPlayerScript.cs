using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BlindPlayerScript : MonoBehaviour
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
    private GameObject spot;
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
    private Vector3 originalScale;
    public static BlindPlayerScript Instance { get; private set; }

    private void Awake()
    {
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
        spot = GameObject.FindGameObjectWithTag("Spotlight");
        playerKnockback = GameObject.FindGameObjectWithTag("PlayerHitbox").GetComponent<PlayerKnockback>();
        cameraControl = mainCamera.GetComponent<CameraControlScript>();
    }
    void Start()
    {
        gun.SetActive(false);
        BulletPanelScript.instance.InstaHideBulletCount();
        originalScale = spot.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        HandleWeapons();
        //HandleCameraShake();
        HandleDeath();
        HandleRotation();
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LerpScale(originalScale * 3, 2f));
        }
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

    IEnumerator LerpScale(Vector3 endScale, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            float t = elapsedTime / time;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            spot.transform.localScale = Vector3.Lerp(originalScale, endScale, t);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        spot.transform.localScale = endScale; // Ensure it reaches the exact target scale
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

    private void HandleWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gun.SetActive(true);
            BulletPanelScript.instance.InstaAppearBulletCount();
            sword.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gun.SetActive(false);
            BulletPanelScript.instance.HideBulletCount();
            sword.SetActive(true);
        }
    }
    private void HandleDeath()
    {
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene("Tutorial");
            StartCoroutine(cameraControl.Shake());
        }
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

    public void TakeDamage(int damage)
    {
        if (!playerKnockback.invincible)
        {
            playerHealth -= damage;
            takenDamage = true;
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
        if (stamina >= dashStamina)
        {
            isDashing = true;
            stamina -= dashStamina;
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
