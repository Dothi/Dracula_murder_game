using UnityEngine;
using System.Collections;

public class TriggerAreaScript : MonoBehaviour
{

    EnemyAI AI;
    CircleCollider2D triggerArea;
    NearbyEnemiesScript nearbyEnemiesScript;
    FieldOfView fov;
    CollapseScript cs;
    GameObject player;
    GameObject gamecontroller;
    GameController gc;

    
    


    void Start()
    {
        AI = GetComponentInParent<EnemyAI>();
        triggerArea = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gamecontroller = GameObject.FindGameObjectWithTag("GameController");
        nearbyEnemiesScript = player.GetComponent<NearbyEnemiesScript>();
        gc = gamecontroller.GetComponent<GameController>();
        fov = GetComponentInParent<FieldOfView>();
        cs = GetComponentInParent<CollapseScript>();
        
        
    }

    void Update()
    {
        EnableTrigger();

        if (ClosetScript.playerIsHiding)
        {
            
            nearbyEnemiesScript.nearbyEnemies.Clear();
        }

        if (fov.enemiesInFOV.Count > 0)
        {
            for (int i = 0; i < fov.enemiesInFOV.Count; i++)
            {
                if (nearbyEnemiesScript.nearbyEnemies.Contains(fov.enemiesInFOV[i]))
                {
                    EnemyAI currEnemyState = fov.enemiesInFOV[i].GetComponent<EnemyAI>();
                    switch (currEnemyState.currentEnemyState)
                    {
                        case EnemyAI.EnemyState.Collapsed:
                            cs.isSuspicious = true;
                            break;
                        case EnemyAI.EnemyState.Dead:
                            gc.gameOver = true;
                            break;
                        case EnemyAI.EnemyState.IsBeingKilled:
                            gc.gameOver = true;
                            break;
                    }
                }
            }
        }


        if (AI.seePlayer)
        {

            for (int i = 0; i < nearbyEnemiesScript.nearbyEnemies.Count; i++)
            {
                EnemyAI currentEnemyAI = nearbyEnemiesScript.nearbyEnemies[i].GetComponent<EnemyAI>();
                if (currentEnemyAI.currentEnemyState == EnemyAI.EnemyState.Dead || currentEnemyAI.currentEnemyState == EnemyAI.EnemyState.IsBeingKilled)
                {
                    gc.gameOver = true;
                }
                else if (currentEnemyAI.currentEnemyState == EnemyAI.EnemyState.Collapsed)
                {
                    cs.isSuspicious = true;
                }
            }
        }
    }
    void EnableTrigger()
    {
        switch (AI.currentEnemyState)
        {
            case EnemyAI.EnemyState.Collapsed:
                triggerArea.enabled = true;
                triggerArea.radius = 1.6f;
                break;
            case EnemyAI.EnemyState.Dead:
                triggerArea.enabled = true;
                triggerArea.radius = 1.6f;
                break;
            case EnemyAI.EnemyState.IsBeingKilled:
                triggerArea.enabled = true;
                triggerArea.radius = 1.6f;
                break;
            case EnemyAI.EnemyState.Suspicious:
                triggerArea.enabled = true;
                triggerArea.radius = 1.2f;
                break;
            case EnemyAI.EnemyState.Patrolling:
                triggerArea.enabled = false;
                break;

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (this.triggerArea.enabled == true && other.tag == "Player" && !ClosetScript.playerIsHiding)
        {

            switch (this.AI.currentEnemyState)
            {
                case EnemyAI.EnemyState.Collapsed:
                    if (!nearbyEnemiesScript.nearbyEnemies.Contains(gameObject.transform.parent.gameObject))
                    {
                        nearbyEnemiesScript.nearbyEnemies.Add(gameObject.transform.parent.gameObject);
                    }
                    break;
                case EnemyAI.EnemyState.Dead:
                    if (!nearbyEnemiesScript.nearbyEnemies.Contains(gameObject.transform.parent.gameObject))
                    {
                        nearbyEnemiesScript.nearbyEnemies.Add(gameObject.transform.parent.gameObject);
                    }
                    break;
                case EnemyAI.EnemyState.IsBeingKilled:
                    if (!nearbyEnemiesScript.nearbyEnemies.Contains(gameObject.transform.parent.gameObject))
                    {
                        nearbyEnemiesScript.nearbyEnemies.Add(gameObject.transform.parent.gameObject);
                    }
                    break;
                case EnemyAI.EnemyState.Suspicious:
                    if (!nearbyEnemiesScript.nearbyEnemies.Contains(gameObject.transform.parent.gameObject))
                    {
                        nearbyEnemiesScript.nearbyEnemies.Add(gameObject.transform.parent.gameObject);
                    }

                    break;
            }
        }
        else if (this.triggerArea.enabled == true && other.tag == "Enemy")
        {
            switch (this.AI.currentEnemyState)
            {
                case EnemyAI.EnemyState.Dead:
                    if (other.GetComponent<EnemyAI>().currentEnemyState == EnemyAI.EnemyState.Investigating)
                    other.GetComponent<EnemyAI>().isAtWaypoint = true;
                    break;
            }
        }
       
    }
    void OnTriggerExit2D(Collider2D other)
    {
        
        if (nearbyEnemiesScript.nearbyEnemies.Contains(gameObject.transform.parent.gameObject))
        {
            nearbyEnemiesScript.nearbyEnemies.Remove(gameObject.transform.parent.gameObject);
        }
    }
}

