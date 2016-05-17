using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

    GameController gc;
    GameObject pauseMenu;
    GameObject credits;

    void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenu");
    }
    void Start()
    {
        if (Application.loadedLevel == 1)
        {
            gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        if (Application.loadedLevel == 0)
        {
            credits = GameObject.Find("Credits");
            credits.SetActive(false);
        }
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    public void LoadMenu()
    {
        Application.LoadLevel(0);
    }
    public void LoadLevel()
    {
        Application.LoadLevel(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        gc.GetComponent<BloodBar>().enabled = true;
        pauseMenu.SetActive(false);
    }
    public void ToggleCredits()
    {
        if (!credits.activeInHierarchy)
        {
            credits.SetActive(true);
        }
        else
        {
            credits.SetActive(false);
        }
    }
    public void PlayClickSound()
    {

    }
}
