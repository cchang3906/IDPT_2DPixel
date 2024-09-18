using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool canSwing;
    [SerializeField] private float swingCooldown;
    [SerializeField] private Animator anim;
    private bool isLeft;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isLeft)
                {
                    isLeft = false;
                    anim.SetTrigger("LeftToRightAttack");
                }
                else
                {
                    isLeft = true;
                    anim.SetTrigger("RightToLeftAttack");
                }
                timer = swingCooldown;
            }
        }
        timer -= Time.deltaTime;
    }
}
