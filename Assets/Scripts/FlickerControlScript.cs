using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerControlScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator anim;
    public bool flickering = true;
    [SerializeField] private List<string> triggerList;
    [SerializeField] private float minTime, maxTime;

    private void Start()
    {
        StartCoroutine(flickerLights());
    }

    IEnumerator flickerLights()
    {
        while (flickering)
        {
            string name = triggerList[Random.Range(0, triggerList.Count)];
            anim.SetTrigger(name);
            float randTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(randTime);
            anim.ResetTrigger(name);
        }

    }
    public void FlickeringOn()
    {
        flickering = true;
        StartCoroutine(flickerLights());
    }

    public void FlickeringOff()
    {
        flickering = false;
        foreach(string i in triggerList)
        {
            anim.ResetTrigger(i);
        }
    }
    //private void Update()
    //{
    //    Debug.Log(flickering);

    //}
}
