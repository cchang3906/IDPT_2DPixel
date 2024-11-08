using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurControlScript : MonoBehaviour
{
    [SerializeField] Material blurMat;
    public static BlurControlScript Instance;

    private float blurAmount = 0f;
    private void Update()
    {
        ChangeBlur(blurAmount);
        blurAmount += 0.00001f;
    }
    private void ChangeBlur(float alpha)
    {
        blurMat.SetFloat("_BlurAmount", alpha);
    }
    public void SetBlur(float alpha)
    {
        blurAmount = alpha;
    }
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
    }
}
