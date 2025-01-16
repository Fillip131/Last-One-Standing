using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject canvasPlayerUI;
    public GameObject canvasPauseMenu;
    private bool isPaused = false;

    public GameObject MainText;
    public GameObject buttonResume;
    public GameObject buttonExit;
    public GameObject HintMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (HintMenu.activeSelf)
            {
                return;
            }

            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        canvasPlayerUI.SetActive(false);
        canvasPauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        canvasPlayerUI.SetActive(true);
        canvasPauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /////////////////////////////////////       BUTTONS

    public void ButtonResume()
    {
        ResumeGame();
    }

    public void ButtonExit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ButtonHint()
    {
        MainText.SetActive(false);
        buttonExit.SetActive(false);
        buttonResume.SetActive(false);
        HintMenu.SetActive(true);
    }

    public void ButtonHintBack()
    {
        MainText.SetActive(true);
        buttonExit.SetActive(true);
        buttonResume.SetActive(true);
        HintMenu.SetActive(false);
    }
}