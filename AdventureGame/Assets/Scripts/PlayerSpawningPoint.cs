using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static MXUtilities;
public class PlayerSpawningPoint : MonoBehaviour, IInteractable
{ 

    public GameObject PlayerPrefab;
    public Light LightGO;
    private GameObject _PlayerInstanceOBJ;
    private EventManager _EM;
    private UIManager _UIM;
    private Player _Player;
    private Vector3 _SpawnPos;

    private UIWS_TempLabel _Label = null;

    private const float _TimeToSpawn = 3.0f;
    private float _CurrentTime = _TimeToSpawn;
    private bool _PlayerSpawned = false;
    private int _HealthCharges = 4;
    private float _HealthPerCharge = 25.0f;
    private bool _CanInteract = true;
    private const float _InteractionTime = 1.0f;
    private float _InteractionT = _InteractionTime;


    private void OnEnable()
    {
        _EM = GameManager.Instance.EventManager;
        SubscribeToEvents();

        _SpawnPos = transform.Find("SpawningPoint").transform.position;
        _CurrentTime = _TimeToSpawn;
        _PlayerInstanceOBJ = null;

    }

    private void OnDisable()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _EM.PlayerDeath += OnPlayerDeath;
        _EM.PlayerSpawn += OnPlayerSpawn;
    }
    private void UnsubscribeToEvents()
    {
        _EM.PlayerDeath -= OnPlayerDeath;
        _EM.PlayerSpawn -= OnPlayerSpawn;
    }

    void Start()
    {
        SpawnPlayer();

    }

    void Update()
    {
        if(!_PlayerSpawned)
        {
            _CurrentTime -= Time.deltaTime;
            if( _CurrentTime <= 0 )
            {
                SpawnPlayer();
            }
        }

        if(!_CanInteract)
        {
            _InteractionT -= Time.deltaTime;
            if(_InteractionT <= 0)
            {
                _CanInteract = true;
            }
        }
    }

    private void SpawnPlayer()
    {
        MXDebug.Log("Spawning player...");
        if(PlayerPrefab != null)
        {
            if(_PlayerInstanceOBJ != null)
            {
                _Player.transform.position = _SpawnPos;
                _PlayerInstanceOBJ.SetActive(true);
            }
            else
            {
                _PlayerInstanceOBJ = Instantiate(PlayerPrefab, _SpawnPos, Quaternion.identity);
                _Player = _PlayerInstanceOBJ.GetComponent<Player>();
                _Player.transform.LookAt(transform.position);
            }
            _Player.SpawningPoint = this;
            _Label = _UIM.CreateUIWSTempLabel("Press 'E' to interact", new Vector3(transform.position.x, transform.position.y + 0.18f, transform.position.z), transform, 78, false, 0, 0);
        }
    }

    private void OnPlayerSpawn()
    {
        _PlayerSpawned = true;

        _UIM = GameManager.Instance.UIManager;
    }
    private void OnPlayerDeath()
    {
        Destroy(_Label.gameObject); 
        _Label = null;
        _PlayerSpawned = false;
        _CurrentTime = _TimeToSpawn;
    }

    public void Interaction()
    {

        if (_CanInteract)
        {

            string labelText = "";
            _UIM = GameManager.Instance.UIManager;
            MXDebug.Log($"{gameObject} INTERACTION TRIGGERED");
            if (_HealthCharges > 0)
            {
                labelText = $"Remaining Charges: {_HealthCharges}";
                _Player.Heal(_HealthPerCharge);
                _HealthCharges--;
                //LightGO.intensity -= 1;
            }
            else
            {
                labelText = "No more charges!";
            }
            _UIM.CreateUIWSTempLabel(labelText, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), transform,32, true, 0.1f, 2);

            _CanInteract = false;
            _InteractionT = _InteractionTime;
        }
    }

    public void ShowPromptLabel()
    {
        if(_Label != null)
        {
            _Label.gameObject.SetActive(true);
        }
    }

    public void HidePromptLabel()
    {
        if (_Label != null)
        {
            _Label.gameObject.SetActive(false);
        }
    }
}
