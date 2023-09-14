using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class UIWS_TempLabel : MonoBehaviour
{
    private TMP_Text Text;
    public string LabelText = "";
    public float LifeTime = 0.8f;
    public float Speed = 0.5f;
    public int FontSize = 32;
    public bool Temporary = true;

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
        if (Temporary)
        {
            if (LifeTime > 0)
            {
                LifeTime -= Time.deltaTime;
                gameObject.transform.Translate(0, 1 * Speed * Time.deltaTime, 0);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void UpdateLabel(string text, Vector3 position, Transform parent, int fontSize = 32, bool temporary = true, float speed = 0.5f, float lifetime= 0.8f)
    {
        Text.fontSize = fontSize;
        Text.text = text;
        gameObject.transform.position = position;
        gameObject.transform.SetParent(parent);

        Temporary = temporary;
        Speed = speed;
        LifeTime = lifetime;
    }
}
