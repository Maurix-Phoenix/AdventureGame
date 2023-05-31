using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWS_HealthBar : MonoBehaviour
{
    public GameObject Owner;
    public Vector3 Offset;
    //assigned in inspector
    private TMP_Text _HBText;
    private Slider _Slider;

    private void Awake()
    { 
        _Slider = GetComponent<Slider>();
        _HBText = gameObject.transform.GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        
    }

    void LateUpdate()
    {
        //prevent strange behaviours if in parent objects (always face camera)
        transform.LookAt(transform.position + Camera.main.transform.rotation * Camera.main.transform.forward, Camera.main.transform.rotation * Vector3.up);
        
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth, string text = null, bool showText = true)
    {
        _Slider.maxValue = maxHealth;
        _Slider.value = currentHealth;
        if (showText)
        {
            _HBText.text = currentHealth.ToString("N1");
            if (text != null)
            {
                _HBText.text = text;
            }
        }
    }
}
