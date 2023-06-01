using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UIManager: manages show/hide the UI
/// </summary>
public class UIManager : MonoBehaviour
{
    public GameObject UIWSLabelPrefab;
    public GameObject UIWSHealthBarPrefab;
    public bool Initialize()
    {
        return true;
    }

    public void CreateUIWSTempLabel(string text, Vector3 position, Transform parent, float speed = 0.5f, float lifetime = 0.8f)
    {
        UIWS_TempLabel hl = Instantiate(UIWSLabelPrefab, position, Quaternion.identity, parent).GetComponent<UIWS_TempLabel>();
        hl.LabelText = text;
        hl.Speed = speed;
        hl.LifeTime = lifetime;
    }

    public UIWS_HealthBar CreateUIWSHealthBar(Vector3 offsetPosition, Transform parent)
    {
        UIWS_HealthBar hb = Instantiate(UIWSHealthBarPrefab, parent.position + offsetPosition, Quaternion.identity).GetComponent<UIWS_HealthBar>();
        hb.Owner = parent.gameObject;
        hb.transform.SetParent(parent.transform);
        return hb;
    }   
}
