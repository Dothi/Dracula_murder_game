using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClosetScript : MonoBehaviour {

    List<GameObject> ObjectsInside;
    public int ClosetSize = 1;

    GameObject player;
    Collider2D playerFeet;
    bool playerInRange;

    public bool playerCanHide = true;

	void Start ()
    {
        ObjectsInside = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerFeet = player.transform.Find("Collider").GetComponent<BoxCollider2D>();
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
            ObjectsInside.Add(player);
            player.SetActive(false);
        }
    }
    public void UnhidePlayer(GameObject player)
    {
        ObjectsInside.Remove(player);           
        player.SetActive(true);
    }
}
