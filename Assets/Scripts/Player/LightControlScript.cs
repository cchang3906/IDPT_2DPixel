using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightControlScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float minScale = 0.5f; // Minimum size
    [SerializeField] private float scaleSpeed = 5f; // Speed of scaling

    private bool isScaling = false;
    private Vector3 startScale;
    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerControl.Instance.takenDamage && !isScaling)
        {
            Debug.Log("triggered");
            StartCoroutine(ScaleSpotlight());
        }
    }
    private IEnumerator ScaleSpotlight()
    {
        isScaling = true;
        while (transform.localScale.x > minScale)
        {
            transform.localScale -= Vector3.one * scaleSpeed * Time.deltaTime;
            yield return null;
        }

        // Grow the spotlight back to its original scale
        while (transform.localScale.x < startScale.x)
        {
            transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
            yield return null;
        }

        // Ensure it reaches exactly the original scale at the end
        transform.localScale = startScale;
        isScaling = false;
    }
}
