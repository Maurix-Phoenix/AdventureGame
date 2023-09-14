using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, IInteractable
{
    [Header("Status")]
    public bool IsOn = false;

    [Header("Interaction Label")]
    [SerializeField]private UIWS_TempLabel _InteractionLabel;
    [SerializeField]private string _InteractionLabelText;
    
    private ParticleSystem _ParticleS;

    public void Interaction()
    {
        SwichLight();
    }

    public void ShowPromptLabel()
    {
        _InteractionLabel.gameObject.SetActive(true);
    }

    public void HidePromptLabel()
    {
        _InteractionLabel.gameObject.SetActive(false);
    }


    public void SwichLight()
    {
        IsOn = !IsOn;
        if (IsOn)
        {
            _ParticleS.Play();
        }
        else
        {
            _ParticleS.Stop();
        }
    }

    private void OnDisable()
    {
        Destroy(_InteractionLabel);
    }

    private void Start()
    {
        _ParticleS = transform.GetComponentInChildren<ParticleSystem>();
        _ParticleS.Stop();
        _InteractionLabel = GameManager.Instance.UIManager.CreateUIWSTempLabel("Press 'E' to interact", _ParticleS.transform.position, transform, 32, false, 0, 0);
    }
}
