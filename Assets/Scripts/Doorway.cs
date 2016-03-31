using UnityEngine;
using System.Collections;

public class Doorway : MonoBehaviour {

    GameController gc;

    Camera camera;
    Transform cameraTransform;
    CameraArea camArea;
    CameraFollow camFollow;

    public bool playerInTrigger = false;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        camera = FindObjectOfType<Camera>();
        camArea = transform.parent.GetComponentInChildren<CameraArea>();
        camFollow = camera.GetComponent<CameraFollow>();
        cameraTransform = camera.transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerFeet") && !playerInTrigger)
        {
            playerInTrigger = true;
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
            }
        }
        else
        {
           gc.playerIsPeeking = false;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerFeet") && playerInTrigger)
        {
            gc.playerIsPeeking = false;
            playerInTrigger = false;
        }
    }
}
