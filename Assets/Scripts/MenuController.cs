using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{

    GameController gc;
    GameObject pauseMenu;
    GameObject credits;
    public AudioClip clickSound;
    AudioSource audioSource;

    void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        audioSource = GetComponent<AudioSource>();
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
        if (Application.loadedLevel == 0)
        {
            Time.timeScale = 1;
            PlayClickSound();
            Invoke("LoadAfterSound", 1f);
            
        }

        if (Application.loadedLevel == 1)
        {
            LoadAfterSound();
        }
    }
    public void LoadAfterSound()
    {
        Application.LoadLevel(1);
    }
    public void ExitGame()
    {
        PlayClickSound();
        Invoke("ExitAfterSound", 1f);
    }
    public void ExitAfterSound()
    {
        Application.Quit();
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void ToggleCredits()
    {
        PlayClickSound();
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
        audioSource.clip = clickSound;
        audioSource.Play();
    }
}
