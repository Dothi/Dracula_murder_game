using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragBody : MonoBehaviour {

    public LayerMask unwalkableMask;
    List<GameObject> enemiesInRange = new List<GameObject>();
    public GameObject dragTarget = null;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dragTarget = GetClosestEnemy(enemiesInRange);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            if (dragTarget != null && dragTarget.activeInHierarchy)
            {
                BoxCollider2D enemyCol = dragTarget.transform.Find("Collider").GetComponent<BoxCollider2D>();

                if (dragTarget.GetComponent<EnemyAI>().currentEnemyState == EnemyAI.EnemyState.Dead &&
                    !Physics2D.Linecast(transform.position, dragTarget.transform.position, unwalkableMask) &&
                    Vector3.Distance(dragTarget.transform.position, transform.position) < 3)
                {                 
                    if (enemyCol.isTrigger)
                    {
                        enemyCol.isTrigger = false;
                    }
                    dragTarget.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
                }
                else
                {
                    if (!enemyCol.isTrigger)
                    {
                        enemyCol.isTrigger = true;
                    }
                    dragTarget.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (dragTarget != null && dragTarget.activeInHierarchy &&
                dragTarget.GetComponent<EnemyAI>().currentEnemyState == EnemyAI.EnemyState.Dead)
            {
                BoxCollider2D enemyCol = dragTarget.transform.Find("Collider").GetComponent<BoxCollider2D>();
                if (!enemyCol.isTrigger)
                {
                    enemyCol.isTrigger = true;
                }
                dragTarget.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
            dragTarget = null;
        }
    }

    GameObject GetClosestEnemy(List<GameObject> enemies)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies)
        {
            if (potentialTarget.GetComponent<EnemyAI>().currentEnemyState == EnemyAI.EnemyState.Dead)
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
}