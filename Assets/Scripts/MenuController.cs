using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

    public void LoadMenu()
    {
        Application.LoadLevel(0);
    }
    public void LoadLevel()
    {
        Application.LoadLevel(1);
    }
    public void LoadCredits()
    {
        Application.LoadLevel(2);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
