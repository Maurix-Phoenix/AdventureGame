using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MXUtilities;
public class PlayerSpawningPoint : MonoBehaviour
{

    public GameObject PlayerPrefab;
    private EventManager _EM;
    private Player _Player;
    private Vector3 _SpawnPos;

    private void OnEnable()
    {
        _EM = GameManager.Instance.EventManager;
        _SpawnPos = transform.Find("SpawningPoint").transform.position;
    }

    private void OnDisable()
    {

    }

    void Start()
    {
        SpawnPlayer();
    }

    void Update()
    {
        
    }

    private void SpawnPlayer()
    {
        MXDebug.Log("Spawning player...");
        if(PlayerPrefab != null)
        {
            if(_Player != null)
            {
                _Player.transform.position = _SpawnPos;
                _Player.transform.LookAt(transform.position);
                _Player.gameObject.SetActive(true);
            }
            else
            {
                Instantiate(PlayerPrefab, _SpawnPos, Quaternion.identity);
                _Player = PlayerPrefab.GetComponent<Player>();
                _Player.transform.LookAt(transform.position);
            }
        }
        _EM.RaiseOnPlayerSpawn();
        
    }
}
