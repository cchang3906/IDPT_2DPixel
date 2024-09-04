using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet;
    public bool canFire;
    public float firingTime;
    private float timer;
    private PlayerInputHandler inputHandler;
    // Start is called before the first frame update
    void Awake()
    {
        inputHandler = PlayerInputHandler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        TimeGunCooldown();
    }
    private void TimeGunCooldown()
    {
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer >= firingTime)
            {
                canFire = true;
                timer = 0;
            }
        }
    }
    public void OnAttack(InputValue inputValue)
    {
        Debug.Log("fired");
        if (canFire)
        {
            canFire = false;
            Instantiate(bullet, bullet.transform.position, Quaternion.identity);
        }
    }
}
