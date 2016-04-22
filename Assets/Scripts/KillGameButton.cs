using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class LetterSprites
{
    public Sprite A;
    public Sprite S;
    public Sprite D;
    public Sprite Z;
    public Sprite X;
    public Sprite C;
}

public class KillGameButton : MonoBehaviour {

    public LetterSprites sprites = new LetterSprites();
    float fadeRange = 250;

    Image image;
    KeyCode keyCode = KeyCode.A;
    string letter = "A";
    public int r {get {return Random.Range(0, 6);}}

	void Start ()
    {
        image = GetComponent<Image>();
	}	

	void Update ()
    {
        ColorFade();
	}

    public void RandomizeLetter()          //Maybe implement it so that it doesn't random same letter many times in a row
    {
        
        switch (r)
        {
            case 0:
                image.sprite = sprites.A;
                keyCode = KeyCode.A;
                letter = "A";
                break;
            case 1:
                image.sprite = sprites.S;
                keyCode = KeyCode.S;
                letter = "S";
                break;
            case 2:
                image.sprite = sprites.D;
                keyCode = KeyCode.D;
                letter = "D";
                break;
            case 3:
                image.sprite = sprites.Z;
                keyCode = KeyCode.Z;
                letter = "Z";
                break;
            case 4:
                image.sprite = sprites.X;
                keyCode = KeyCode.X;
                letter = "X";
                break;
            case 5:
                image.sprite = sprites.C;
                keyCode = KeyCode.C;
                letter = "C";
                break;
            default:
                break;
        }
    }
    
    public KeyCode getKeyCode()
    {
        return keyCode;
    }

    void ColorFade()
    {
        if (transform.position.x == transform.parent.position.x)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }
        else if (transform.position.x < transform.position.x + fadeRange && transform.position.x > transform.parent.position.x)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - (transform.localPosition.x / fadeRange));
        }
    }
}
