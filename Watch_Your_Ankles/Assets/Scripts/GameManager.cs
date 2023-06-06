using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float gameTime;
    public TextMeshProUGUI gameTimeUI;

    public bool paused;
    public GameObject pauseMenu;
    public GameObject endMenu;

    public int selection;
    public bool selectDone;

    public GameObject[] selectors;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 || paused)
        {
            if (!selectDone)
            {
                if ((Gamepad.current.leftStick.value.y < -.5f || Input.GetAxisRaw("Vertical") < -.1f) && selection < selectors.Length - 1)
                {
                    selection++;
                    Select();
                }
                if ((Gamepad.current.leftStick.value.y > .5f || Input.GetAxisRaw("Vertical") > .1f) && selection > 0)
                {
                    selection--;
                    Select();
                }
            }
            if (Gamepad.current.leftStick.value.y + Input.GetAxisRaw("Vertical") == 0)
                selectDone = false;
            if (Gamepad.current.buttonSouth.wasPressedThisFrame || Input.GetMouseButtonDown(0))
                SelectPress();
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (gameTime > 0)
            {
                gameTime -= Time.deltaTime;
                gameTimeUI.text = "" + Mathf.RoundToInt(gameTime);
                if (Gamepad.current.startButton.wasPressedThisFrame || Input.GetKeyDown(KeyCode.Escape))
                {
                    if (!paused)
                    {
                        selection = 0;
                        selectDone = false;
                        pauseMenu.SetActive(true);
                        Time.timeScale = 0;
                        paused = true;
                    }
                    else
                    {
                        pauseMenu.SetActive(false);
                        Time.timeScale = 1;
                        paused = false;
                    }
                }
            }
            else
            {
                pauseMenu.SetActive(false);
                endMenu.SetActive(true);
                paused = false;
                Time.timeScale = 0;
                if (Gamepad.current.buttonSouth.wasPressedThisFrame || Input.GetMouseButtonDown(0))
                    LoadScene(0);
            }
        }
    }

    void Select()
    {
        selectDone = true;
        for (int i = 0; i < selectors.Length; i++)
        {
            if (i == selection)
                selectors[i].SetActive(false);
            else
                selectors[i].SetActive(true);
        }
    }

    void SelectPress()
    {
        if (selection == 0)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
                LoadScene(1);
            else if (paused)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                paused = false;
            }
        }
        else if (selection == 1)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
                Debug.Log("Settings Tab");
            else if (paused)
                Debug.Log("Settings Tab");
        }
        else if (selection == 2)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
                Application.Quit();
            else if (paused)
                LoadScene(0);
        }
    }

    public void LoadScene(int num)
    {
        selection = 0;
        selectDone = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(num);
    }
}
