using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// UIManager: manages show/hide the UI
/// </summary>
public class UIManager : MonoBehaviour
{
    private GameManager _GM;
    [Header("UI")]
    [SerializeField] private GameObject UIPauseMenu;
    [SerializeField] private GameObject UIPauseMenu_ExitDungeonButton;

    [Header("UIWS")]
    [SerializeField]private GameObject UIWSLabelPrefab;
    [SerializeField]private GameObject UIWSHealthBarPrefab;

    public bool Initialize()
    {
        _GM = GameManager.Instance;
        _GM.GameStart += OnGameStart;
        return true;
    }
    private void OnDisable()
    {
        _GM.GameStart -= OnGameStart;
    }

    public void OnGameStart()
    {       

    }

    public UIWS_TempLabel CreateUIWSTempLabel(string text, Vector3 position, Transform parent, int fontSize = 32, bool temporary = true, float speed = 0.5f, float lifetime = 0.8f)
    {

        UIWS_TempLabel hl = Instantiate(UIWSLabelPrefab, position, Quaternion.identity, parent).GetComponent<UIWS_TempLabel>();
        hl.UpdateLabel(text, position, parent, fontSize,temporary, speed, lifetime);
        return hl;
    }

    public UIWS_HealthBar CreateUIWSHealthBar(Vector3 offsetPosition, Transform parent)
    {
        UIWS_HealthBar hb = Instantiate(UIWSHealthBarPrefab, parent.position + offsetPosition, Quaternion.identity).GetComponent<UIWS_HealthBar>();
        hb.Owner = parent.gameObject;
        hb.transform.SetParent(parent.transform);
        return hb;
    }   


    public void ShowUIPauseMenu()
    {
        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            UIPauseMenu.SetActive(true);


            if (SceneManager.GetActiveScene().name == "Dungeon")
            {
                UIPauseMenu_ExitDungeonButton.SetActive(true);
            }

        }


    }
    public void HideUIPauseMenu()
    {
        UIPauseMenu.SetActive(false);
    }

    public void UIPauseMenu_ButtonResumeGame()
    {
        GameManager.Instance.SetGameState(GameManager.State.Playing);
    }
    public void UIPauseMenu_ButtonRestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void UIPauseMenu_ButtonExitDungeon()
    {
        SceneManager.LoadScene("TrainingValley"); //TMP: game manager should be the one that change scenes change this after 
    }
}
