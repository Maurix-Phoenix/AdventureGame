using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static AdventureGame;

public class Player : MonoBehaviour
{
    private InputManager _IM;
    private EventManager _EM;


    private Rigidbody _RigidBody;

    #region Player Movement Data
    private Vector3 _Direction = Vector3.zero;
    private float _MoveSpeed = 3.0f;

    #endregion

    #region Unity Methods
    private void Awake()
    {

    }

    private void OnEnable()
    {
        _IM = GameManager.Instance.InputManager;
        _EM = GameManager.Instance.EventManager;
        _RigidBody = GetComponent<Rigidbody>();
        SubscribeToInputs();
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        _IM = GameManager.Instance.InputManager;
        _EM = GameManager.Instance.EventManager;
        UnsubscribeToInputs();
        UnsubscribeToEvents();
    }
    private void Start()
    {
        
    }

    private void Update()
    {

    }
    #endregion

    #region Events Un/Subscription

    private void SubscribeToEvents()
    {
        _EM.TriggerAreaEnter += OnTriggerAreaEnter;
    }
    private void UnsubscribeToEvents()
    {
        _EM.TriggerAreaEnter -= OnTriggerAreaEnter;
    }

    private void OnTriggerAreaEnter(MXEventParams<TriggerType> type)
    {
        switch(type.Param)
        {
            case TriggerType.None: { break; }
            case TriggerType.AdventureGuild: 
                { 
                    Debug.Log("TRIGGERED AdventureGuild"); break; 
                }
            case TriggerType.Shop: 
                { 
                    Debug.Log("TRIGGERED Shop"); break; 
                }
            case TriggerType.Dungeon:
                { 
                    Debug.Log("TRIGGERED Dungeon"); break; 
                }
        }
    }

    #endregion

    #region Player Commands Input
    private void SubscribeToInputs()
    {
        _IM.SubscribeInput("Action", InputManager.InputType.isPressed, OnActionInput);
        _IM.SubscribeInput("Attack", InputManager.InputType.isPressed, OnAttackInput);
        _IM.SubscribeInput("MoveForward", InputManager.InputType.isHelded, OnMoveForwardInput);
        _IM.SubscribeInput("MoveBackward", InputManager.InputType.isHelded, OnMoveBackwardInput);
        _IM.SubscribeInput("MoveLeft", InputManager.InputType.isHelded, OnMoveLeftInput);
        _IM.SubscribeInput("MoveRight", InputManager.InputType.isHelded, OnMoveRightInput);
    }
    private void UnsubscribeToInputs()
    {
        _IM.UnsubscribeInput("Action", OnActionInput);
        _IM.UnsubscribeInput("Attack", OnAttackInput);
        _IM.UnsubscribeInput("MoveForward",  OnMoveForwardInput);
        _IM.UnsubscribeInput("MoveBackward",  OnMoveBackwardInput);
        _IM.UnsubscribeInput("MoveLeft",  OnMoveLeftInput);
        _IM.UnsubscribeInput("MoveRight",  OnMoveRightInput);
    }

    private void OnActionInput() 
    { 
    
    }
    private void OnAttackInput() 
    { 

    }
    private void OnMoveForwardInput() 
    {
        _Direction.z = 1;
        Move();
    }
    private void OnMoveBackwardInput() 
    {        
        _Direction.z = -1;
        Move();
    }
    private void OnMoveLeftInput() 
    { 
        _Direction.x = -1;
        Move();
    }
    private void OnMoveRightInput() 
    {
        _Direction.x = 1;
        Move();
    }
    #endregion

    #region Movement

    private void Move()
    {
        transform.LookAt(_Direction + transform.position);
        transform.Translate(Vector3.forward * Time.deltaTime * _MoveSpeed, Space.Self);

        //_RigidBody.transform.LookAt(_Direction + transform.position);
        //_RigidBody.velocity = _Direction * _MoveSpeed;
        //_RigidBody.MovePosition(_Direction + transform.position);
        
        _Direction = Vector3.zero;
    }

    #endregion


}

