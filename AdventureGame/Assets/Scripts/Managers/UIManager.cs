using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject HitLabelPrefab;
    public bool Initialize()
    {
        return true;
    }

    public void CreateWorldLabel(string text, Vector3 position, Transform parent, float speed = 0.5f, float lifetime = 0.8f)
    {
        UIWorldLabel hl = Instantiate(HitLabelPrefab, position, Quaternion.identity, parent).GetComponent<UIWorldLabel>();
        hl.LabelText = text;
        hl.Speed = speed;
        hl.LifeTime = lifetime;
    }
}
