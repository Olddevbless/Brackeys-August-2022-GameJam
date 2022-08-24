using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameManager instance;
    public GameObject pauseMenu;
    public bool pauseMenuIsActive;
    public GameObject optionsPanel;
    public bool optionsPanelIsActive;
    void Awake()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        optionsPanel = GameObject.Find("Options Panel");
        pauseMenu = GameObject.Find("PauseMenu");
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);


        pauseMenu.SetActive(false);
        pauseMenuIsActive = false;
        optionsPanelIsActive = false;
        optionsPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
            
    }
   void PauseMenu()
    {
        pauseMenuIsActive = !pauseMenuIsActive;
        if (pauseMenuIsActive)
        {
            Debug.Log("pausing");
            Debug.Log(Time.timeScale);
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
            
    }
   public void Quit()
    {
        Application.Quit();
    }
   public  void Resume()
    {
        pauseMenuIsActive = false;
        pauseMenu.SetActive(false);
    }
  public  void MainMenu()
    {
        SceneManager.LoadScene(0);
        bool mainMenuScene = true;
        pauseMenuIsActive = false;
        pauseMenu.SetActive(false);
        if (mainMenuScene)
        {
            pauseMenu.SetActive(false);
            //REMEMBER TO SET MAINMENUSCENE BOOL TO FALSE WHEN GAME STARTS

        }



    }
   public void Options()
    {
        optionsPanelIsActive = !optionsPanelIsActive;
        if (optionsPanelIsActive)
        {
            optionsPanel.SetActive(true);
        }
        else
        {
            optionsPanel.SetActive(false);
        }

    }
}
