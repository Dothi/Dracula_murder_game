using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClosetScript : MonoBehaviour {

    GameController gc;

    List<GameObject> ObjectsInside;
    public int ClosetSize = 1;
    TextMesh statusText;
    MeshRenderer textMesh;

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

    public Sprite normalSprite;
    public Sprite highlightSprite;

	void Start ()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        ObjectsInside = new List<GameObject>();
        statusText = transform.parent.GetComponentInChildren<TextMesh>();
        textMesh = transform.parent.GetComponentInChildren<MeshRenderer>();
        textMesh.enabled = false;
        statusText.text = ObjectsInside.Count.ToString() + "/" + ClosetSize.ToString();

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
                if (ObjectsInside.Count == ClosetSize)
                {
                    UnhideBody();
                }
                else
                {
                    HidePlayer(player);
                }               
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
            textMesh.enabled = true;
            transform.parent.GetComponentInChildren<SpriteRenderer>().sprite = highlightSprite;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other == playerFeet)
        {
            playerInRange = false;
            textMesh.enabled = false;
            transform.parent.GetComponentInChildren<SpriteRenderer>().sprite = normalSprite;
        }
    }

    public void HideBody(GameObject enemy)
    {
        if (playerInRange && ObjectsInside.Count < ClosetSize)
        {
            if (!ObjectsInside.Contains(enemy))
            {
                player.GetComponent<DragBody>().enemiesInRange.Remove(enemy);
                player.GetComponent<NearbyEnemiesScript>().nearbyEnemies.Remove(enemy);
                ObjectsInside.Add(enemy);
                enemy.SetActive(false);
                statusText.text = ObjectsInside.Count.ToString() + "/" + ClosetSize.ToString();
            }
        }
    }
    public void UnhideBody()
    {
        if (playerInRange)
        {
            for (int i = 0; i < ObjectsInside.Count; i++)
            {
                if (ObjectsInside[i].CompareTag("Enemy"))
                {
                    ObjectsInside[i].SetActive(true);
                    ObjectsInside.Remove(ObjectsInside[i]);
                    //ObjectsInside[i].transform.Find("Collider").GetComponent<BoxCollider2D>().isTrigger = true;
                    //ObjectsInside[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    statusText.text = ObjectsInside.Count.ToString() + "/" + ClosetSize.ToString();
                    break;
                }
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
            statusText.text = ObjectsInside.Count.ToString() + "/" + ClosetSize.ToString();
        }
    }
    public void UnhidePlayer(GameObject player)
    {
        gc.playerInCloset = false;
        ObjectsInside.Remove(player);           
        player.SetActive(true);
        statusText.text = ObjectsInside.Count.ToString() + "/" + ClosetSize.ToString();
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
