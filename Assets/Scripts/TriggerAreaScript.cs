using UnityEngine;
using System.Collections;

public class TriggerAreaScript : MonoBehaviour
{

    EnemyAI AI;
    CircleCollider2D triggerArea;

    GameObject player;
    GameObject gamecontroller;
    GameController gc;

    void Start()
    {
        AI = GetComponent<EnemyAI>();
        triggerArea = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gamecontroller = GameObject.FindGameObjectWithTag("GameController");
        gc = gamecontroller.GetComponent<GameController>();

    }

    void Update()
    {
        EnableTrigger();
    }
    void EnableTrigger()
    {
        switch (AI.currentEnemyState)
        {
            case EnemyAI.EnemyState.Collapsed:
                triggerArea.enabled = true;
                break;
            case EnemyAI.EnemyState.Dead:
                triggerArea.enabled = true;
                break;
            case EnemyAI.EnemyState.Suspicious:
                triggerArea.enabled = true;
                break;
            case EnemyAI.EnemyState.Patrolling:
                triggerArea.enabled = false;
                break;

        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
       switch (AI.currentEnemyState)
       {
           case EnemyAI.EnemyState.Collapsed:
               break;
           case EnemyAI.EnemyState.Dead:
               break;
           case EnemyAI.EnemyState.Suspicious:
               gc.gameOver = true;
               break;
       }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        
    }
}

