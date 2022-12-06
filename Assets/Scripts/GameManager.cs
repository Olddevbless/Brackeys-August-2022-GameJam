using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager current;
    
    public GameManager instance;
    public GameObject pauseMenu;
    public bool pauseMenuIsActive;
    public GameObject optionsPanel;
    public bool optionsPanelIsActive;
    bool mainMenuScene;
    public int score;
    GameObject notificationPanel;
    TMP_Text notificationTxt;
    Scene mainMenu;


    void Awake()
    {

        current = this;
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        optionsPanel = GameObject.Find("Options Panel");
        pauseMenu = GameObject.Find("PauseMenu");
         //notificationPanel = GameObject.Find("NotiPanel");
         //notificationTxt = GameObject.Find("NotiTxt").GetComponent<TMP_Text>();
        //notificationPanel.SetActive(false);
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
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
        {
            mainMenuScene = true;
            pauseMenu.SetActive(false);
        }
        else
        {
            mainMenuScene = false;
        }
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
    
  public  void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        mainMenuScene = true;
        pauseMenuIsActive = false;
        pauseMenu.SetActive(false);
        if (mainMenuScene)
        {
            pauseMenu.SetActive(false);
            //REMEMBER TO SET MAINMENUSCENE BOOL TO FALSE WHEN GAME STARTS

        }
        
        



    }
   public void StartGame()
    {
        if (mainMenu != null)
            mainMenuScene= false;

        SceneManager.LoadScene(1);
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

    public IEnumerator Notice(string info)
    {
        notificationPanel.SetActive(true);
        notificationTxt.text = info;
        yield return new WaitForSeconds(3);
       notificationPanel.SetActive(false);
    }

    public void IncreaseScore(int value)
    {
        
    }
}
