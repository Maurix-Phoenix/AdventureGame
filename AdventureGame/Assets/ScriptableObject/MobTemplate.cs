using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mob", menuName = "Scriptable Objects/Mobs/New Mob")]
public class MobTemplate : ScriptableObject
{
    public string Name = "";
    public List<string> Tags;

    [Header("Object")]
    public GameObject MobPrefab = null;

    [Header("Base Stats")]
    public float Health = 10f;
    public float Attack = 5f;
    public float Defence = 3f;
    public float MoveSpeed = 1f;
    public float AttackSpeed = 1.5f;

    [Header("InCombat")]
    public float AttackRange = 1.0f;
    public float AggroRange = 3.0f;

    [Header("Coin Loot")]
    public List<CoinTemplate> CoinsLoot = new List<CoinTemplate>();
   
    
}
