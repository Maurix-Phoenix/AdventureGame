using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static MXUtilities;
using static AdventureGame;
using static AdventureGame.AGDungeons;
using UnityEngine.SceneManagement;
using System.Diagnostics.Tracing;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.VisualScripting;

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
    public Room ParentRoom = null;


    private bool _IsInDungeon = false;

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
            SpawnMob(_InitialMobsNumber, ParentRoom);
        }

    }
    void Update()
    {
        if(_IsActive)
        {
            if (!_IsInDungeon)
            {
                _SpawnT -= Time.deltaTime;
                if (_SpawnT <= 0)
                {
                    SpawnMob(_SpawnNumber, ParentRoom);
                }
            }
        }
        else
        {
            if(_IsInDungeon)
            {
                ClearMobs();
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
    public void SpawnMob(int number, Room room = null)
    {
        for(int i = 0; i < number; i++)
        {
            Vector3 newPos = new Vector3(transform.position.x + Random.Range(-_SpawnDistance, _SpawnDistance),
                                         transform.position.y+1,
                                         transform.position.z + Random.Range(-_SpawnDistance, _SpawnDistance));

            //if the room is active spawn mob basing on the room tiles
            if (ParentRoom != null)
            {
                newPos = ParentRoom.Tiles[Random.Range(0, ParentRoom.Tiles.Count)].WorldPosition;
                newPos = new Vector3(newPos.x, newPos.y + 0.5f, newPos.z);
            }

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

        if(_IsInDungeon && _MobsList.Count <= 0)
        {
            _IsActive = false;
            //LightO.gameObject.SetActive(true);
            foreach(Torch torch in ParentRoom.TorchesList)
            {
                torch.Interaction();
            }
            MXDebug.Log($"Mob Spawner deactivate in {ParentRoom.name}");
        }

    }
    private void InitializeFactory()
    {
        for(int i = 0; i < _Capacity * 2 ; i++)
        {
            GameObject mobPrefab = MST.MobsList[Random.Range(0, MST.MobsList.Count)].MobPrefab;
            Vector3 newPos = new Vector3(transform.position.x + Random.Range(-_SpawnDistance, _SpawnDistance),
                             transform.position.y + 0.5f,
                             transform.position.z + Random.Range(-_SpawnDistance, _SpawnDistance));

            //if the room is active spawn mob basing on the room tiles
            if (SceneManager.GetActiveScene().name == "Dungeon" && ParentRoom != null)
            {
                _IsInDungeon = true;
                newPos = ParentRoom.Tiles[Random.Range(0, ParentRoom.Tiles.Count)].WorldPosition;
                newPos = new Vector3(newPos.x, newPos.y + 0.5f, newPos.z);
            }
            GameObject mobObj = Instantiate(mobPrefab, newPos, Quaternion.identity);
            mobObj.transform.SetParent(transform);
            mobObj.SetActive(false);
            _MobFactory.Enqueue( mobObj );
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        
        //show the normal spawn area radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        Gizmos.DrawWireSphere(transform.position, MST.SpawnDistance);

    }
#endif
}
