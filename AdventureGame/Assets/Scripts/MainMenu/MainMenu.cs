using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject ReadmePanel;
    public GameObject StartGameButton;
    public GameObject QuitGameButton;
    private bool _readme = false;
    public void StartGameButtonClick()
    {
        SceneManager.LoadScene("TrainingValley");
    }
    public void QuitGameButtonClick()
    {
        GameManager.Instance.GameQuitting();
    }

    public void ReadMeButtonClick()
    {
        if(!_readme)
        {

           ReadmePanel.SetActive(true);
            _readme = true;
        }
        else
        {
            ReadmePanel.SetActive(false);
            _readme = false;
        }
    }
}