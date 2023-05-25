
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static MXUtilities;

public class MobState_Idle : IMobState
{
    private string _Name;
    private float _IdleTime = 5.0f;
    private float _IdleT;
    private float _MoveTime = 1f;
    private float _MoveT;
    private bool _CanMove;

    private Vector3 _Direction;
    private Vector3 _Destination;
    public Mob Mob { get; set; }

    public void OnEnterState(Mob mob)
    {
        Mob = mob;
        _Name = Mob.MT.Name;
        _IdleT = _IdleTime;
        _MoveT = _MoveTime;
        _CanMove = false;
        _Direction = Vector3.zero;
        _Destination = Vector3.zero;
    }

    public void OnExitState()
    {

    }

    public void OnFixedUpdateState()
    {
        Move();
    }

    public void OnUpdateState()
    {
        _IdleT -= Time.deltaTime;
        if(_IdleT < 0 )
        {
            if (!_CanMove)
            {
                _Destination = new Vector3(Mob.transform.position.x + Random.Range(-1, 1),
                           Mob.transform.position.y,
                           Mob.transform.position.z + Random.Range(-1, 1));
                _Direction = (_Destination - Mob.transform.position).normalized;

                _CanMove = true;
            }
        }
    }

    private void Move()
    {
        if(_CanMove)
        {
            _MoveT -= Time.fixedDeltaTime;
            MXDebug.Log("MOVING...");
            Mob.transform.LookAt(_Destination);
            Mob.Rigidbody.MovePosition(Mob.transform.position + _Direction * Time.fixedDeltaTime * Mob.MT.MoveSpeed);

            //apply move animation here
            AnimationController.Instance.PlayAnimation(Mob.Animator, _Name, "Move", 0, false, true);

            if (_MoveT < 0 )
            {

                //Deactivate move aniamtion here
                AnimationController.Instance.PlayAnimation(Mob.Animator, _Name, "Idle", 0, false, true);


                _Destination = Vector3.zero;
                _Direction = Vector3.zero;
                _IdleT = _IdleTime;

                _MoveT = _MoveTime;
                _CanMove=false;
            }

        }
    }
}