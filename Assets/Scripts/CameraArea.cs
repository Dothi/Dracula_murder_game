﻿using UnityEngine;
using System.Collections;

public class CameraArea : MonoBehaviour {

    Transform playerTransform;
    GameController gc;

    Camera camera;
    CameraFollow cameraFollow;
    Transform cameraTransform;

    float roomWidth;
    float roomHeight;
    
    Vector2 roomBoundsMin;
    Vector2 roomBoundsMax;

    public float cameraZoom;
    public LayerMask playerMask;

    SpriteRenderer fade;
    public bool currentRoom;
    float lerpTime = 0.6f;
    float currentLerpTime;
    bool resetLerp;
    Color fadeStartValue;
    Color fadeEndValue;
    Color lastFadeEndValue;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        camera = FindObjectOfType<Camera>();
        cameraFollow = camera.GetComponent<CameraFollow>();
        cameraTransform = camera.transform;

        roomBoundsMin = new Vector2(GetComponent<BoxCollider2D>().bounds.min.x, GetComponent<BoxCollider2D>().bounds.min.y);
        roomBoundsMax = new Vector2(GetComponent<BoxCollider2D>().bounds.max.x, GetComponent<BoxCollider2D>().bounds.max.y);

        fade = transform.parent.FindChild("Fade").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        SetCurrentRoomAndCamera();
        FadeRooms();
    }
    void SetCurrentRoomAndCamera()
    {
        if (!gc.playerIsPeeking)
        {
            if (Physics2D.OverlapArea(roomBoundsMin, roomBoundsMax, playerMask) != null &&
                Physics2D.OverlapArea(roomBoundsMin, roomBoundsMax, playerMask).gameObject.CompareTag("PlayerFeet"))
            {
                cameraFollow.cameraEndPos = new Vector3(transform.parent.position.x,
                                                        transform.parent.position.y,
                                                        cameraTransform.position.z);
                cameraFollow.zoomEndValue = cameraZoom / camera.aspect;
                currentRoom = true;
            }
            else
            {
                currentRoom = false;
            }
        }
    }
    void FadeRooms()
    {
        if (currentRoom)
        {
            if (resetLerp)
            {
                currentLerpTime = 0;
                resetLerp = false;
                fadeStartValue = fade.color;
            }

            fadeEndValue = new Color(fade.color.r, fade.color.g, fade.color.b, 0.0f);

            currentLerpTime += Time.deltaTime;

            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float perc = currentLerpTime / lerpTime;
            
            Color newFadeValue = Color.Lerp(fadeStartValue, fadeEndValue, perc);

            fade.color = newFadeValue;
        }
        else if(!gc.playerInCloset)
        {
            if (!resetLerp)
            {
                currentLerpTime = 0;
                resetLerp = true;
                fadeStartValue = fade.color;
            }

            fadeEndValue = new Color(fade.color.r, fade.color.g, fade.color.b, 1.0f);

            currentLerpTime += Time.deltaTime;

            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float perc = currentLerpTime / lerpTime;

            Color newFadeValue = Color.Lerp(fadeStartValue, fadeEndValue, perc);

            fade.color = newFadeValue;
        }
    }
}