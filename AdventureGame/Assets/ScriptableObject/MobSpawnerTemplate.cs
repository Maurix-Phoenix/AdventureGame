using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Mob Spawner", menuName ="Scriptable Objects/Mobs/New Mob Spawner")]
public class MobSpawnerTemplate : ScriptableObject
{
    public GameObject Prefab;
    public string Tag = "";
    public List<MobTemplate> MobsList;
 
    public float SpawnIntervail = 10f; //in seconds
    public int InitialMobsNumber = 3; //how many enemies are spawned at the start of the game
    public int SpawnNumber = 1; //how many enemies should spawn at the same time
    public int Capacity = 10; //maximun number of current spawned enemies

    public float SpawnDistance = 3f;
}
