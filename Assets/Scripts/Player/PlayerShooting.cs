using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private bool canFire = true;
    [SerializeField] private float firingTime;
    //public bool allowButtonHold;
    //public BulletScript bulletType;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        TimeGunCooldown();
    }
    private void TimeGunCooldown()
    {
        if (!canFire)
        {
            BulletPanelScript.instance.UpdateBulletCount();
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
        //Debug.Log("fired");
        if (canFire && PlayerControl.Instance.bulletCount > 0)
        {
            canFire = false;
            StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControlScript>().Shake());
            PlayerControl.Instance.bulletCount -= 1;
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }
}
