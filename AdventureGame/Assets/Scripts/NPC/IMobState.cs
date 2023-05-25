
using UnityEngine;

public interface IMobState 
{
    public Mob Mob { get; set; }
    public void OnEnterState(Mob mob);
    public void OnUpdateState();
    public void OnFixedUpdateState();
    public void OnExitState();

}
