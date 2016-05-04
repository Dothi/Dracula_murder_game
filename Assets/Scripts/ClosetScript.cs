using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClosetScript : MonoBehaviour {

    GameController gc;

    public List<GameObject> ObjectsInside;
    public int ClosetSize = 1;
    TextMesh statusText;
    MeshRenderer textMesh;

    GameObject player;
    Collider2D playerFeet;
    bool playerInRange = false;

    public bool playerCanHide = true;
    public static bool playerIsHiding = false;

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

    AudioSource audioSource;
    public AudioClip closetClose;
    public AudioClip closetOpen;

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
        audioSource = GetComponent<AudioSource>();
	}
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Hide / Peek") && !player.GetComponent<KillScript>().isSuckingBlood)
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
        if (Input.GetButtonUp("Drag Body") && !player.GetComponent<KillScript>().isSuckingBlood)
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
        if (other != null && other == playerFeet)
        {
            playerInRange = true;
            gc.playerNearCloset = true;
            textMesh.enabled = true;
            transform.parent.GetComponentInChildren<SpriteRenderer>().sprite = highlightSprite;
            Debug.Log("Player in range: " + playerInRange);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other == playerFeet)
        {
            playerInRange = false;
            gc.playerNearCloset = false;
            textMesh.enabled = false;
            transform.parent.GetComponentInChildren<SpriteRenderer>().sprite = normalSprite;
            Debug.Log("Player in range: " + playerInRange);
        }
    }

    public void HideBody(GameObject enemy)
    {
        if (playerInRange)
        {
            if (!ObjectsInside.Contains(enemy) && ObjectsInside.Count < ClosetSize)
            {
                ObjectsInside.Add(enemy);
                enemy.SetActive(false);
                player.GetComponent<DragBody>().enemiesInRange.Remove(enemy);
                player.GetComponent<NearbyEnemiesScript>().nearbyEnemies.Remove(enemy);
                
                statusText.text = ObjectsInside.Count.ToString() + "/" + ClosetSize.ToString();
                audioSource.clip = closetOpen;
                audioSource.Play();
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
                    ObjectsInside[i].transform.Find("Collider").GetComponent<BoxCollider2D>().isTrigger = true;
                    ObjectsInside[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    ObjectsInside.Remove(ObjectsInside[i]);
                    statusText.text = ObjectsInside.Count.ToString() + "/" + ClosetSize.ToString();
                    audioSource.clip = closetClose;
                    audioSource.Play();
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
            playerIsHiding = true;
            statusText.text = ObjectsInside.Count.ToString() + "/" + ClosetSize.ToString();
            audioSource.clip = closetOpen;
            audioSource.Play();
        }
    }
    public void UnhidePlayer(GameObject player)
    {
        gc.playerInCloset = false;
        ObjectsInside.Remove(player);           
        player.SetActive(true);
        playerIsHiding = false;
        statusText.text = ObjectsInside.Count.ToString() + "/" + ClosetSize.ToString();
        audioSource.clip = closetClose;
        audioSource.Play();
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
