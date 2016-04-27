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
    public int r {get {return Random.Range(0, 6);}}

	void Update ()
    {
        ColorFade();
	}

    public void RandomizeLetter()          //Maybe implement it so that it doesn't random same letter many times in a row
    {
        if (image != null)
        {
            image = GetComponent<Image>();
        }
          
        switch (r)
        {
            case 0:
                image.sprite = sprites.A;
                keyCode = KeyCode.A;
                break;
            case 1:
                image.sprite = sprites.S;
                keyCode = KeyCode.S;
                break;
            case 2:
                image.sprite = sprites.D;
                keyCode = KeyCode.D;
                break;
            case 3:
                image.sprite = sprites.Z;
                keyCode = KeyCode.Z;
                break;
            case 4:
                image.sprite = sprites.X;
                keyCode = KeyCode.X;
                break;
            case 5:
                image.sprite = sprites.C;
                keyCode = KeyCode.C;
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
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - (transform.localPosition.x / fadeRange));
    }
    public void SetImageComponent()
    {
        image = GetComponent<Image>();
    }
}
