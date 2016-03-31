using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragBody : MonoBehaviour {


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
                if (dragTarget.GetComponent<EnemyAI>().currentEnemyState == EnemyAI.EnemyState.Dead)
                {
                    if (dragTarget.transform.parent != gameObject.transform)
                    {
                        dragTarget.transform.parent = gameObject.transform;
                    }
                    //dragTarget.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
                    //^ tarvii colliderin ja rigidbodyn
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (dragTarget != null && dragTarget.activeInHierarchy &&
                dragTarget.GetComponent<EnemyAI>().currentEnemyState == EnemyAI.EnemyState.Dead)
            {
                if (dragTarget.transform.parent == gameObject.transform)
                {
                    dragTarget.transform.parent = null;
                }
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