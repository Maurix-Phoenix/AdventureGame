using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public CoinTemplate CT; 
    private int _Value = 1;
    private float _RotationSpeed = 50f;
    private bool _Attracted = false;
    private Vector3 _Direction = Vector3.zero;
    private float _MoveSpeed = 1f;

    private EventManager _EM;
    private AudioManager _AM;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        _EM = GameManager.Instance.EventManager;
        _AM = GameManager.Instance.AudioManager;
        if (CT != null)
        {
            _Value = CT.Value;
        }
        else _Value = 1;
        _Attracted = false;
    }

    // Update is called once per frame
    void Update()
    { 
        transform.Rotate(0,1 * _RotationSpeed * Time.deltaTime,0);
        if(transform.position.y < -5)
        {
            Destroy(gameObject);
        }

        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        if(Player.Instance!= null && !Player.Instance.Dead)
        {
            if (_Attracted)
            {
                _Direction = (Player.Instance.RigidBody.position - transform.position).normalized;
                transform.Translate(_Direction * _MoveSpeed * Time.deltaTime);
                if (Vector3.Distance(Player.Instance.transform.position, transform.position) < 0.2)
                {
                    _AM.PlaySFX(CT.PickupSFX);
                    Player.Instance.AddCoins(_Value);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            _Attracted = true;
        }
    }
}
