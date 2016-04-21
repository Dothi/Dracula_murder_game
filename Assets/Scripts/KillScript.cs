using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KillScript : MonoBehaviour {

    BloodBar bloodBar;
    GameController gameController;

    public List<GameObject> enemiesInRange = new List<GameObject>();
    public GameObject killTarget = null;
    float killHoldTime = 0;
    public float killHoldDuration = 15;
    public bool isSuckingBlood;
    public AudioClip bodyFall;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        bloodBar = gameController.GetComponent<BloodBar>();
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            killHoldTime = 0;
            killTarget = GetClosestEnemy(enemiesInRange);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            if (killTarget != null && killTarget.activeInHierarchy)
            {
                if (killHoldTime < killHoldDuration)
                {
                    isSuckingBlood = true;
                    killHoldTime += 10 * Time.deltaTime;
                }
                else if (killHoldTime >= killHoldDuration)
                {
                    if (isSuckingBlood)
                    {
                        bloodBar.GetBloodFromKill(bloodBar.bloodFromKill);
                        isSuckingBlood = false;
                    }
                    KillEnemy(killTarget);
                }

            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            if (killHoldTime < killHoldDuration && killTarget != null && killTarget.activeInHierarchy)
            {
                killTarget.GetComponent<CollapseScript>().isCollapsed = true;
            }

            isSuckingBlood = false;
            killTarget = null;
            killHoldTime = 0;
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

    void KillEnemy(GameObject target)
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

