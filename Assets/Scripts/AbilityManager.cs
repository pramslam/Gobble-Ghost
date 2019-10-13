using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class AbilityManager : MonoBehaviour
{
    private Entity Stats;
    public DashFX dashFX;

    void Start()
    {
        try
        {
            Stats = gameObject.GetComponent<Entity>();
        }
        catch (Exception e)
        {
            Debug.LogError("Entity or its Stat component missing or unable to be found! Error: \n" + e);
        }
    }

    public void Dash()
    {
        StartCoroutine(BeginDashing(Time.time));
        dashFX.DashShow();
    }

    IEnumerator BeginDashing(float initialTime, Entity e = null)
    {
        var temp = Stats.Speed;
        var increaseSpeed = Stats.Speed * Stats.DashSpeedBonusMultiplier;
        while (Time.time < initialTime + Stats.DashDuration)
        {
            Stats.Speed = increaseSpeed;
            yield return null;
        }
        dashFX.DashHide();
        Stats.Speed = temp;
    }
}
