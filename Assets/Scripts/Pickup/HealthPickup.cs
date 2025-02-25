using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : ItemObject
{
    [SerializeField] private int healing = 30;

    // Update is called once per frame
    void Update()
    {

    }

    public override void UseItem()
    {
        PlayerControl.Instance.playerHealth += healing;
        base.UseItem();
    }
}
