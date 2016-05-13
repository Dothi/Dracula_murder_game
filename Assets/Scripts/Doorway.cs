using UnityEngine;
using System.Collections;

public class Doorway : MonoBehaviour {

    GameController gc;
    KillScript ks;

    Transform cameraTransform;
    CameraArea camArea;
    Transform camPos;
    CameraFollow camFollow;

    public GameObject door;
    SpriteRenderer doorSprite;
    SpriteRenderer doorText;

    public bool playerInTrigger = false;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        ks = GameObject.FindGameObjectWithTag("Player").GetComponent<KillScript>();
        doorSprite = door.transform.Find("Sprite (door)").GetComponent<SpriteRenderer>();
        doorText = door.transform.Find("Text").GetComponent<SpriteRenderer>();
        doorText.enabled = false;

        camArea = transform.parent.GetComponentInChildren<CameraArea>();
        camPos = camArea.transform.parent.Find("CameraPosition");
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
                doorText.enabled = true;
            }            
        }
        else
        {
            gc.playerIsPeeking = false;
            playerInTrigger = false;
            doorSprite.sprite = door.GetComponent<Door>().normalSprite;
            doorText.enabled = false;

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
        if (Input.GetButton("Hide / Peek") && !ks.isSuckingBlood)
        {
            if (playerInTrigger)
            {
                camFollow.cameraEndPos = new Vector3(camPos.position.x,
                                        camPos.position.y,
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
            doorText.enabled = false;

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
