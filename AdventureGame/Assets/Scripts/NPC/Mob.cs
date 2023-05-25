
using UnityEngine;
using UnityEngine.AI;

using static MXUtilities;
public class Mob : MonoBehaviour
{

    public MobTemplate MT;
    public Animator Animator;
    public Rigidbody Rigidbody;
    public  Vector3 StartingPos;

    private IMobState _CurrentState;

    private float _MaxHealth = 10f;
    private float _CurrentHealth;
    private float _BaseAttack = 5f;
    private float _BaseDefence = 3f;
    private float _MoveSpeed = 1f;
    private float _AttackSpeed = 1.5f;

    private float _AttackRange = 1.0f;
    private float _AggroRange = 3.0f;
    private bool _CanRunAway = false;
    private float _RunAwaySpeedMultiplier = 1.0f;

    private Vector3 _direction = Vector3.zero;
    private void OnEnable()
    {
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
            _CanRunAway = MT.CanRunAway;
            _RunAwaySpeedMultiplier = MT.RunAwaySpeedMultiplier;
        }
        else
        {
            MXDebug.Log($"{gameObject.name}: MobTemplate NOT FOUND! Using defaul values.");
        }
        _CurrentHealth = _MaxHealth;
        StartingPos = transform.position;
    }
    private void OnDisable()
    {
        
    }

    private void Start()
    {
        ChangeState(new MobState_Idle());
    }

    private void Update()
    {
        _CurrentState.OnUpdateState();

        if(Vector3.Distance(Player.Instance.transform.position, transform.position) < _AggroRange )
        {
            ChangeState(new MobState_Chase());
        }
        
    }
    private void FixedUpdate()
    {
        _CurrentState.OnFixedUpdateState();
    }

    public void ChangeState(IMobState newState)
    {
        _CurrentState?.OnExitState();
        _CurrentState = newState;
        _CurrentState?.OnEnterState(this);
    }

}
