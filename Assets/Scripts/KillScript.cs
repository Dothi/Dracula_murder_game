using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KillScript : MonoBehaviour {

    BloodBar bloodBar;

    List<GameObject> enemiesInRange = new List<GameObject>();
    public GameObject killTarget = null;
    float killHoldTime = 0;
    public float killHoldDuration = 15;
    public bool isSuckingBlood;

    void Start()
    {
        bloodBar = GetComponent<BloodBar>();
    }

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

    void LateUpdate()
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
                    bloodBar.GetBloodFromKill(bloodBar.bloodFromKill);
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
            if (!potentialTarget.GetComponent<CollapseScript>().isCollapsed)
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
        target.SetActive(false);
        enemiesInRange.Remove(target);
    }
}

