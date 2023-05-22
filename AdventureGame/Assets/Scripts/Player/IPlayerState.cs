using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public Player Player { get; set; }
    public void EnterState(Player player);
    public void UpdateState();
    public void ExitState();
}
