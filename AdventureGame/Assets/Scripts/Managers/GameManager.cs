using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MXUtilities;


/// <summary>
/// GameManager Manages the game Current State as a State Machine and GameFlow Related Events
/// </summary>
public class GameManager : MonoBehaviour
{
    //Singleton I HATE THIS
    private static GameManager _Instance = null;
    public static GameManager Instance { get { return _Instance; } }


    //Other Managers
    public AudioManager AudioManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public EventManager EventManager { get; private set; }
    public InputManager InputManager { get; private set; }
    public AnimationManager AnimationManager { get; private set; }
    public SceneManager SceneManager { get; private set; }

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
        if(_Instance != null && _Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _Instance = this;
            DontDestroyOnLoad(_Instance.gameObject);
        }


        InitializingManagers();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void InitializingManagers()
    {
        GameFlowState = State.Initializing;
        
        //for correct script execution order refer to this:
        InputManager = GetComponentInChildren<InputManager>();
        InputManager.Initialize();
        EventManager = GetComponentInChildren<EventManager>();
        EventManager.Initialize();
        UIManager = GetComponentInChildren<UIManager>();
        UIManager.Initialize();
        AudioManager = GetComponentInChildren<AudioManager>();
        AudioManager.Initialize();
        AnimationManager = GetComponentInChildren<AnimationManager>();
        AnimationManager.Initialize();
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioManager.PlaySceneMusic(SceneManager.GetActiveScene());
    }


    private void Start()
    {
        SetGameState(State.Started);
    }

    private void OnGameStart()
    {
        GameFlowState = State.Started;
        Time.timeScale = 1;
        //Get called after Awake and Initialization
        //do starting operation here
        MXDebug.Log(gameObject.name + " " + GameFlowState.ToString());
        GameStart?.Invoke();
    }

    private void OnGamePause()
    {
        GameFlowState = State.Paused;
        Time.timeScale = 0;
        //do pause operation here
        UIManager.ShowUIPauseMenu();

        GamePause?.Invoke();
    }

    private void OnGamePlaying()
    {
        GameFlowState = State.Playing;
        Time.timeScale = 1;
        //do unpause/playing operation here
        UIManager.HideUIPauseMenu(); 

        GamePlaying?.Invoke();
    }

    private void OnGameQuitting()
    {
        GameFlowState = State.Quitting;
        //do quitting operation here save ecc...

        GameQuitting?.Invoke();

        Application.Quit();

        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }

    public void RestartGame()
    {
        UIManager.HideUIPauseMenu();
        SceneManager.LoadScene("MainMenu");
        SetGameState(State.Started);
    }
}
 