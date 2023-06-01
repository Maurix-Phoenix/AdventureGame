using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AdventureGame;

/// <summary>
/// EventManager: In Game Related Events
/// </summary>
public class EventManager : MonoBehaviour
{
    public bool Initialize()
    {
        return true;
    }

    public Action<MXEventParams<TriggerType>> TriggerAreaEnter;
    public void RaiseOnTriggerAreaEnter(MXEventParams<TriggerType> e)
    {
        TriggerAreaEnter?.Invoke(e);
    }


    #region Player Related Events

    public Action PlayerSpawn;
    public void RaiseOnPlayerSpawn()
    {
        PlayerSpawn?.Invoke();
    }

    public Action<MXEventParams<float>> PlayerTakeDamage;
    public void RaiseOnPlayerTakeDamage(MXEventParams<float> e)
    {
        PlayerTakeDamage?.Invoke(e);
    }

    public Action PlayerDeath;
    public void RaiseOnPlayerDeath()
    {
        PlayerDeath?.Invoke();
    }

    public Action<MXEventParams<int>> PlayerEarnCoin;
    public void RaiseOnPlayerEarnCoin(MXEventParams<int> e)
    {
        PlayerEarnCoin?.Invoke(e);
    }

    public Action<MXEventParams<float>> PlayerHeal;
    public void RaiseOnPlayerHeal(MXEventParams<float> e)
    {
        PlayerHeal?.Invoke(e);
    }
    #endregion
}
