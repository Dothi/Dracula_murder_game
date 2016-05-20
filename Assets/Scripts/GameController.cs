﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public bool gameOver = false;
    bool gameWon = false;
    //public bool gameWon = false;
    string gameOverMessage = "-";
    GameObject endMenu;
    Text endMsg;
    Image gameOverText;
    public List<GameObject> enemies;
    public List<GameObject> pausedEnemies;
    public Sprite youWin;
    public Sprite youLose;


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
        endMenu = GameObject.Find("EndMenu");
        endMsg = endMenu.transform.Find("EndMessage").GetComponent<Text>();
        gameOverText = endMenu.transform.Find("GameOverText").GetComponent<Image>();
        endMenu.SetActive(false);
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
            if (!endMenu.activeInHierarchy)
            {
                endMenu.SetActive(true);
            }
            if (endMsg.text != gameOverMessage)
            {
                endMsg.text = gameOverMessage;
            }
            if (gameWon)
            {
                if (gameOverText.sprite != youWin)
                {
                    gameOverText.sprite = youWin;
                }
            }
            else
            {
                if (gameOverText.sprite != youLose)
                {
                    gameOverText.sprite = youLose;
                }
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
    public void GameOver(bool gameWon, string endMsg/*, Vector2 cameraLocation, float cameraZoom*/)
    {
        this.gameOver = true;
        this.gameWon = gameWon;
        this.gameOverMessage = endMsg;

        //Camera.main.GetComponent<CameraFollow>().cameraEndPos = cameraLocation;
        //Camera.main.GetComponent<CameraFollow>().zoomEndValue = cameraZoom;
    }
    public void PlayAgain()
    {
        gameOver = false;
        Application.LoadLevel(Application.loadedLevel);
    }
}
