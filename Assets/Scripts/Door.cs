using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    SpriteRenderer doorSprite;

    public Sprite normalSprite;
    public Sprite highlightSprite;

    void Start()
    {
        doorSprite = transform.Find("Sprite (door)").GetComponent<SpriteRenderer>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyFeet") ||
            other.gameObject.CompareTag("PlayerFeet"))
        {
            if (doorSprite.color != new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 0.0f))
            {
                doorSprite.color = new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 0.0f);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyFeet") ||
            other.gameObject.CompareTag("PlayerFeet"))
        {
            doorSprite.color = new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 1.0f);
        }
	}
}
