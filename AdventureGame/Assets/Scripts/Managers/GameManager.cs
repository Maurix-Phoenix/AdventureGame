using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// GameManager Manages the game Current State as a State Machine and GameFlow Related Events
/// </summary>
public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Instance {  get; private set; }

    //Other Managers
    public AudioManager AudioManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public EventManager EventManager { get; private set; }
    public InputManager InputManager { get; private set; }

    //GameFlow Events 
    public Action GameStart;
    public Action GamePause;
    public Action GamePlaying;
    public Action GameQuitting;

    //GameFlow State
    public enum State
    {
        None,
        Initializing,
        Started,
        Playing,
        Paused,
        Quitting,
    }
    public State GameFlowState { get;  private set; }

    public void SetGameState(State gameFlowState)
    {
        switch (gameFlowState)
        {
            case State.None: { return; }
            case State.Initializing: { InitializingManagers(); break; }
            case State.Started: { OnGameStart(); break; }
            case State.Playing: { OnGamePlaying(); break; }
            case State.Paused: { OnGamePause(); } break;
            case State.Quitting: { OnGameQuitting(); break; }
            default: return;
        }
    }

    private void Awake()
    {
        //Singleton
        if(Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance.gameObject);
            InitializingManagers();
        }
    }

    private void InitializingManagers()
    {
        GameFlowState = State.Initializing;
        AudioManager = GetComponentInChildren<AudioManager>();
        AudioManager.Initialize();
        InputManager = GetComponentInChildren<InputManager>();
        InputManager.Initialize();
        EventManager = GetComponentInChildren<EventManager>();
        EventManager.Initialize();
        UIManager = GetComponentInChildren<UIManager>();
        UIManager.Initialize();
    }

    private void Start()
    {
            SetGameState(State.Started);
    }

    private void OnGameStart()
    {
        GameFlowState = State.Started;
        //Get called after Awake and Initialization
        //do starting operation here
        Debug.Log(gameObject.name + " " + GameFlowState.ToString());
        GameStart?.Invoke();
    }

    private void OnGamePause()
    {
        GameFlowState = State.Paused;
        //do pause operation here

        GamePause?.Invoke();
    }

    private void OnGamePlaying()
    {
        GameFlowState = State.Playing;
        //do unpause/playing operation here

        GamePlaying?.Invoke();
    }

    private void OnGameQuitting()
    {
        GameFlowState = State.Quitting;
        //do quitting operation here

        GameQuitting?.Invoke();
    }



}
 