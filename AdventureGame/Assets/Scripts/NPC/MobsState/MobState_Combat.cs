using System.Collections;
using UnityEngine;
using static MXUtilities;
public class MobState_Combat : IMobState
{
    public Mob Mob { get; set; }
    private bool _CanAttack = false;
    private float _AttackT;
    private int _AttackCount = 0;

    public void OnEnterState(Mob mob)
    {
        Mob = mob;
        _AttackT = mob.MT.AttackSpeed;
        _AttackCount = 0;

        Mob.StartCoroutine(Combat());
    }

    public void OnExitState()
    {
        
    }

    public void OnFixedUpdateState()
    {
        
    }

    public void OnUpdateState()
    {
    }

    private IEnumerator Combat()
    {
        while (Mob.GetCurrentState() == this)
        {
            Player player = Player.Instance;
            if (player.gameObject.activeSelf)
            {
                Mob.transform.LookAt(player.transform.position);
                if (Vector3.Distance(player.transform.position, Mob.transform.position) <= Mob.MT.AttackRange)
                {
                    _AttackT -= Time.deltaTime;
                    if (_AttackT < 0)
                    {
                        _CanAttack = true;
                    }
                    else
                    {
                        //idle animation
                        AnimationController.Instance.PlayAnimation(Mob.Animator, Mob.MT.Name, $"Idle_Battle");
                    }

                    if (_CanAttack)
                    {
                        _AttackCount++;
                        if (_AttackCount > 2)
                        {
                            _AttackCount = 1;
                        }
                        //attack animation
                        AnimationController.Instance.PlayAnimation(Mob.Animator, Mob.MT.Name, $"Attack{_AttackCount}");

                        float attackDamage = Mob.MT.Attack;
                        float totalAttack = attackDamage;

                        if (Random.value >= 0.9) //randomcrit 10%
                        {
                            totalAttack = totalAttack * 2;
                        }
                        
                        Player.Instance.TakeDamage(totalAttack);

                        yield return MXProgramFlow.EWait(0.8f); //givin the animation the time to play
                        _CanAttack = false;
                        _AttackT = Mob.MT.AttackSpeed;
                    }
                }
                else
                {
                    Mob.ChangeState(new MobState_Chase());
                }
            } 
            yield return null;
        }
    }

  
}