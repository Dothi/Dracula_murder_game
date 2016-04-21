using UnityEngine;
using System.Collections;

public class Garlic : MonoBehaviour {

    public float bloodDecreaseValue = 3;

    GameObject player;
    Collider2D playerFeet;
    KillScript killScript;

    GameController gc;
    BloodBar bb;
    UnityEngine.UI.Image bloodSprite;

    public Sprite normalBlood;
    public Sprite garlicBlood;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerFeet = player.transform.Find("Collider").GetComponent<BoxCollider2D>();
        killScript = player.GetComponent<KillScript>();

        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        bb = GameObject.FindGameObjectWithTag("GameController").GetComponent<BloodBar>();
        bloodSprite = GameObject.Find("BloodSlider").transform.Find("Fill Area").transform.Find("Fill").GetComponent<UnityEngine.UI.Image>();
        Debug.Log(bloodSprite);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == playerFeet)
        {
            bloodSprite.sprite = garlicBlood;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other == playerFeet)
        {
            bb.BloodDecrease(bloodDecreaseValue);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == playerFeet)
        {
            bloodSprite.sprite = normalBlood;
        }
    }
}
