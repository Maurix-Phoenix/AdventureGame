using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public CoinTemplate CT; 
    private int _Value = 1;
    private float _RotationSpeed = 50f;
    private EventManager _EM;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        _EM = GameManager.Instance.EventManager;
        if(CT != null)
        {
            CT.Value = _Value;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        transform.Rotate(0,1 * _RotationSpeed * Time.deltaTime,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.AddCoins(_Value);

            Destroy(gameObject);
        }
    }
}
