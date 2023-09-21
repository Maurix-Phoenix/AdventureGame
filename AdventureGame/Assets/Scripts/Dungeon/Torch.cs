using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, IInteractable
{
    [Header("Status")]
    public bool IsOn = false;

    [Header("Light")]
    [SerializeField]private Light _Light;
    [SerializeField]private float _baseRange = 3.0f;
    [SerializeField]private float _rangeVariation = 1.0f;
    [SerializeField]private float _baseIntensity = 1.0f;
    [SerializeField]private float _IntensityVariation = 0.3f;
    [SerializeField]private float _Frequency = 1f;
    [SerializeField] private float _StepAtLimit = 1.5f;

    [Header("Interaction Label")]
    [SerializeField]private UIWS_TempLabel _InteractionLabel;
    [SerializeField]private string _InteractionLabelText;
    
    private ParticleSystem _ParticleS;

    public string Name { get =>_Name; set => _Name = value; }
    private string _Name = "Torch";

    public void Interaction()
    {
        IsOn = !IsOn;
        if (IsOn)
        {
            _ParticleS.Play();
            _Light.gameObject.SetActive(true);
            _Light.range = _baseRange;
            _Light.intensity = _baseIntensity;
        }
        else
        {
            _ParticleS.Stop();
            _Light.gameObject.SetActive(false);
        }
    }

    public void ShowPromptLabel()
    {
         _InteractionLabel.gameObject.SetActive(true);
    }

    public void HidePromptLabel()
    {
         _InteractionLabel.gameObject.SetActive(false);
    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        _ParticleS = transform.GetComponentInChildren<ParticleSystem>();
        _ParticleS.Stop();
        _Light.gameObject.SetActive(false);
        _InteractionLabel = GameManager.Instance.UIManager.CreateUIWSTempLabel("Press 'E' to interact", _ParticleS.transform.position, transform, 32, false, 0, 0);
        HidePromptLabel();
    }

    private void Update()
    {
        if (IsOn)
        {
            _Light.range +=  Random.Range(-_rangeVariation, _rangeVariation) * Time.deltaTime * _Frequency;
            _Light.intensity += Random.Range(-_IntensityVariation, _IntensityVariation) * Time.deltaTime * _Frequency;

            if(_Light.intensity > _baseIntensity + _IntensityVariation)
            {
                _Light.intensity -= _IntensityVariation * _StepAtLimit;

            }
            else if(_Light.intensity < _baseIntensity - _IntensityVariation)
            {
                _Light.intensity += _IntensityVariation * _StepAtLimit;
            }

            if(_Light.range >= _baseRange + _rangeVariation)
            {
                _Light.range -= _rangeVariation * _StepAtLimit;
            }
            else if(_Light.range < _baseRange - _IntensityVariation)
            {
                _Light.range += _rangeVariation *  _StepAtLimit;
;
            }

        }
    }
}
