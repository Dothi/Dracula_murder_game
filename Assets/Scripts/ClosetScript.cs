using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClosetScript : MonoBehaviour {

    GameController gc;

    List<GameObject> ObjectsInside;
    public int ClosetSize = 1;

    GameObject player;
    Collider2D playerFeet;
    bool playerInRange = false;

    public bool playerCanHide = true;

    GameObject peekOverlay;
    List<SpriteRenderer> overlayPieces;
    float lerpTime = 0.6f;
    float currentLerpTime;
    bool resetLerp;
    Color fadeStartValue;
    Color fadeEndValue;
    Color lastFadeEndValue;

	void Start ()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        ObjectsInside = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerFeet = player.transform.Find("Collider").GetComponent<BoxCollider2D>();

        overlayPieces = new List<SpriteRenderer>();
        peekOverlay = GameObject.Find("PeekOverlay");
        overlayPieces.AddRange(peekOverlay.transform.GetComponentsInChildren<SpriteRenderer>());
	}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!ObjectsInside.Contains(player))
            {
                HidePlayer(player);
            }
            else if (ObjectsInside.Contains(player))
            {
                UnhidePlayer(player);
            } 
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GameObject dragTarget = player.GetComponent<DragBody>().dragTarget;
            if (dragTarget != null && dragTarget.activeInHierarchy && dragTarget.GetComponent<EnemyAI>().currentEnemyState == EnemyAI.EnemyState.Dead)
            {
                HideBody(dragTarget);
            }
        }

        FadePeekOverlay();

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == playerFeet)
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other == playerFeet)
        {
            playerInRange = false;
        }
    }

    public void HideBody(GameObject enemy)
    {
        if (playerInRange && ObjectsInside.Count < ClosetSize)
        {
            if (!ObjectsInside.Contains(enemy))
            {
                ObjectsInside.Add(enemy);
                enemy.SetActive(false);
            }
        }
    }
    public void HidePlayer(GameObject player)
    {
        if (playerInRange && playerCanHide && ObjectsInside.Count < ClosetSize)
        {
            peekOverlay.transform.position = transform.parent.position;
            gc.playerInCloset = true;
            ObjectsInside.Add(player);
            player.SetActive(false);
        }
    }
    public void UnhidePlayer(GameObject player)
    {
        gc.playerInCloset = false;
        ObjectsInside.Remove(player);           
        player.SetActive(true);
    }
    private void FadePeekOverlay()
    {
        if (!gc.playerInCloset)
        {
            if (resetLerp)
            {
                currentLerpTime = 0;
                resetLerp = false;
                if (overlayPieces.Count > 0)
                {
                    fadeStartValue = overlayPieces[0].color;                    
                }
            }
            if (overlayPieces.Count > 0)
            {
                fadeEndValue = new Color(overlayPieces[0].color.r, overlayPieces[0].color.g, overlayPieces[0].color.b, 0.0f);
            }           

            currentLerpTime += Time.deltaTime;

            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float perc = currentLerpTime / lerpTime;

            Color newFadeValue = Color.Lerp(fadeStartValue, fadeEndValue, perc);

            foreach (SpriteRenderer sr in overlayPieces)
            {
                sr.color = newFadeValue;
            }

        }
        else
        {
            if (!resetLerp)
            {
                currentLerpTime = 0;
                resetLerp = true;
                if (overlayPieces.Count > 0)
                {
                    fadeStartValue = overlayPieces[0].color;
                }
            }

            fadeEndValue = new Color(overlayPieces[0].color.r, overlayPieces[0].color.g, overlayPieces[0].color.b, 1.0f);

            currentLerpTime += Time.deltaTime;

            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float perc = currentLerpTime / lerpTime;

            Color newFadeValue = Color.Lerp(fadeStartValue, fadeEndValue, perc);

            foreach (SpriteRenderer sr in overlayPieces)
            {
                sr.color = newFadeValue;
            }
        }
    }
}
