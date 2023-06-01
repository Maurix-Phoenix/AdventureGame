using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static MXUtilities;
public class PlayerSpawningPoint : MonoBehaviour, IInteractable
{ 

    public GameObject PlayerPrefab;
    public Light LightGO;
    private GameObject _PlayerInstanceOBJ;
    private EventManager _EM;
    private Player _Player;
    private Vector3 _SpawnPos;

    private const float _TimeToSpawn = 3.0f;
    private float _CurrentTime = _TimeToSpawn;
    private bool _PlayerSpawned = false;
    private int _HealthCharges = 4;
    private float _HealthPerCharge = 25.0f;


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
        }        
    }

    private void OnPlayerSpawn()
    {
        _PlayerSpawned = true;
    }
    private void OnPlayerDeath()
    {
        _PlayerSpawned = false;
        _CurrentTime = _TimeToSpawn;
    }

    public void Interaction()
    {
        MXDebug.Log($"{gameObject} INTERACTION TRIGGERED");
        if(_HealthCharges > 0)
        {
            _Player.Heal(_HealthPerCharge);
            _HealthCharges--;

            LightGO.intensity -= 1;
        }
    }
}
