using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static AdventureGame;

public class Player : MonoBehaviour
{
    private InputManager _IM;
    private EventManager _EM;
    private AnimationController _AC;

    private Animator _Animator;
    private Rigidbody _RigidBody;

    private IPlayerState _PlayerState;

    #region Player Movement Data
    private bool _CanMove = true;
    private bool _IsMoving = false;
    private bool _IsSprinting = false;
    private Vector3 _Direction = Vector3.zero;
    private float _MoveSpeed = 3.0f;
    private float _SpintSpeedAdd = 1.5f;
    private float _Speed;

    #endregion

    #region Combat Data
    private bool _IsInCombat = false;
    private bool _CombatStance = false;
    private const float _CombatStanceTimer = 5.0f;
    private float _CombatStanceT = _CombatStanceTimer;
    #endregion

    #region Player Attacks Data
    private bool _CanAttack = true;
    private float _AttackSequenceTime = 1.5f;
    private float _AttackSequenceT = 0;
    private int _AttackComboCount = 0;
    private int _AttackComboCountMAX = 3;
    private const float _AttackTime = 0.5f;
    private float _AttackT;
    // private float _BaseAttackDamage = 2.0f;
    #endregion;

    #region Unity Methods
    private void Awake()
    {

    }

    private void OnEnable()
    {
        _IM = GameManager.Instance.InputManager;
        _EM = GameManager.Instance.EventManager;
        _AC = AnimationController.Instance;
        _RigidBody = GetComponent<Rigidbody>();
        _Animator = GetComponent<Animator>();

        _Speed = _MoveSpeed;
        _AttackComboCount = 0;

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
        _PlayerState?.UpdateState();
        UpdateAttack();
        UpdateCombat();
    }

    private void FixedUpdate()
    {
        CheckDirection();
    }
    #endregion

    #region StateMachine
    public void SetPlayerState(IPlayerState newState)
    {
        _PlayerState?.ExitState();
        _PlayerState = newState;
        _PlayerState?.EnterState(this);
    }
    #endregion

    #region Events Un/Subscription

    private void SubscribeToEvents()
    {

    }
    private void UnsubscribeToEvents()
    {

    }

    #endregion

    #region Player Commands Input
    private void SubscribeToInputs()
    {
        _IM.SubscribeInput("Action", InputManager.InputType.isPressed, OnActionInput);
        _IM.SubscribeInput("Attack", InputManager.InputType.isPressed, OnAttackInput);

        _IM.SubscribeInput("Sprint", InputManager.InputType.isHelded, OnSprintInput);
        _IM.SubscribeInput("Sprint", InputManager.InputType.isReleased, OnSprintInputReleased);

        //movements inpus
        _IM.SubscribeInput("MoveForward", InputManager.InputType.isReleased, OnMoveForwardReleased);
        _IM.SubscribeInput("MoveBackward", InputManager.InputType.isReleased, OnMoveBackwardReleased);
        _IM.SubscribeInput("MoveLeft", InputManager.InputType.isReleased, OnMoveLeftReleased);
        _IM.SubscribeInput("MoveRight", InputManager.InputType.isReleased, OnMoveRightReleased);
        _IM.SubscribeInput("MoveForward", InputManager.InputType.isHelded, OnMoveForwardInput);
        _IM.SubscribeInput("MoveBackward", InputManager.InputType.isHelded, OnMoveBackwardInput);
        _IM.SubscribeInput("MoveLeft", InputManager.InputType.isHelded, OnMoveLeftInput);
        _IM.SubscribeInput("MoveRight", InputManager.InputType.isHelded, OnMoveRightInput);
    }
    private void UnsubscribeToInputs()
    {
        _IM.UnsubscribeInput("Action", OnActionInput);

        _IM.UnsubscribeInput("Attack", OnAttackInput);

        _IM.UnsubscribeInput("Sprint", OnSprintInput);
        _IM.UnsubscribeInput("Sprint", OnSprintInputReleased);

        //movements inputs
        _IM.UnsubscribeInput("MoveForward", OnMoveForwardReleased);
        _IM.UnsubscribeInput("MoveBackward", OnMoveBackwardReleased);
        _IM.UnsubscribeInput("MoveLeft", OnMoveLeftReleased);
        _IM.UnsubscribeInput("MoveRight", OnMoveRightReleased);
        _IM.UnsubscribeInput("MoveForward", OnMoveForwardInput);
        _IM.UnsubscribeInput("MoveBackward", OnMoveBackwardInput);
        _IM.UnsubscribeInput("MoveLeft", OnMoveLeftInput);
        _IM.UnsubscribeInput("MoveRight", OnMoveRightInput);
    }

