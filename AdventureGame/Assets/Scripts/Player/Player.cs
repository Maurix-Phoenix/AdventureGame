using UnityEngine;
using static MXUtilities;
using static AdventureGame;

using System.Collections;

using UnityEngine.SceneManagement;
using System.Linq.Expressions;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public PlayerSpawningPoint SpawningPoint;
    public Rigidbody RigidBody;
    public Transform AttackPoint;
    public TrailRenderer SwordEffectTrail;
    public LayerMask MobsLayerMask;
    public Light PlayerLight;
    public Collider PlayerNearTrigger;


    private InputManager _IM;
    private EventManager _EM;
    private UIManager _UI;
    private AnimationManager _ANIMM;
    private AudioManager _AM;
    private UIWS_HealthBar _HealthBar = null;
    private AudioSource _AudioSource;

    private Animator _Animator;

    public bool IsDead = false;

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
    public int Coins { get { return _Coins; } }
    #endregion

    #region Player Dungeon Data
    public Room CurrentRoom { get; private set; }
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
        _ANIMM = GameManager.Instance.AnimationManager;
        _AM = GameManager.Instance.AudioManager;

        RigidBody = GetComponent<Rigidbody>();
        _Animator = GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();

        _CanMove = false;
        IsDead = false;
        _Speed = _MoveSpeed;
        _AttackComboCount = 0;
        _Health = _MaxHealth;

        if (_HealthBar != null)
        {
            _HealthBar.gameObject.SetActive(true);
        }
        else
        {
            _HealthBar = _UI.CreateUIWSHealthBar(new Vector3(0, 0.4f, 0), transform);
        }

        _HealthBar.UpdateHealthBar(_Health, _MaxHealth);

        _EM.RaiseOnPlayerSpawn();

        SubscribeToInputs();
        SubscribeToEvents();

        if (SceneManager.GetActiveScene().name == "TrainingValley")
        {
            PlayerLight.gameObject.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name != "Dungeon")
        {
            PlayerLight.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _UI = GameManager.Instance.UIManager;
        _IM = GameManager.Instance.InputManager;
        _EM = GameManager.Instance.EventManager;

        _HealthBar.gameObject.SetActive(false);

        UnsubscribeToInputs();
        UnsubscribeToEvents();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Dungeon")
        {
            CheckPlayerPositionInDungeon();
        }

        if (!IsDead)
        {
            UpdateAttack();
            UpdateCombat();
        }


    }

    private void FixedUpdate()
    {
        if (!IsDead)
        {
            Idle();
            Move();
        }
    }
    #endregion

    #region Events Un/Subscription

    private void SubscribeToEvents()
    {
        _EM.CameraReachPosition += OnCameraReachPlayer;

    }
    private void UnsubscribeToEvents()
    {

        _EM.CameraReachPosition -= OnCameraReachPlayer;
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

    private void OnCameraReachPlayer(MXEventParams<Vector3> position)
    {
        _CanMove = true;
    }

    private void Idle()
    {
        if (!_IsMoving && !_IsAttacking)
        {
            if (_CombatStance)
            {
                _ANIMM.PlayAnimation(_Animator, "Player", "Idle_Battle");
            }
            else
            {
                _ANIMM.PlayAnimation(_Animator, "Player", "Idle");
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
                    _ANIMM.PlayAnimation(_Animator, "Player", "Sprint");
                }
                else
                {

                    _ANIMM.PlayAnimation(_Animator, "Player", "Move");
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
            _AM.PlaySFXLocal(_AudioSource, $"PlayerAttack{_AttackComboCount}");
            _ANIMM.PlayAnimation(_Animator, "Player", attackNameAnimation, 0, forceState: true);

            float totalDamage = _Attack;
            if (Random.value > 0.7f)
            {
                totalDamage = totalDamage * 2;
            }



            Collider[] mobsHit = Physics.OverlapSphere(AttackPoint.position, _AttackRange, MobsLayerMask);
            foreach (Collider col in mobsHit)
            {
                if (col.gameObject.GetComponent<Mob>())
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
        if (!IsDead)
        {
            float totalDamage = d - (_Defence / 2);
            if (totalDamage > 0)
            {
                _EM.RaiseOnPlayerTakeDamage(new MXEventParams<float>(totalDamage));
                _Health -= totalDamage;

                _ANIMM.PlayAnimation(_Animator, "Player", "GetHit");
                _UI.CreateUIWSTempLabel($"-{totalDamage.ToString("N1")}", transform.position, transform);
                _HealthBar.UpdateHealthBar(_Health, _MaxHealth);
                //damage taken sound

                if (_Health <= 0)
                {
                    //death method
                    _HealthBar.UpdateHealthBar(_Health, _MaxHealth, "DEATH");
                    StartCoroutine(Kill());
                }
            }
            else
            {
                //maybe a "damage blocked" sound?
                _UI.CreateUIWSTempLabel($"Miss", transform.position, transform);
            }
        }
    }

    public IEnumerator Kill()
    {
        IsDead = true;
        _EM.RaiseOnPlayerDeath();
        _ANIMM.PlayAnimation(_Animator, "Player", "Die", forceState: true);
        yield return MXProgramFlow.EWait(_Animator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
    }

    #endregion

    #region Player Actions

    private void PlayerAction()
    {
        MXDebug.Log("Action Key Pressed");

        RaycastHit[] hitResults = new RaycastHit[5];
        Physics.BoxCastNonAlloc(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), new Vector3(0.2f, 0.3f, 0.2f), Vector3.forward, hitResults, Quaternion.identity, 0.2f);
        foreach(RaycastHit other in hitResults)
        {
            if(other.collider != null)
            {
                if (other.transform.root.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    interactable.Interaction();
                }
            }
        }


        






        //Ray ray = new Ray(new Vector3(RigidBody.position.x, RigidBody.position.y + 0.1f, transform.position.z), transform.forward);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, 0.5f))
        //{
        //    MXDebug.Log($"hit: {hit.transform.gameObject.name}");
        //    if (hit.collider != null)
        //    {
        //        if (hit.transform.parent.TryGetComponent(out IInteractable interactable))
        //        {
        //            interactable.Interaction();
        //            MXDebug.Log($"hit Interactable: {hit.transform.gameObject.name}");
        //        }
        //    }
        //}
    }

    #endregion

    #region Resources

    public void AddCoins(int coins)
    {
        _Coins += coins;
        _EM.RaiseOnPlayerEarnCoin(new MXEventParams<int>(coins));
        _UI.CreateUIWSTempLabel($"+{coins} G", transform.position, transform, lifetime: 1f);
    }

    public void Heal(float h)
    {
        if (_Health + h < _MaxHealth)
        {
            _Health += h;
        }
        else if (_Health + h >= _MaxHealth)
        {
            _Health = _MaxHealth;
        }
        _EM.RaiseOnPlayerHeal(new MXEventParams<float>(h));

        _UI.CreateUIWSTempLabel($"+Heal {h}", transform.position, transform);
        _HealthBar.UpdateHealthBar(_Health, _MaxHealth);

    }

    #endregion

    #region Dungeon

    private void CheckPlayerPositionInDungeon()
    {
        Ray ray = new Ray(new Vector3(RigidBody.position.x, RigidBody.position.y, transform.position.z), -transform.up);
        RaycastHit hit;



        if (Physics.Raycast(ray, out hit, 0.1f))
        {
            if (hit.collider != null)
            {
                MXDebug.Log($"Player Ground: {hit.collider.name}");
                if (hit.transform.parent.parent.TryGetComponent(out Room room))
                {
                    if (room != null)
                    {
                        CurrentRoom = room;
                        MXDebug.Log($"CurrentRoom: {room.name}");
                    }
                }
            }
        }
    }

    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            MXDebug.Log($"Showing Label {other.transform.root}");
            interactable.ShowPromptLabel();
        }
    }
    private void OnTriggerExit(Collider other) 
    { 
        if(other.transform.root.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            MXDebug.Log($"Hiding Label {other.transform.root}");
            interactable.HidePromptLabel();
        }
    }


#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), new Vector3(0.2f, 0.2f, 0.2f));

        //checking the attack range
        if (AttackPoint != null)
        {
            Gizmos.DrawSphere(AttackPoint.position, _AttackRange);
        }

    }
#endif
}

