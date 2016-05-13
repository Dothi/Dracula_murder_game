using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{

    SpriteRenderer doorSprite;
    SpriteRenderer doorText;
    AudioSource audio;
    public AudioClip Open;
    public AudioClip Close;
    public Sprite normalSprite;
    public Sprite highlightSprite;

    void Start()
    {
        doorSprite = transform.Find("Sprite (door)").GetComponent<SpriteRenderer>();
        doorText = transform.Find("Text").GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyFeet") ||
            other.gameObject.CompareTag("PlayerFeet"))
        {
            if (doorSprite.color != new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 0.0f))
            {
                audio.clip = Open;
                audio.Play();
                doorSprite.color = new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 0.0f);
                doorText.enabled = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyFeet") ||
            other.gameObject.CompareTag("PlayerFeet"))
        {
            audio.clip = Close;
            audio.Play();
            doorSprite.color = new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 1.0f);
        }
    }
}
