using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Coin", menuName = "Scriptable Objects/Resources/New Coin")]

public class CoinTemplate : ScriptableObject
{
    public GameObject CoinPrefab;
    public int Value;
}
