using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AdventureGame;

public class Trigger : MonoBehaviour
{
    private EventManager EM;
    public TriggerType type;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerBody")
        {
            EM = GameManager.Instance.EventManager;
            Debug.Log("Entering Trigger...");

            EM.RaiseOnTriggerAreaEnter(new MXEventParams<TriggerType>(type));
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerBody")
        {
            Debug.Log("Staying in Trigger...");
        }
    }
}
