using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject selectLevelMenu;
    public GameObject optionsMenu;

    ///////////////////////     MAIN MENU
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SelectLevelCanvas()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        selectLevelMenu.SetActive(true);
    }


    public void Options()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }


    public void Exit()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }


    ///////////////////////     SELECT LEVEL MENU
    public void SelectLevelTutorial()
    {
        SceneManager.LoadScene(1);
    }
    public void SelectLevel1()
    {
        SceneManager.LoadScene(2);
    }
    public void SelectLevel2()
    {
        SceneManager.LoadScene(3);
    }
    public void SelectLevelBack()
    {
        selectLevelMenu.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
