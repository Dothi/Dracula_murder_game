using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    PlayerMovement playerMovement;

    float lerpTime = 0.6f;
    float currentLerpTime;
    Vector3 cameraStartPos;
    Vector3 lastCameraEndPos;
    public Vector3 cameraEndPos;

    Camera camera;
    float zoomStartValue;
    public float zoomEndValue;

	void Start ()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        cameraStartPos = transform.position;
        cameraEndPos = transform.position;
        camera = GetComponent<Camera>();
	}
	
	void Update ()
    {             
        MoveAndZoomCamera();   
	}
    void MoveAndZoomCamera()
    {
        if (cameraEndPos != lastCameraEndPos)
        {
            currentLerpTime = 0;
            cameraStartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            lastCameraEndPos = cameraEndPos;
            zoomStartValue = camera.orthographicSize;
        }

        currentLerpTime += Time.deltaTime;

        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float perc = currentLerpTime / lerpTime;
        Vector3 newCamPos = Vector3.Lerp(cameraStartPos, cameraEndPos, perc);
        float newZoomValue = Mathf.Lerp(zoomStartValue, zoomEndValue, perc);

        transform.position = newCamPos;
        camera.orthographicSize = newZoomValue;
    }
}
