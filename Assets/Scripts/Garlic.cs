using UnityEngine;
using System.Collections;

public class Garlic : MonoBehaviour {

    public float bloodDecreaseValue = 3;
    bool playerInRange = false;

    GameObject player;
    Collider2D playerFeet;

    BloodBar bb;
    UnityEngine.UI.Image bloodSprite;

    public Sprite normalBlood;
    public Sprite garlicBlood;
    public RuntimeAnimatorController normalWave;
    public RuntimeAnimatorController garlicBubble;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerFeet = player.transform.Find("Collider").GetComponent<BoxCollider2D>();

        bb = GameObject.FindGameObjectWithTag("GameController").GetComponent<BloodBar>();
        bloodSprite = GameObject.Find("BloodSlider").transform.Find("Fill Area").transform.Find("Fill").GetComponent<UnityEngine.UI.Image>();
	}

    void Update()
    {
        if (playerInRange)
        {
            bb.BloodDecrease(bloodDecreaseValue);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == playerFeet)
        {
            bloodSprite.sprite = garlicBlood;
            bb.SetWaveAnim(garlicBubble);

            playerInRange = true;
            bb.SetNearGarlic(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == playerFeet)
        {
            bloodSprite.sprite = normalBlood;
            bb.SetWaveAnim(normalWave);

            playerInRange = false;
            bb.SetNearGarlic(false);
        }
    }
}
