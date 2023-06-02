using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MXUtilities;

/// <summary>
/// Input Manager: Custom Inputs related Events
/// </summary>
public class InputManager : MonoBehaviour
{
    public List<MXInput> InputsList = new List<MXInput>();
    public bool Initialize()
    {
        //add the standard inputs here
        AddInput(new MXInput("PauseMenu", KeyCode.Escape));

        AddInput(new MXInput("Action", KeyCode.E));
        AddInput(new MXInput("Attack", KeyCode.F));
        AddInput(new MXInput("Sprint", KeyCode.LeftShift));
        AddInput(new MXInput("Jump", KeyCode.Space));
        
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

                    OnHolding(input);
                }
                if (Input.GetKeyDown(input.InputKey))
                {

                    OnPressed(input);
                }
                if(Input.GetKeyUp(input.InputKey))
                {

                    OnReleased(input);
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

    private void AddInput(MXInput input)
    {
        if(!InputExists(input))
        {
            InputsList.Add(input);
        }
        else
        {
            MXDebug.Log($"{input} input already exists!");
        }
    }
    private void RemoveInput(MXInput input)
    {
        if (InputExists(input))
        {
            InputsList.Remove(input);
        }
    }
    private bool InputExists(MXInput input)
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
    private MXInput GetInput(KeyCode kc)
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
    private MXInput GetInput(string inputName)
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


    private void OnPressed(MXInput input)
    {
        input.OnHolding();

        //do other input action here
        if(input.InputName == "PauseMenu" && SceneManager.GetActiveScene().name != "MainMenu")
        {
            if(GameManager.Instance.GameFlowState == GameManager.State.Paused)
            {
                GameManager.Instance.SetGameState(GameManager.State.Playing);
            }
            else
            {
                GameManager.Instance.SetGameState(GameManager.State.Paused);
            }
        }
    }
    private void OnHolding(MXInput input)
    {
        input.OnPressed();

        //do other input action here

    }
    private void OnReleased(MXInput input)
    {
        input.OnReleased();

        //do other input action here

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
