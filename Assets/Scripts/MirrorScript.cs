using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MirrorScript : MonoBehaviour {

    List<GameObject> EnemiesInRange;

    GameObject player;
    Collider2D playerFeet;

    bool playerInRange;

    public enum Facing { down, right, left };
    public Facing facing = Facing.down;

	void Start ()
    {
        EnemiesInRange = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerFeet = player.transform.Find("Collider").GetComponent<BoxCollider2D>();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))  //pitää muuttaa kun viholliset saa "jalkacolliderit"
        {
            EnemiesInRange.Add(other.gameObject);
        }
        else if (other == playerFeet)
        {
            playerInRange = true;
        }
        if (playerInRange)
        {
            foreach (GameObject enemy in EnemiesInRange)
            {
                if (enemy.GetComponent<EnemyAI>().currentEnemyState == EnemyAI.EnemyState.Patrolling ||
                    enemy.GetComponent<EnemyAI>().currentEnemyState == EnemyAI.EnemyState.Waiting)
                {
                    if (facing == Facing.down && enemy.GetComponent<FieldOfView>().currentFacingState != FieldOfView.FacingState.DOWN)
                    {
                        enemy.GetComponent<CollapseScript>().isSuspicious = true;
                    }
                    else if (facing == Facing.left && enemy.GetComponent<FieldOfView>().currentFacingState != FieldOfView.FacingState.LEFT)
                    {
                        enemy.GetComponent<CollapseScript>().isSuspicious = true;                        
                    }
                    else if (facing == Facing.right && enemy.GetComponent<FieldOfView>().currentFacingState != FieldOfView.FacingState.RIGHT)
                    {
                        enemy.GetComponent<CollapseScript>().isSuspicious = true;
                    }
                }               
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))  //pitää muuttaa kun viholliset saa "jalkacolliderit"
        {
            EnemiesInRange.Remove(other.gameObject);
        }
        else if (other == playerFeet)
        {
            playerInRange = false;
        }
    }
}
