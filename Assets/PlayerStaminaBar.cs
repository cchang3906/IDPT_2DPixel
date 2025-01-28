using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Slider easeSlider;
    private PlayerControl playerControl;
    private float currStamina;
    private float lerpSpeed = 0.05f;
    void Start()
    {
        playerControl = PlayerControl.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        currStamina = playerControl.stamina;
        if (staminaSlider.value != currStamina)
        {
            staminaSlider.value = currStamina;
        }
        if (staminaSlider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, currStamina, lerpSpeed);
        }
    }
}
