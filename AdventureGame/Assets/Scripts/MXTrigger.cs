using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AdventureGame;
using static MXUtilities;

public class MXTrigger : MonoBehaviour
{
    //there can be an interface with all the triggerstype(like the mob's state machine) and their own functions.
    //change this later.

    private EventManager EM;
    public TriggerType type;


    public string SceneToLoad;
    public bool IsActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if(IsActive)
        {
            if (other.tag == "Player")
            {
                EM = GameManager.Instance.EventManager;
                EM.RaiseOnTriggerAreaEnter(new MXEventParams<TriggerType>(type)); //testing the MXEventClass

                TriggerFunction();
            }
        }
    }

    private void TriggerFunction()
    {
        switch(type)
        {
            case TriggerType.LoadScene:
                { 
                    switch(SceneToLoad)
                    {
                        case "TrainingValley":{ SceneManager.LoadScene("TrainingValley"); break; }
                        case "Dungeon": { SceneManager.LoadScene("Dungeon"); break; }
                    }
                    break;
                }
        }
    }
}
