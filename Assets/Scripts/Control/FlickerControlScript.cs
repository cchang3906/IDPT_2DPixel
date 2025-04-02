using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerControlScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator anim;
    public bool flickering;
    [SerializeField] private List<string> triggerList;
    [SerializeField] private float minTime, maxTime;
    [SerializeField] private int enemyCount = 0;
    public static FlickerControlScript Instance { get; private set; }
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
    private void Start()
    {
        StartCoroutine(flickerLights());
    }
    public void EnemyDetected()
    {
        enemyCount++;
        Debug.Log("Seen");
        UpdateFlickerState();
    }

    public void EnemyLost()
    {
        enemyCount = Mathf.Max(0, enemyCount - 1);
        Debug.Log("Not Seen");
        UpdateFlickerState();
    }

    private void UpdateFlickerState()
    {
        if (enemyCount > 0)
        {
            FlickeringOn();
        }
        else
        {
            FlickeringOff();
        }
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
