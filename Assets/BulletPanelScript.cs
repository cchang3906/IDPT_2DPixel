using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletPanelScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] bullets;
    public static BulletPanelScript instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UpdateBulletCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateBulletCount()
    {
        foreach (GameObject image in bullets)
        {
            image.SetActive(false);
        }
        for (int i = 0; i < PlayerControl.Instance.bulletCount; i++)
        {
            bullets[i].SetActive(true);
        }
        //Debug.Log(PlayerControl.Instance.bulletCount);
    }

    public void HideBulletCount()
    {
        foreach (GameObject image in bullets)
        {
            image.GetComponent<Image>().CrossFadeAlpha(0f, .25f, false);
            //image.GetComponent<Image>().canvasRenderer.SetAlpha(0f);
            //Debug.Log(image.GetComponent<Image>().color.a);
        }
    }
    //public void AppearBulletCount()
    //{
    //    foreach (GameObject image in bullets)
    //    {
    //        image.GetComponent<Image>().CrossFadeAlpha(1f, .25f, false);
    //        Debug.Log(image.GetComponent<Image>().color.a);
    //    }
    //}

    public void InstaHideBulletCount()
    {
        foreach (GameObject image in bullets)
        {
            image.GetComponent<Image>().canvasRenderer.SetAlpha(0f);
        }
    }
    public void InstaAppearBulletCount()
    {
        foreach (GameObject image in bullets)
        {
            image.GetComponent<Image>().canvasRenderer.SetAlpha(1f);
        }
    }

    
}
