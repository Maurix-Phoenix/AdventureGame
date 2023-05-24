using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MXUtilities;
public class MobSpawner : MonoBehaviour
{
    public MobSpawnerTemplate MST;

    private float _SpawnIntervail = 10f;
    private int _InitialEnemiesNumber = 3;
    private int _EnemiesNumber;
    private int _SpawnNumber = 1;
    private int _Capacity = 3;
    private float _SpawnDistance = 3;

    private void OnEnable()
    {
        if (MST != null)
        {
            _SpawnIntervail = MST.SpawnIntervail;
            _InitialEnemiesNumber = MST.InitialEnemiesNumber;
            _SpawnNumber = MST.SpawnNumber;
            _Capacity = MST.Capacity;
            _SpawnDistance = MST.SpawnDistance;
        }
        else
        {
            MXDebug.Log($"{gameObject.name}: MobSpawnerTemplate NOT FOUND! Using defaul values.");
        }
    }

    private void OnDisable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
