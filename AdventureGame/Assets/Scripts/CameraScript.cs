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
    private float _Speed = 4.5f;
    private bool _PlayerDead = false;

    private void OnEnable()
    {
        _EM = GameManager.Instance.EventManager;
        transform.position = StartingPoint;
        _FirstAnimationPerformed = false;

        _EM.PlayerSpawn += OnPlayerSpawn;
        _EM.PlayerDeath += OnPlayerDeath;
    }
    private void OnDisable()
    {
        _EM.PlayerSpawn -= OnPlayerSpawn;
        _EM.PlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerSpawn()
    {
        _PlayerDead = false;
        _Player = Player.Instance;
    }
    private void OnPlayerDeath()
    {
        _PlayerDead = true;
        _FirstAnimationPerformed = false;
        
    }

    private void LateUpdate()
    {
        if(!_PlayerDead)
        {
            FollowAndLookPlayer();
        }
        else
        {
            transform.Translate(Vector3.forward *( _Speed / 8) * Time.deltaTime);
        }
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
                        && (Vector3.Distance(transform.position, _Player.RigidBody.position + Offset) > 0.3))
                    {
                        _Direction = (_Player.RigidBody.position + Offset - transform.position).normalized;
                        transform.Translate(_Direction * _Speed * Time.deltaTime);
                        transform.LookAt(_Player.RigidBody.position);
                    }
                    else
                    {
                        _FirstAnimationPerformed = true;
                        transform.position = _Player.RigidBody.position + Offset;
                        _EM.RaiseOnCameraReachPosition(new MXEventParams<Vector3>(transform.position));
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
