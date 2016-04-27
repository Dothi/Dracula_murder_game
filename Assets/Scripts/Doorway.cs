using UnityEngine;
using System.Collections;

public class Doorway : MonoBehaviour {

    GameController gc;

    Transform cameraTransform;
    CameraArea camArea;
    CameraFollow camFollow;

    public GameObject door;
    SpriteRenderer doorSprite;

    public bool playerInTrigger = false;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        doorSprite = door.transform.Find("Sprite (door)").GetComponent<SpriteRenderer>();

        camArea = transform.parent.GetComponentInChildren<CameraArea>();
        camFollow = Camera.main.GetComponent<CameraFollow>();
        cameraTransform = Camera.main.transform;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!gc.playerNearCloset)
        {
            if (other.CompareTag("PlayerFeet") && !playerInTrigger)
            {
                playerInTrigger = true;
                doorSprite.sprite = door.GetComponent<Door>().highlightSprite;   
            }            
        }
        else
        {
            gc.playerIsPeeking = false;
            playerInTrigger = false;
            doorSprite.sprite = door.GetComponent<Door>().normalSprite;
            if (doorSprite.color != new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 1f))
            {
                if (doorSprite.color != new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 0.0f)) // = some has opened the door
                {
                    doorSprite.color = new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 1f);
                }
            }
        }
    }
    
    void Update()
    {
        if (Input.GetButton("Hide / Peek"))
        {
            if (playerInTrigger)
            {
                camFollow.cameraEndPos = new Vector3(transform.parent.position.x,
                                        transform.parent.position.y,
                                        cameraTransform.position.z);
                camFollow.zoomEndValue = camArea.cameraZoom / Camera.main.aspect;

                gc.playerIsPeeking = true;
                camArea.currentRoom = true;

                if (doorSprite.color != new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 0.5f))
                {
                    if (doorSprite.color != new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 0.0f)) // = some has opened the door
                    {
                        doorSprite.color = new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 0.5f);
                    }
                }
            }
        }
        else
        {
           gc.playerIsPeeking = false;

           if (doorSprite.color != new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 1f))
           {
               if (doorSprite.color != new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 0.0f)) // = some has opened the door
               {
                   doorSprite.color = new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 1f);
               }
           }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerFeet") && playerInTrigger)
        {
            gc.playerIsPeeking = false;
            playerInTrigger = false;
            doorSprite.sprite = door.GetComponent<Door>().normalSprite;
            if (doorSprite.color != new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 1f))
            {
                if (doorSprite.color != new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 0.0f)) // = some has opened the door
                {
                    doorSprite.color = new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, 1f);
                }            
            }
        }
    }
}
