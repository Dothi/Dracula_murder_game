using UnityEngine;
using System.Collections;

public class Doorway : MonoBehaviour {

    GameController gc;

    Camera camera;
    Transform cameraTransform;
    CameraArea camArea;
    CameraFollow camFollow;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        camera = FindObjectOfType<Camera>();
        camArea = transform.parent.GetComponentInChildren<CameraArea>();
        camFollow = camera.GetComponent<CameraFollow>();
        cameraTransform = camera.transform;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKey(KeyCode.Q) && other.CompareTag("PlayerFeet"))
        {
            camFollow.cameraEndPos = new Vector3(   transform.parent.position.x,
                                                    transform.parent.position.y,
                                                    cameraTransform.position.z);
            camFollow.zoomEndValue = camArea.cameraZoom / camera.aspect;

            gc.playerIsPeeking = true;
            camArea.currentRoom = true;
        }
        else
        {
           gc.playerIsPeeking = false;
           camArea.currentRoom = false;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerFeet"))
        {
            gc.playerIsPeeking = false;
        }
    }
}
