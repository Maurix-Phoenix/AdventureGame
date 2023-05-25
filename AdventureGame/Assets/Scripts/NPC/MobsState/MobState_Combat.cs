using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobState_Combat : IMobState
{
    public Mob Mob { get; set; }

    public void OnEnterState(Mob mob)
    {
        Mob = mob;
    }

    public void OnExitState()
    {
        
    }

    public void OnFixedUpdateState()
    {
        
    }

    public void OnUpdateState()
    {
        //Combat here
    }

  
}
