using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KillMinigame : MonoBehaviour {

    List<KeyCode> keyCodes = new List<KeyCode>();
    KeyCode CancelKey = KeyCode.Space;

    int MinigameLength = 10;
    int currentButton = 0;
    float buttonGap = 2;

    public GameObject killGameButtonPrefab;
    List<KillGameButton> buttonList = new List<KillGameButton>();

    GameObject buttonParent;

    void Start()
    {
        SetKeyCodesList();
        buttonParent = transform.Find("Buttons").gameObject;

        SpawnButtons();
        SetButtonPositions();       
    }

	void OnEnable ()
    {
        //TO DO:
        //Set the minigame above player position here
        //Disable player movement and other input stuff
        //Disable target enemy
        SetButtonPositions();
        currentButton = 0;

        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponent<Image>().enabled = true;
            buttonList[i].RandomizeLetter();
        }
	}

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(CancelKey))
            {
                //TO DO: Cancel minigame
                //collapse enemy
                //enable player input
                gameObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i < keyCodes.Count; i++)
                {
                    if (Input.GetKeyDown(buttonList[currentButton].getKeyCode()))
                    {
                        //Correct button!
                        buttonList[currentButton].GetComponent<Image>().enabled = false;
                        currentButton++;
                        if (currentButton == MinigameLength)
                        {
                            //TO DO: winning
                            //disable minigame
                            //enable player input
                            //kill enemy
                            //heal player
                            Debug.Log("Minigame won!");
                            break;
                        }
                        //Move buttons -TO DO: Maybe add smooth movement (lerp)?
                        for (int j = 0; j < buttonList.Count; j++)
                        {
                            KillGameButton button = buttonList[j];
                            if (button.GetComponent<Image>().enabled == true)
                            {
                                button.transform.position = new Vector2(button.transform.position.x - (button.GetComponent<RectTransform>().rect.width + buttonGap),
                                        buttonParent.transform.position.y);
                            }
                        }
                        break;
                    }
                    if (Input.GetKeyDown(keyCodes[i]))
                    {
                        //TO DO: Fail
                        //Cancel minigame
                        //collapse enemy
                        //enable player input
                        Debug.Log("Minigame failed!");
                        break;
                    }
                }
            }
        }
	}
    void SpawnButtons()
    {
        for (int i = 0; i < MinigameLength; i++)
        {
            GameObject button = (GameObject)Instantiate(killGameButtonPrefab, buttonParent.transform.position, Quaternion.identity);
            button.transform.SetParent(buttonParent.transform);
            buttonList.Add(button.GetComponent<KillGameButton>());
        }
    }
    void SetButtonPositions()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            GameObject button = buttonList[i].gameObject;
            if (i == 0)
            {
                //Set position of first button
                button.transform.position = buttonParent.transform.position;
            }
            else
            {                                          //Position of last button               + width of button                                 + assigned gap
                button.transform.position = new Vector2(buttonList[i - 1].transform.position.x + button.GetComponent<RectTransform>().rect.width + buttonGap,
                                                        buttonParent.transform.position.y);
            }
        }
    }

    void SetKeyCodesList()
    {
        keyCodes.Add(KeyCode.A);
        keyCodes.Add(KeyCode.S);
        keyCodes.Add(KeyCode.D);
        keyCodes.Add(KeyCode.Z);
        keyCodes.Add(KeyCode.X);
        keyCodes.Add(KeyCode.C);
    }
}
