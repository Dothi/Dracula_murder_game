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

    Canvas canvas;
    GameObject player;
    KillScript ks;

    bool gameStarted = false;

    void Start()
    {
        SetKeyCodesList();
        buttonParent = transform.Find("Buttons").gameObject;

        canvas = FindObjectOfType<Canvas>();
        player = GameObject.FindGameObjectWithTag("Player");
        ks = player.GetComponent<KillScript>();
        ks.minigame = gameObject;

        SpawnButtons();
        SetButtonPositions();

        gameStarted = true;
        gameObject.SetActive(false);
    }

	void OnEnable ()
    {
        if (gameStarted)
        {
            player.GetComponentInChildren<Animator>().enabled = false;
            SetButtonPositions();
            SetAboveCharacters(player, ks.killTarget);
            currentButton = 0;

            for (int i = 0; i < buttonList.Count; i++)
            {
                buttonList[i].GetComponent<Image>().enabled = true;
                buttonList[i].RandomizeLetter();
            }
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(CancelKey) || Input.GetKeyDown(KeyCode.Escape))
            {
                //Cancel
                ks.CancelKill();
                player.GetComponentInChildren<Animator>().enabled = true;
                Debug.Log("Minigame cancelled");
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
                            //Win
                            ks.SuccesfulKill();
                            player.GetComponentInChildren<Animator>().enabled = true;
                            Debug.Log("Minigame won!");
                            gameObject.SetActive(false);

                            break;
                        }

                        //TO DO: Maybe add smooth movement (lerp)?
                        MoveButtons();
                        break;
                    }
                    if (Input.GetKeyDown(keyCodes[i]))
                    {
                        //Fail
                        ks.CancelKill();
                        player.GetComponentInChildren<Animator>().enabled = true;
                        Debug.Log("Minigame failed!");
                        gameObject.SetActive(false);

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
            button.GetComponent<RectTransform>().localScale = Vector3.one;
            button.transform.SetParent(buttonParent.transform);
            buttonList.Add(button.GetComponent<KillGameButton>());
            button.GetComponent<KillGameButton>().SetImageComponent();
        }
    }
    void SetButtonPositions()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            GameObject button = buttonList[i].gameObject;
            button.GetComponent<RectTransform>().localScale = Vector3.one;
            if (i == 0)
            {
                //Set position of first button
                button.GetComponent<RectTransform>().position = buttonParent.GetComponent<RectTransform>().position;
            }
            else
            {                                          //Position of last button               + width of button                                 + assigned gap
                button.GetComponent<RectTransform>().position = new Vector2(buttonList[i - 1].GetComponent<RectTransform>().position.x + (button.GetComponent<RectTransform>().rect.width + buttonGap) * canvas.scaleFactor,
                                                                            buttonParent.GetComponent<RectTransform>().position.y);
            }
        }
    }
    void MoveButtons()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            KillGameButton button = buttonList[i];
            if (button.GetComponent<Image>().enabled == true)
            {
                button.GetComponent<RectTransform>().position = new Vector2(button.GetComponent<RectTransform>().position.x - (button.GetComponent<RectTransform>().rect.width + buttonGap) * canvas.scaleFactor,
                                                                            buttonParent.GetComponent<RectTransform>().position.y);
            }
        }
    }
    void SetAboveCharacters(GameObject player, GameObject target)
    {
        Vector2 pos = Vector2.Lerp(player.transform.position, target.transform.position, 0.5f);
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos);

        GetComponent<RectTransform>().anchorMax = viewportPoint; 
        GetComponent<RectTransform>().anchorMin = viewportPoint; 
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
