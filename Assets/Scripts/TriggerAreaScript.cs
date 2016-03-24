using UnityEngine;
using System.Collections;

public class TriggerAreaScript : MonoBehaviour
{

    EnemyAI AI;
    CircleCollider2D triggerArea;

    GameObject player;
    GameObject gamecontroller;
    PlayerStatusScript playerstatus;
    GameController gc;

    void Start()
    {
        AI = GetComponent<EnemyAI>();
        triggerArea = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerstatus = player.GetComponent<PlayerStatusScript>();
        gamecontroller = GameObject.FindGameObjectWithTag("GameController");
        gc = gamecontroller.GetComponent<GameController>();

    }

    void Update()
    {
        if (AI.currentEnemyState == EnemyAI.EnemyState.Collapsed)
        {
            triggerArea.enabled = true;
        }


    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (playerstatus.currentPlayerStatus == PlayerStatusScript.PlayerStatus.Harmless)
        {
            if (other.gameObject.tag == "Player")
            {
                if (AI.currentEnemyState == EnemyAI.EnemyState.Collapsed)
                {
                    
                    playerstatus.currentPlayerStatus = PlayerStatusScript.PlayerStatus.Suspicious;

                }
            }

        }
        else if (playerstatus.currentPlayerStatus == PlayerStatusScript.PlayerStatus.Suspicious)
        {
            if (other.gameObject.tag == "Player")
            {
                if (AI.currentEnemyState == EnemyAI.EnemyState.Suspicious)
                {
                    gc.gameOver = true;
                }
            }
        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (AI.currentEnemyState == EnemyAI.EnemyState.Collapsed)
            {
                playerstatus.currentPlayerStatus = PlayerStatusScript.PlayerStatus.Harmless;
            }
        }
    }
}

