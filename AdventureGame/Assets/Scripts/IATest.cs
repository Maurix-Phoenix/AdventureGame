using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IATest : MonoBehaviour
{
    NavMeshAgent agent;

    public Vector3 StartingPos = Vector3.zero;
    public Vector3 DestinationPos = Vector3.zero;

    public float TimeToMove = 5.0f;
    public float currTime;
    public bool CanMove = false;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        StartingPos = transform.position;
        currTime = TimeToMove;
    }
    private void Update()
    {

            currTime -= Time.deltaTime;
            if (currTime < 0)
            {
                NewDestination();
                Move();
            }
    }

    private void NewDestination()
    {
        DestinationPos = Player.Instance.transform.position;
        CanMove = true;
    }

    private void Move()
    {

        agent.SetDestination(DestinationPos);

        if(Vector3.Distance(DestinationPos, transform.position) < 1.5 )
        {
            CanMove = false;
            currTime = TimeToMove;
        }
    }
}
