using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static MXUtilities;
public class MobSpawner : MonoBehaviour
{
    public MobSpawnerTemplate MST;

    private bool _IsActive = true; //this refers to a custom activation (not the one in Inspector) can change with events in future
    private float _SpawnIntervail = 10f;
    private float _SpawnT;
    private int _InitialMobsNumber = 3;
    private int _SpawnNumber = 1;
    private int _Capacity = 3;
    private float _SpawnDistance = 3;

    private List<GameObject> _MobsList = new List<GameObject>();

    private void OnEnable()
    {
        if (MST != null)
        {
            _SpawnIntervail = MST.SpawnIntervail;
            _InitialMobsNumber = MST.InitialMobsNumber;
            _SpawnNumber = MST.SpawnNumber;
            _Capacity = MST.Capacity;
            _SpawnDistance = MST.SpawnDistance;
        }
        else
        {
            MXDebug.Log($"{gameObject.name}: MobSpawnerTemplate NOT FOUND! Using defaul values.");
        }

        _SpawnT = _SpawnIntervail;
    }

    private void OnDisable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        if(_IsActive)
        {
            ClearMobs();
            SpawnMob(_InitialMobsNumber);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_IsActive)
        {
            _SpawnT -= Time.deltaTime;
            if (_SpawnT <= 0)
            {
                SpawnMob(_SpawnNumber);
            }
        }

        CheckMobsPosition();
    }

    private void CheckMobsPosition()
    {
        if(_MobsList.Count > 0)
        {
            for(int i = _MobsList.Count-1; i>=0 ; i--)
            {
                if(_MobsList[i].transform.position.y < -5)
                {
                    Destroy(_MobsList[i].gameObject);
                }
            }
        }
    }

    private void ClearMobs()
    {
        for (int i = _MobsList.Count - 1; i >= 0; i--)
        {
            Destroy(_MobsList[i].gameObject);
        }
        _MobsList.Clear();
    }

    private void SpawnMob(int number)
    {
        for(int i = 0; i < number; i++)
        {
            Vector3 newPos = new Vector3(transform.position.x + Random.Range(-_SpawnDistance, _SpawnDistance),
                                         transform.position.y+1,
                                         transform.position.z + Random.Range(-_SpawnDistance, _SpawnDistance));
            if (_MobsList.Count < _Capacity)
            {
                GameObject mobPrefab = MST.MobsList[Random.Range(0, MST.MobsList.Count)].MobPrefab;
                GameObject mob = Instantiate(mobPrefab, newPos, Quaternion.identity);
                mob.transform.SetParent(transform);
                _SpawnT = _SpawnIntervail;
                _MobsList.Add( mobPrefab );

                MXDebug.Log($"Spawning Mob {mobPrefab.name}");
            }
        }
    }
}
