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

    [Header("InCombat")]
    public float AttackRange = 1.0f;
    public float AggroRange = 3.0f;
    public bool CanRunAway = false;
    public float RunAwaySpeedMultiplier = 1.0f;

    [Header("OutOfCombat")]
    public bool CanRecoverHealth = false;
    public float RecoveringHealthMultiplier = 1.0f;
}