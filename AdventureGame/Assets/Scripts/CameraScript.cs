using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Player _Player;
    private EventManager _EM;
    
    public Vector3 StartingPoint = Vector3.zero;
    public Vector3 Offset = new Vector3(0,5,-3);
    private Vector3 _Direction;
    private bool _FirstAnimationPerformed = false;
    private float _Speed = 5.0f;

    private void OnEnable()
    {
        _EM = GameManager.Instance.EventManager;
        transform.position = StartingPoint;
        _FirstAnimationPerformed = false;

        _EM.PlayerSpawn += OnPlayerSpawn;
    }
    private void OnDisable()
    {
        _EM.PlayerSpawn -= OnPlayerSpawn;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlayerSpawn()
    {
        _Player = Player.Instance;
    }

    private void LateUpdate()
    {
        FollowAndLookPlayer();
    }

    private void FollowAndLookPlayer()
    {
        if (_Player != null)
        {
            if (_Player.isActiveAndEnabled)
            {
                if (!_FirstAnimationPerformed)
                {
                    if (transform.position != (_Player.RigidBody.position + Offset)
                        && (Vector3.Distance(transform.position, _Player.RigidBody.position + Offset) > 1))
                    {
                        _Direction = (_Player.RigidBody.position + Offset) - transform.position;
                        transform.Translate(_Direction.normalized * _Speed * Time.deltaTime);
                        transform.LookAt(_Player.RigidBody.position);
                    }
                    else
                    {
                        _FirstAnimationPerformed = true;
                        transform.position = _Player.RigidBody.position + Offset;
                    }
                }
                else
                {
                    transform.position = _Player.RigidBody.position + Offset;
                    transform.LookAt(_Player.RigidBody.position);
                }
            }
        }
    }
}