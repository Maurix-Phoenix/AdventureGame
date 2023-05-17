using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AdventureGame;

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
}
