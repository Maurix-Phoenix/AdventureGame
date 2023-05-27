using UnityEngine;
using static MXUtilities;
using static AdventureGame;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public PlayerSpawningPoint SpawningPoint;
    public GameObject HitLabel;
    public Rigidbody RigidBody;
    public Transform AttackPoint;
    public TrailRenderer SwordEffectTrail;
    public LayerMask MobsLayerMask;

    private InputManager _IM;
    private EventManager _EM;
    private UIManager _UI;
    private AnimationController _AC;

    private Animator _Animator;

    private bool _Dead = false;

    #region Player Movement Data
    private bool _CanMove = true;
    private bool _IsMoving = false;
    private bool _IsSprinting = false;
    private Vector3 _MoveDirection = Vector3.zero;
    private float _MoveSpeed = 1.0f;
    private float _SpintSpeedAdd = 1.5f;
    private float _Speed;

    #endregion

    #region Combat Data
    private const float _MaxHealth = 50f;
    private float _Health = _MaxHealth;
    private float _Defence = 5f;
    private float _Attack = 5f;
    private float _AttackRange = 0.2f;
    private bool _IsInCombat = false;
    private bool _CombatStance = false;
    private bool _IsAttacking = false;
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

    #region Player Resources Data
    private int _Coins;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        _UI = GameManager.Instance.UIManager;
        _IM = GameManager.Instance.InputManager;
        _EM = GameManager.Instance.EventManager;
        _AC = AnimationController.Instance;
        RigidBody = GetComponent<Rigidbody>();
        _Animator = GetComponent<Animator>();

        _Dead = false;
        _Speed = _MoveSpeed;
        _AttackComboCount = 0;
        _Health = _MaxHealth;

        _EM.RaiseOnPlayerSpawn();

        SubscribeToInputs();
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        _UI = GameManager.Instance.UIManager;
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
        if (!_Dead)
        {
            UpdateAttack();
            UpdateCombat();
        }
    }

    private void FixedUpdate()
    {
        if(!_Dead)
        {
            Idle();
            Move();
        }
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
        PlayerAction();
    }
    private void OnAttackInput()
    {
        Attack();
    }
    private void OnMoveForwardReleased()
    {
        _MoveDirection.z = 0;
    }
    private void OnMoveBackwardReleased()
    {
        _MoveDirection.z = 0;
    }
    private void OnMoveLeftReleased()
    {
        _MoveDirection.x = 0;
    }
    private void OnMoveRightReleased()
    {
        _MoveDirection.x = 0;
    }
    private void OnMoveForwardInput()
    {
        _MoveDirection.z = 1;
    }
    private void OnMoveBackwardInput()
    {
        _MoveDirection.z = -1;
    }
    private void OnMoveLeftInput()
    {
        _MoveDirection.x = -1;
    }
    private void OnMoveRightInput()
    {
        _MoveDirection.x = 1;
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
        _Speed = _MoveSpeed;
        _IsSprinting = false;
    }
    #endregion

    #region Movement
    private void Idle()
    { 
        if(!_IsMoving && !_IsAttacking)
        {
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
    private void Move()
    {
        CheckDirection();
        if (_CanMove)
        {
            if (_MoveDirection != Vector3.zero)
            {
                _IsMoving = true;
                transform.LookAt(transform.position + new Vector3(_MoveDirection.x, 0, _MoveDirection.z));
                RigidBody.MovePosition(transform.position + _MoveDirection * Time.fixedDeltaTime * _Speed);

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
                _MoveDirection = Vector3.zero;
                Idle();
            }
        }
    }
    private void CheckDirection()
    {
        if (!_IsMoving)
        {
            if (_MoveDirection != Vector3.zero)
            {
                _IsMoving = true;
            }
            else if (_MoveDirection == Vector3.zero)
            {
                _IsMoving = false;
            }
        }
    }
    #endregion

    #region Combat
    #region Attacking
    private void Attack()
    {
        if (_CanAttack)
        {
            _IsAttacking = true;
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
            
            float totalDamage = _Attack;
            if(Random.value > 0.7f)
            {
                totalDamage = totalDamage * 2;
            }

            

            Collider[] mobsHit = Physics.OverlapSphere(AttackPoint.position, _AttackRange, MobsLayerMask);
            foreach(Collider col in mobsHit)
            {
                if(col.gameObject.GetComponent<Mob>())
                {
                    Mob mob = col.gameObject.GetComponent<Mob>();
                    if (mob != null)
                    {
                        MXDebug.Log($"Hit: {mob.MT.Name}");
                        mob.TakeDamage(totalDamage);
                        
                    }
                }
            }

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
            _IsAttacking = false;
            _CanAttack = true;
        }

        if (_AttackSequenceT > _AttackSequenceTime)
        {
            _AttackComboCount = 0;
            _AttackSequenceT = 0;
        }

        //MXDebug.Log($"AttackSeq = {_AttackSequenceT}\n_AttackT = {_AttackT}\ncanAttack = {_CanAttack}\nComboCount = {_AttackComboCount}");
    }
    #endregion
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

    public void TakeDamage(float d)
    {
        if (!_Dead)
        {
            float totalDamage = d - (_Defence / 2);
            if (totalDamage > 0)
            {
                _EM.RaiseOnPlayerTakeDamage(new MXEventParams<float>(totalDamage));
                _Health -= totalDamage;

                _AC.PlayAnimation(_Animator, "Player", "GetHit");
                _UI.CreateWorldLabel($"-{totalDamage.ToString("N1")}", transform.position, transform);
                //damage taken sound

                if (_Health <= 0)
                {
                    //death method
                    StartCoroutine(Kill());
                }

            }
            else
            {
                //maybe a "damage blocked" sound?
            }
        }
    }

    public IEnumerator Kill()
    {
        _Dead = true;
        _EM.RaiseOnPlayerDeath();
        _AC.PlayAnimation(_Animator, "Player", "Die", forceState: true);
        yield return MXProgramFlow.EWait(_Animator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);

    }

    #endregion

    #region Player Actions

    private void PlayerAction()
    {
        MXDebug.Log("Action Key Pressed");
    }

    #endregion

    #region Resources

    public void AddCoins(int coins)
    {
        _Coins += coins;
        _EM.RaiseOnPlayerEarnCoin(new MXEventParams<int>(coins));
        _UI.CreateWorldLabel($"+{coins} G", transform.position, transform, lifetime: 1f);
    }

    #endregion
    private void OnDrawGizmosSelected()
    {
        //checking the attack range
        if(AttackPoint != null)
        {
            Gizmos.DrawSphere(AttackPoint.position, _AttackRange);
        }
       
    }
}

