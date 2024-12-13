using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int doorID;
    public bool open;
    private GameObject lamp;
    [SerializeField] private Animator anim;
    [SerializeField] private float fadeSpeed = 0.5f;
    void Start()
    {
        lamp = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            anim.SetTrigger("Open");
            Color objectColor = lamp.GetComponent<Renderer>().material.color;
            float fadeAmount = objectColor.a - fadeSpeed * Time.deltaTime;
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            lamp.GetComponent<Renderer>().material.color = objectColor;
            if(objectColor.a <= 0)
            {
                Destroy(lamp);
                open = false;
            }
        }
    }
}

