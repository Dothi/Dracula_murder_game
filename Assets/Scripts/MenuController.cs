using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{

    GameController gc;
    GameObject pauseMenu;
    GameObject credits;
    GameObject tutorial;
    Image tutImg;
    public List<Sprite> tutorialSprites;
    int currentTutSprite = 1;
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
            tutorial = GameObject.Find("Tutorial");
            tutImg = tutorial.GetComponent<Image>();
            credits.SetActive(false);
            tutorial.SetActive(false);
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
        CloseTutOrCred(tutorial);
    }
    public void ToggleTutorial()
    {
        PlayClickSound();
        if (!tutorial.activeInHierarchy)
        {
            tutorial.SetActive(true);
            currentTutSprite = 0;
            tutImg.sprite = tutorialSprites[currentTutSprite];
        }
        else
        {
            tutorial.SetActive(false);
        }
        CloseTutOrCred(credits);
    }
    public void PlayClickSound()
    {
        audioSource.clip = clickSound;
        audioSource.Play();
    }
    public void NextTutorialSprite()
    {
        PlayClickSound();

        if (currentTutSprite == tutorialSprites.Count - 1)
        {
            currentTutSprite = 0;
        }
        else
        {
            currentTutSprite++;
        }
        tutImg.sprite = tutorialSprites[currentTutSprite];
    }
    void CloseTutOrCred(GameObject tutOrCred)
    {
        if (tutOrCred.activeInHierarchy)
        {
            tutOrCred.SetActive(false);
        }
    }
}
