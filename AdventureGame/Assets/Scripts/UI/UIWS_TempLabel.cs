using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIWS_TempLabel : MonoBehaviour
{
    
    private TMP_Text Text;
    public string LabelText = "";
    public float LifeTime = 0.8f;
    public float Speed = 0.5f;

    private void OnEnable()
    {
        Text = GetComponentInChildren<TMP_Text>();
    }
    private void OnDisable()
    {
        Text.text = null;
    }
    private void Update()
    {
        Text.text = LabelText;
        if (LifeTime > 0)
        {
            LifeTime -= Time.deltaTime;
            gameObject.transform.Translate(0, 1 * Speed * Time.deltaTime, 0);

            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
