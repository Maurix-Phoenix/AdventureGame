using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemalePlayer : MonoBehaviour
{
    #region Move Data
    private bool _IsMoving;

    #endregion

    #region Combat Data
    private bool _IsAttacking;
    private bool _CombatStance;
    #endregion

    private AnimationController _AC;
    private Animator _Animator;
    private void OnEnable()
    {
       _Animator = GetComponent<Animator>();
        _AC = AnimationController.Instance;
    }
    private void OnDisable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Idle();
    }

    private void Idle()
    {
        if (!_IsMoving && !_IsAttacking)
        {
            if (_CombatStance)
            {
                _AC.PlayAnimation(_Animator, "Player", "Idle_Battle");
            }
            else
            {
                _AC.PlayAnimation(_Animator, "Player", "Idle");
            }
        }
    }
}
