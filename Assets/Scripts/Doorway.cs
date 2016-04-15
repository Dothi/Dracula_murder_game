using UnityEngine;
using System.Collections;

public class Doorway : MonoBehaviour {

    GameController gc;

    Camera camera;
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

        camera = camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camArea = transform.parent.GetComponentInChildren<CameraArea>();
        camFollow = camera.GetComponent<CameraFollow>();
        cameraTransform = camera.transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerFeet") && !playerInTrigger)
        {
            playerInTrigger = true;
            doorSprite.sprite = door.GetComponent<Door>().highlightSprite;
        }
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (playerInTrigger)
            {
                camFollow.cameraEndPos = new Vector3(transform.parent.position.x,
                                        transform.parent.position.y,
                                        cameraTransform.position.z);
                camFollow.zoomEndValue = camArea.cameraZoom / camera.aspect;

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
