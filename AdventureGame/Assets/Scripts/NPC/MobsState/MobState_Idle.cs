
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static MXUtilities;

public class MobState_Idle : IMobState
{
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
        MXDebug.Log("Enter state...");
        Mob = mob;
        _IdleT = _IdleTime;
        _MoveT = _MoveTime;
        _CanMove = false;
        _Direction = Vector3.zero;
        _Destination = Vector3.zero;
    }

    public void OnExitState()
    {
        MXDebug.Log("Exiting state...");

    }

    public void OnFixedUpdateState()
    {
        MXDebug.Log("Fixed Update state...");

        Move();
    }

    public void OnUpdateState()
    {
        MXDebug.Log("Update state...");
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


            if (_MoveT < 0 )
            {

                //Deactivate move aniamtion here

                _Destination = Vector3.zero;
                _Direction = Vector3.zero;
                _IdleT = _IdleTime;

                _MoveT = _MoveTime;
                _CanMove=false;
            }

        }
    }
}