using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraArea : MonoBehaviour {

    GameController gc;

    CameraFollow cameraFollow;
    Transform cameraTransform;

    Transform roomCenter;

    //float roomWidth;
    //float roomHeight;
    float roomAspect;
    
    Vector2 roomBoundsMin;
    Vector2 roomBoundsMax;

    public float cameraZoom;
    public float cameraZoomH;
    public LayerMask playerMask;

    public List<GameObject> bottomWallPieces = new List<GameObject>();
    List<SpriteRenderer> bottomWallSprites = new List<SpriteRenderer>();
    Color wallFadeStartValue;
    Color wallFadeEndValue;
    
    SpriteRenderer fade;
    //SpriteRenderer fade2;
    //public SpriteRenderer fadeBelow;
    public bool currentRoom;
    float lerpTime = 0.6f;
    float currentLerpTime;
    bool resetLerp;
    Color fadeStartValue;
    Color fadeEndValue;
    Color lastFadeEndValue;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        cameraFollow = Camera.main.transform.GetComponent<CameraFollow>();
        cameraTransform = Camera.main.transform;

        roomCenter = transform.parent.Find("CameraPosition").transform;

        roomBoundsMin = new Vector2(GetComponent<BoxCollider2D>().bounds.min.x, GetComponent<BoxCollider2D>().bounds.min.y);
        roomBoundsMax = new Vector2(GetComponent<BoxCollider2D>().bounds.max.x, GetComponent<BoxCollider2D>().bounds.max.y);

        roomAspect = GetComponent<BoxCollider2D>().bounds.max.x / GetComponent<BoxCollider2D>().bounds.max.y;

        for (int i = 0; i < bottomWallPieces.Count; i++)
        {
            bottomWallSprites.AddRange(bottomWallPieces[i].GetComponentsInChildren<SpriteRenderer>());
        }

        fade = transform.parent.FindChild("Fade").GetComponent<SpriteRenderer>();
        //fade2 = transform.parent.FindChild("Fade2").GetComponent<SpriteRenderer>();

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
                cameraFollow.cameraEndPos = new Vector3(roomCenter.position.x,
                                                        roomCenter.position.y,
                                                        cameraTransform.position.z);

                if (roomAspect > Camera.main.aspect)
                {
                    cameraFollow.zoomEndValue = cameraZoom / Camera.main.aspect;                
                }
                else if (roomAspect < Camera.main.aspect)
                {
                    cameraFollow.zoomEndValue = cameraZoom / Camera.main.aspect; //en tajuu miten tän pitäs toimii lol
                }

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

                if (bottomWallSprites.Count > 0)
                {
                    wallFadeStartValue = bottomWallSprites[0].color;
                }
            }

            fadeEndValue = new Color(fade.color.r, fade.color.g, fade.color.b, 0.0f);
            if (bottomWallSprites.Count > 0)
            {
                wallFadeEndValue = new Color(1f, 1f, 1f, 0.6f);
            }

            currentLerpTime += Time.deltaTime;

            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float perc = currentLerpTime / lerpTime;
            
            Color newFadeValue = Color.Lerp(fadeStartValue, fadeEndValue, perc);

            fade.color = newFadeValue;
            /*fade2.color = newFadeValue;
            if (fadeBelow != null)
            {
                fadeBelow.color = newFadeValue;    
            }*/         

            for (int i = 0; i < bottomWallSprites.Count; i++)
            {
                bottomWallSprites[i].color = Color.Lerp(wallFadeStartValue,
                                                        wallFadeEndValue,
                                                        perc);
            }
        }
        else if(!gc.playerInCloset)
        {
            if (!resetLerp)
            {
                currentLerpTime = 0;
                resetLerp = true;
                fadeStartValue = fade.color;

                if (bottomWallSprites.Count > 0)
                {
                    wallFadeStartValue = bottomWallSprites[0].color;
                }
            }

            fadeEndValue = new Color(fade.color.r, fade.color.g, fade.color.b, 1.0f);
            if (bottomWallSprites.Count > 0)
            {
                wallFadeEndValue = new Color(1f, 1f, 1f, 1.0f);
            }

            currentLerpTime += Time.deltaTime;

            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float perc = currentLerpTime / lerpTime;

            Color newFadeValue = Color.Lerp(fadeStartValue, fadeEndValue, perc);

            fade.color = newFadeValue;
            /*fade2.color = newFadeValue;
            if (fadeBelow != null)
            {
                fadeBelow.color = newFadeValue;
            }*/ 

            for (int i = 0; i < bottomWallSprites.Count; i++)
            {
                bottomWallSprites[i].color = Color.Lerp(wallFadeStartValue,
                                                        wallFadeEndValue,
                                                        perc);
            }
        }
    }
}
