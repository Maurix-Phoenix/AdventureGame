using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public List<MXInput> InputsList = new List<MXInput>();
    public bool Initialize()
    {
        //add the standard inputs here
        AddInput(new MXInput("Action", KeyCode.E));
        AddInput(new MXInput("Attack", KeyCode.F));
        
        AddInput(new MXInput("MoveForward", KeyCode.W));
        AddInput(new MXInput("MoveBackward", KeyCode.S));
        AddInput(new MXInput("MoveLeft", KeyCode.A));
        AddInput(new MXInput("MoveRight", KeyCode.D));
        return true;
    }

    private void Update()
    {
        foreach (MXInput input in InputsList)
        {
            if (input != null)
            {
                if(Input.GetKey(input.InputKey))
                {
                    input.OnHolding();
                }
                if (Input.GetKeyDown(input.InputKey))
                {
                    input.OnPressed();
                }
                if(Input.GetKeyUp(input.InputKey))
                {
                    input.OnReleased();
                }
            }
        }
    }


    public enum InputType
    {
        isPressed,
        isHelded,
        isReleased,
    }

    #region Input Methods

    public void AddInput(MXInput input)
    {
        if(!InputExists(input))
        {
            InputsList.Add(input);
        }
    }
    public void RemoveInput(MXInput input)
    {
        if (InputExists(input))
        {
            InputsList.Remove(input);
        }
    }
    public bool InputExists(MXInput input)
    {
        if(InputsList.Count > 0)
        {
            for(int i = 0; i < InputsList.Count; i++)
            {
                if (InputsList[i] == input)
                {
                    return true;
                }

                if (InputsList[i].InputKey == input.InputKey)
                {
                    return true;
                }

                if (InputsList[i].InputName == input.InputName)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public MXInput GetInput(KeyCode kc)
    {
        foreach(MXInput input in InputsList)
        {
            if(input.InputKey == kc)
            {
                return input;
            }
        }
        return null;
    }
    public MXInput GetInput(string inputName)
    {
        foreach (MXInput input in InputsList)
        {
            if (input.InputName == inputName)
            {
                return input;
            }
        }
        return null;
    }
    public void Rebind(MXInput input, KeyCode newKC)
    {
        if(InputExists(input))
        {
            InputsList[InputsList.IndexOf(input)].InputKey = newKC;
        }
    }
    public void SubscribeInput(string inputName, InputType type , Action subscriber)
    {
        MXInput input = GetInput(inputName);
        if(input != null)
        {
            switch (type)
            {
                case InputType.isPressed: 
                    {
                        input.Pressed += subscriber;
                        break; 
                    }
                case InputType.isHelded: 
                    {
                        input.Holding += subscriber;
                        break; 
                    }
                case InputType.isReleased: 
                    {
                        input.Released += subscriber;
                        break; 
                    }
            }
        }
    }
    public void UnsubscribeInput(string inputName, Action subscriber)
    {
        MXInput input = GetInput(inputName);
        if (input != null)
        {
            input.Pressed -= subscriber;
            input.Holding -= subscriber;
            input.Released -= subscriber;
        }
    }
    #endregion
}
