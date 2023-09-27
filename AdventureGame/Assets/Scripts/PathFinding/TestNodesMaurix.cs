using MXNodes;
using System.Linq;
using UnityEngine;

public class TestNodesMaurix : MonoBehaviour
{
    public NodePath _NodePath;
    public float Speed = 0.5f;
    public float TargetDestinationMinRange = 0.1f;
    public Vector3 Destination = Vector3.zero;

    private void OnEnable()
    {
        _NodePath = gameObject.GetComponent<NodePath>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetRandomDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if(_NodePath != null)
        {
            if(Vector3.Distance(transform.position,Destination) > TargetDestinationMinRange)
            {
                _NodePath.MoveSpeed = Speed;
                _NodePath.MoveToDestination(Speed);
            }
            else
            {
                SetRandomDestination();
            }

        }

    }

    void SetRandomDestination()
    {
        if( _NodePath != null)
        {
            Destination = _NodePath.SetDestination(Dungeon.Instance.Tiles[Random.Range(0, Dungeon.Instance.Tiles.Count - 1)].WorldPosition);
        }
    }
}
