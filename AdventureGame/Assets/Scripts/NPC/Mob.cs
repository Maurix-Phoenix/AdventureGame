
using UnityEngine;
using UnityEngine.AI;

using static MXUtilities;
public class Mob : MonoBehaviour
{

    public MobTemplate MT;
    public Rigidbody Rigidbody;

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
    }
    private void FixedUpdate()
    {
        _CurrentState.OnFixedUpdateState();
    }

    private void ChangeState(IMobState newState)
    {
        
        _CurrentState?.OnExitState();
        _CurrentState = newState;
        _CurrentState?.OnEnterState(this);
    }

}
