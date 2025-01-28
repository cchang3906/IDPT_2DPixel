using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeSlider;
    private PlayerControl playerControl;
    private float currHealth;
    private float lerpSpeed = 0.05f;
    void Start()
    {
        playerControl = PlayerControl.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        currHealth = playerControl.playerHealth;
        if(healthSlider.value != currHealth)
        {
            healthSlider.value = currHealth;
        }
        if (healthSlider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, currHealth, lerpSpeed);
        }
    }
}
