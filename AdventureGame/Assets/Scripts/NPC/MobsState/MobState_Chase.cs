using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class MobState_Chase : IMobState
{
    private AnimationManager _ANIMM;
    public Mob Mob { get; set; }

    public void OnEnterState(Mob mob)
    {
        _ANIMM = GameManager.Instance.AnimationManager;
           Mob = mob;
    }

    public void OnExitState()
    {
       
    }

    public void OnFixedUpdateState()
    {
        Move(Player.Instance);
    }

    public void OnUpdateState()
    {
       
    }

    private void Move(Player player)
    {
        Vector3 _Destination = new Vector3(player.transform.position.x,
                                           player.transform.position.y,
                                           player.transform.position.z);
        Vector3 _Direction = (_Destination - Mob.transform.position).normalized;

        Mob.transform.LookAt(_Destination);
        Mob.Rigidbody.MovePosition(Mob.transform.position + _Direction * Time.fixedDeltaTime * Mob.MT.MoveSpeed);
        _ANIMM.PlayAnimation(Mob.Animator, Mob.MT.Name, "Move");

        if(Vector3.Distance(_Destination, Mob.transform.position) <= Mob.MT.AttackRange)
        {
            Mob.ChangeState(new MobState_Combat());
        }
       
        if(Vector3.Distance(_Destination, Mob.transform.position) >= Mob.MT.AggroRange)
        {
            
            _Destination = new Vector3(Mob.StartingPos.x, 0, Mob.StartingPos.z);
            _Direction=(_Destination - Mob.transform.position).normalized;

            Mob.transform.LookAt(_Destination);
            Mob.Rigidbody.MovePosition(Mob.transform.position + _Direction * Time.fixedDeltaTime * Mob.MT.MoveSpeed);
            if (Vector3.Distance(_Destination, Mob.transform.position) < 1)
            {
                Mob.ChangeState(new MobState_Idle());
            }
        }
    }
}