    private void OnActionInput()
    {
        //do something
    }
    private void OnAttackInput()
    {
        Attack();
    }
    private void OnMoveForwardReleased()
    {
        _Direction.z = 0;
        Move();

    }
    private void OnMoveBackwardReleased()
    {
        _Direction.z = 0;
        Move();

    }
    private void OnMoveLeftReleased()
    {
        _Direction.x = 0;
        Move();

    }
    private void OnMoveRightReleased()
    {
        _Direction.x = 0;
        Move();
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
    private void OnSprintInput()
    {
        if (_IsMoving)
        {
            _Speed = _MoveSpeed + _SpintSpeedAdd;
            _IsSprinting = true;
        }
    }
    private void OnSprintInputReleased()
    {
        if (_IsMoving && _IsSprinting)
        {
            _Speed = _MoveSpeed;
            _IsSprinting = false;
        }

    }
    #endregion

    #region Movement
    private void Move()
    {
        if (_CanMove)
        {
            if (_Direction != Vector3.zero)
            {
                _IsMoving = true;
                transform.LookAt(transform.position + new Vector3(_Direction.x, 0 ,_Direction.z));
                _RigidBody.MovePosition(transform.position + _Direction * Time.fixedDeltaTime * _Speed);

                if (_IsSprinting)
                {
                    _AC.PlayAnimation(_Animator, "Player", "Sprint");
                }
                else
                {

                    _AC.PlayAnimation(_Animator, "Player", "Move");
                }
            }
            else
            {
                _IsMoving = false;
                _Direction = Vector3.zero;
                if (_CombatStance)
                {
                    _AC.PlayAnimation(_Animator, "Player", "Idle_Battle");
                }
                else
                {
                    _AC.PlayAnimation(_Animator, "Player", "Idle");
                }
            }
        }
    }
    private void CheckDirection()
    {
        if (!_IsMoving)
        {
            if (_Direction != Vector3.zero)
            {
                _IsMoving = true;
            }
            else if (_Direction == Vector3.zero)
            {
                _IsMoving = false;
            }
        }
    }
    #endregion

    #region Attacking
    private void Attack()
    {
        if (_CanAttack)
        {
            _CombatStance = true;
            if (_AttackComboCount < _AttackComboCountMAX)
            {
                _AttackComboCount++;
            }
            else
            {
                _AttackComboCount = 1;
            }

            string attackNameAnimation = $"Attack{_AttackComboCount}";
            _AC.PlayAnimation(_Animator, "Player", attackNameAnimation, 0, forceState: true);
            _AttackSequenceT = 0;
            _AttackT = _AttackTime;
        }
    }

    private void UpdateAttack()
    {

        if (_AttackComboCount > 0)
        {
            _AttackSequenceT += Time.deltaTime;
        }

        if (_AttackT > 0)
        {
            _AttackT -= Time.deltaTime;
            _CanAttack = false;
        }
        else
        {
            _CanAttack = true;
        }

        if (_AttackSequenceT > _AttackSequenceTime)
        {
            _AttackComboCount = 0;
            _AttackSequenceT = 0;
        }

        //Debug.Log($"AttackSeq = {_AttackSequenceT}\n_AttackT = {_AttackT}\ncanAttack = {_CanAttack}\nComboCount = {_AttackComboCount}");
    }
    #endregion

    #region Combat
    private void UpdateCombat()
    {
        if (_IsInCombat)
        {
            _CombatStance = true;
            _CombatStanceT = _CombatStanceTimer;
        }

        if (_CombatStance)
        {
            _CombatStanceT -= Time.deltaTime;
            if (_CombatStanceT < 0)
            {
                _CombatStance = false;
                _CombatStanceT = _CombatStanceTimer;
            }
        }

    }
    #endregion
}

