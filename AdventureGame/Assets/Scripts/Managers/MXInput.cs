using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MXInput
{
    public string InputName;
    public KeyCode InputKey;
    public Action Pressed;
    public Action Holding;
    public Action Released;


    public MXInput(string inputName, KeyCode inputKey)
    {
        InputName = inputName;
        InputKey = inputKey;
    }
    
    /// <summary>
    /// Active when button go down first time
    /// </summary>
    public void OnPressed()
    {
        Pressed?.Invoke();
    }

    /// <summary>
    /// Active while button is down
    /// </summary>
    public void OnHolding()
    {
        Holding?.Invoke();
    }

    /// <summary>
    /// Active when button is being released
    /// </summary>
    public void OnReleased()
    {
        Released?.Invoke();
    }

}
