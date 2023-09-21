using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string Name { get; set; }
    public void ShowPromptLabel();
    public void HidePromptLabel();
    public void Interaction();

}
