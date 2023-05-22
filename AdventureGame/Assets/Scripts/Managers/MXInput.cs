using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MXInput : Input
{
    public string InputName;
    public KeyCode InputKey;
    public Action Pressed;
    public Action Holding;
    public Action Released;
    public bool Value;

    public MXInput(string inputName, KeyCode inputKey)
    {
        InputName = inputName;
        InputKey = inputKey;
        Value = false;
    }
    
    /// <summary>
    /// Active when button go down first time
    /// </summary>
    public void OnPressed()
    {
        if(!Value)
        {
            Value = true;
            Pressed?.Invoke();
        }
        
    }

    /// <summary>
    /// Active while button is down
    /// </summary>
    public void OnHolding()
    {
        if(Value)
        {
            Value = true;
            Holding?.Invoke();
        }
    }

    /// <summary>
    /// Active when button is being released
    /// </summary>
    public void OnReleased()
    {
        if(Value)
        {
            Value = false;
            Released?.Invoke();
        }
        
        
    }

}
