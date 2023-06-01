
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

using static MXUtilities;
public class Mob : MonoBehaviour
{
    private EventManager _EM;
    private UIManager _UI;
    private AnimationManager _ANIMM;

    public MobSpawner Spawner;
    public MobTemplate MT;
    public Animator Animator;
    public Rigidbody Rigidbody;
    public  Vector3 StartingPos;

    private IMobState _CurrentState;

    private bool _PlayerIsDead = false;
    private bool _Dead = false;
    private float _MaxHealth = 10f;
    private float _CurrentHealth;
    private float _BaseAttack = 5f;
    private float _BaseDefence = 3f;
    private float _MoveSpeed = 1f;
    private float _AttackSpeed = 1.5f;

    private float _AttackRange = 1.5f;
    private float _AggroRange = 3.0f;

    private Vector3 _direction = Vector3.zero;

    private UIWS_HealthBar _HealthBar;

    private void OnEnable()
    {
        _UI = GameManager.Instance.UIManager;
        _EM = GameManager.Instance.EventManager;
        _ANIMM = GameManager.Instance.AnimationManager;

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();

        if(MT != null)
        {
            _MaxHealth = MT.Health;
            _BaseAttack = MT.Attack;
            _BaseDefence = MT.Defence;
            _MoveSpeed = MT.MoveSpeed;
            _AttackSpeed = MT.AttackSpeed;

            _AttackRange = MT.AttackRange;
            _AggroRange = MT.AggroRange;
        }
        else
        {
            MXDebug.Log($"{gameObject.name}: MobTemplate NOT FOUND! Using defaul values.\nAssign the MobTemplate or the Mob will not work properly!");
        }
        _CurrentHealth = _MaxHealth;
        StartingPos = transform.position;


        if( _HealthBar != null )
        {
            _HealthBar.gameObject.SetActive( true );
        }
        else
        {
            _HealthBar = _UI.CreateUIWSHealthBar(new Vector3(0, -0.2f, 0), transform);
        }
        _HealthBar.UpdateHealthBar(_CurrentHealth, _MaxHealth);


        SubscribeEvents();
    }
    private void OnDisable()
    {
        _HealthBar.gameObject.SetActive( false );
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _EM.PlayerDeath += OnPlayerDeath;
        _EM.PlayerSpawn += OnPlayerSpawn;
    }

    private void UnsubscribeEvents()
    {
        _EM.PlayerDeath -= OnPlayerDeath;
        _EM.PlayerSpawn -= OnPlayerSpawn;

    }

    private void Start()
    {
        ChangeState(new MobState_Idle());
    }

    private void Update()
    {
        if(!_Dead)
        {
            _CurrentState.OnUpdateState();

            if (Vector3.Distance(Player.Instance.transform.position, transform.position) < _AggroRange &&
               Vector3.Distance(Player.Instance.transform.position, transform.position) > _AttackRange &&
               !_PlayerIsDead)
            {
                ChangeState(new MobState_Chase());
            }
        }

        if(transform.position.y < -5)
        {
            StartCoroutine(Kill());
        }
        
    }
    private void FixedUpdate()
    {
        if (!_Dead)
        {
            _CurrentState.OnFixedUpdateState();
        }
    }

    public void ChangeState(IMobState newState)
    {
        _CurrentState?.OnExitState();
        _CurrentState = newState;
        _CurrentState?.OnEnterState(this);
    }

    public IMobState GetCurrentState()
    {
        return _CurrentState;
    }

    public void TakeDamage(float d)
    {
        if(!_Dead)
        {
            float totalDamage = d - (_BaseDefence / 2);
            _CurrentHealth -= totalDamage;
            //take damage sound
            _ANIMM.PlayAnimation(Animator, MT.Name, "GetHit");
            _UI.CreateUIWSTempLabel(totalDamage.ToString("N1"), transform.position, transform);

            _HealthBar.UpdateHealthBar(_CurrentHealth, MT.Health);

            if (_CurrentHealth <= 0)
            {
                StartCoroutine(Kill());
            }
        }

    }
    public IEnumerator Kill()
    {
        _Dead = true;

        _HealthBar.UpdateHealthBar(_CurrentHealth, MT.Health, "DEAD");

        //Play Sound here...

        _ANIMM.PlayAnimation(Animator, MT.Name, "Die", forceState: true);
        yield return MXProgramFlow.EWait(Animator.GetCurrentAnimatorClipInfo(0).Length);

        //Loot
        if (MT.CoinsLoot.Count > 0)
        {
            Vector3 coinPos = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
            Instantiate(MT.CoinsLoot[Random.Range(0, MT.CoinsLoot.Count)].CoinPrefab, coinPos, Quaternion.identity);
        }

        if (Spawner != null )
        {
            Spawner.Renqueue(this);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void OnPlayerSpawn()
    {
        _PlayerIsDead = false;
    }
    private void OnPlayerDeath()
    {
        _PlayerIsDead = true;
        ChangeState(new MobState_Idle());
    }

}
