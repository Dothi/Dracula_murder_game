using UnityEngine;
using System.Collections;

public class TextCamera : MonoBehaviour {

    Camera mainCamera;
    Camera camera;

	void Start ()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camera = GetComponent<Camera>();
	}

	void Update ()
    {
        camera.orthographicSize = mainCamera.orthographicSize;
	}
}
