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

    private List<Mob> _MobsList = new List<Mob>();
    private Queue<GameObject> _MobFactory = new Queue<GameObject>();

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

    void Start()
    {
        InitializeFactory();
        if(_IsActive)
        {
            ClearMobs();
            SpawnMob(_InitialMobsNumber);
        }
    }
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
    }
    private void ClearMobs()
    {
        for (int i = _MobsList.Count - 1; i >= 0; i--)
        {
            Destroy(_MobsList[i].gameObject);
        }
        _MobsList.Clear();
    }
    public void SpawnMob(int number)
    {
        for(int i = 0; i < number; i++)
        {
            Vector3 newPos = new Vector3(transform.position.x + Random.Range(-_SpawnDistance, _SpawnDistance),
                                         transform.position.y+1,
                                         transform.position.z + Random.Range(-_SpawnDistance, _SpawnDistance));

            if (_MobsList.Count < _Capacity)
            {
                Mob mob = _MobFactory.Dequeue().GetComponent<Mob>();
                if (mob != null)
                {
                    mob.gameObject.transform.position = newPos;
                    mob.Spawner = this;
                    mob.gameObject.SetActive(true);
                    _SpawnT = _SpawnIntervail;
                    _MobsList.Add(mob);
                }
            }
        }
    }
    public void Renqueue(Mob mob)
    {
        int id = _MobsList.IndexOf(mob);
        _MobsList.RemoveAt(id);
        mob.gameObject.SetActive(false);
        mob.gameObject.transform.position = Vector3.zero;
        _MobFactory.Enqueue(mob.gameObject);
    }
    private void InitializeFactory()
    {
        for(int i = 0; i < _Capacity * 2 ; i++)
        {
            GameObject mobPrefab = MST.MobsList[Random.Range(0, MST.MobsList.Count)].MobPrefab;
            Vector3 newPos = new Vector3(transform.position.x + Random.Range(-_SpawnDistance, _SpawnDistance),
                             transform.position.y + 1,
                             transform.position.z + Random.Range(-_SpawnDistance, _SpawnDistance));
            GameObject mobObj = Instantiate(mobPrefab, newPos, Quaternion.identity);
            mobObj.transform.SetParent(transform);
            mobObj.SetActive(false);
            _MobFactory.Enqueue( mobObj );
        }
    }
}
