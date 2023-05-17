using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingVillage : MonoBehaviour
{
    public Player Player;
    public Transform PlayerStartingPoint;

    private void Awake()
    {
        PlayerStartingPoint = GameObject.Find("PlayerStartingPoint").transform;
    }

    void Start()
    {
        SpawnPlayer();
    }

   
    void Update()
    {
        
    }

    void SpawnPlayer()
    {
        Player.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
    }
}
