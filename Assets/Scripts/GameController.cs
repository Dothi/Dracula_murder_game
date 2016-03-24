using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class GameController : MonoBehaviour {

    public bool gameOver = false;
    GameObject playAgainButton;

	void Start ()
    {
        playAgainButton = GameObject.Find("Button_PlayAgain");
	}
	

	void Update ()
    {
        if (gameOver && Time.timeScale != 0)
        {
            playAgainButton.SetActive(true);
            playAgainButton.GetComponentInChildren<Text>().text = "You Lose - Play Again";
            Time.timeScale = 0;
        }
        else if (!gameOver)
        {
            if (playAgainButton.activeInHierarchy == true)
            {
                playAgainButton.SetActive(false);
            }
            if (Time.timeScale != 1)
            {
                Time.timeScale = 1; 
            }                       
        }
	}
    public void PlayAgain()
    {
        gameOver = false;
        Application.LoadLevel(Application.loadedLevel);
    }
}
