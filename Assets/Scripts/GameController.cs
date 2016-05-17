using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public bool gameOver = false;
    public bool gameWon = false;
    GameObject playAgainButton;
    public List<GameObject> enemies;
    public List<GameObject> pausedEnemies;

    public bool playerIsPeeking = false;
    public bool playerInCloset = false;
    public bool playerNearCloset = false;

    GameObject killMinigame;

    void Awake()
    {
        GameObject.Find("Canvas").SetActive(true);
    }
	void Start ()
    {
        playAgainButton = GameObject.Find("Button_PlayAgain");
        playAgainButton.SetActive(false);
        enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        killMinigame = GameObject.Find("Canvas").transform.Find("Kill_Minigame").gameObject;
        Time.timeScale = 1;
	}
	

	void Update ()
    {
        if (gameOver)
        {
            if (killMinigame.activeInHierarchy)
            {
                killMinigame.SetActive(false);   
            }

            if (Time.timeScale != 0)
            {
                Time.timeScale = 0;
            }
            if (!playAgainButton.activeInHierarchy)
            {
                playAgainButton.SetActive(true);
            }
            if (gameWon)
            {
                playAgainButton.GetComponentInChildren<Text>().text = "You Win! - Play Again";
            }
            else
            {
                playAgainButton.GetComponentInChildren<Text>().text = "You Lose - Play Again";
            }            
        }
        /*
        else if (!gameOver)
        {
            if (playAgainButton.activeInHierarchy == true)
            {
                playAgainButton.SetActive(false);
            }
            if (Time.timeScale != 1 && )
            {
                Time.timeScale = 1; 
            }                       
        }*/
	}
    public void PlayAgain()
    {
        gameOver = false;
        Application.LoadLevel(Application.loadedLevel);
    }
}
