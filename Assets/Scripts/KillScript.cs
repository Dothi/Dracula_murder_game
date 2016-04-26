using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KillScript : MonoBehaviour {

    public BloodBar bloodBar;
    GameController gameController;

    public List<GameObject> enemiesInRange = new List<GameObject>();
    public GameObject killTarget = null;

    public bool isSuckingBlood;
    public AudioClip bodyFall;

    public GameObject minigame;
    PlayerMovement pMove;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        bloodBar = gameController.GetComponent<BloodBar>();
        pMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!enemiesInRange.Contains(other.gameObject))
            {
                if (other.gameObject.GetComponent<EnemyAI>().currentEnemyState != EnemyAI.EnemyState.Collapsed &&
                    other.gameObject.GetComponent<EnemyAI>().currentEnemyState != EnemyAI.EnemyState.Dead)
                {
                    enemiesInRange.Add(other.gameObject);   
                }
            }         
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != null && other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isSuckingBlood)
        {
            killTarget = GetClosestEnemy(enemiesInRange);

            if (killTarget != null && killTarget.activeInHierarchy)
            {
                isSuckingBlood = true;
                pMove.canMove = false;
                minigame.SetActive(true);
            }
        }
    }

    GameObject GetClosestEnemy(List<GameObject> enemies)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies)
        {
            if (potentialTarget.GetComponent<EnemyAI>().currentEnemyState != EnemyAI.EnemyState.Collapsed &&
                potentialTarget.GetComponent<EnemyAI>().currentEnemyState != EnemyAI.EnemyState.Dead)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
        }

        return bestTarget;
    }
    public void CancelKill()
    {
        if (killTarget != null && killTarget.activeInHierarchy)
        {
            killTarget.GetComponent<CollapseScript>().isCollapsed = true;
        }
        isSuckingBlood = false;
        killTarget = null;
        pMove.canMove = true;
    }
    public void SuccesfulKill()
    {
        if (killTarget != null && killTarget.activeInHierarchy)
        {
            bloodBar.GetBloodFromKill(bloodBar.bloodFromKill);
            KillEnemy(killTarget);
        }
        isSuckingBlood = false;
        pMove.canMove = true;
    }
    public void KillEnemy(GameObject target)
    {
        target.GetComponent<AudioSource>().clip = bodyFall;
        target.GetComponent<EnemyAI>().currentEnemyState = EnemyAI.EnemyState.Dead;
        target.GetComponent<AudioSource>().Play();
        target.GetComponentInChildren<SpriteRenderer>().color = Color.black;
        enemiesInRange.Remove(target);
        CheckWinForZeroEnemies(target);
        killTarget = null;
    }
    void CheckWinForZeroEnemies(GameObject target)
    {
        gameController.enemies.Remove(target);
        if (gameController.enemies.Count < 1)
        {
            gameController.gameOver = true;
            gameController.gameWon = true;
        }
    }
}

