using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyPointer : MonoBehaviour {

    bool active = false;
    EnemyCounter enemyCounter;
    Image pointer;
    GameObject lastEnemy = null;

    private Quaternion lookRotation;
    private Vector3 direction;
    /*
	void Start ()
    {
        enemyCounter = transform.parent.Find("Enemy Counter").GetComponent<EnemyCounter>();
        pointer = GetComponent<Image>();
        pointer.enabled = false;
	}
	
	void Update ()
    {
        if (!active && enemyCounter.GetEnemiesRemaining() == 1)
        {
            active = true;
            pointer.enabled = true;

            List<GameObject> enemies = new List<GameObject>();
            enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyAI>().currentEnemyState != EnemyAI.EnemyState.Dead)
                {
                    lastEnemy = enemy;
                    break;
                }
            }
        }

        if (active)
        {
            if (lastEnemy != null)
            {
                
                Vector3 vectorToTarget = lastEnemy.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                //transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 4);
                //transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);
               
                
                direction = (lastEnemy.transform.position - transform.position).normalized;
                lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1);
                //Vector3 WorldPos = Camera.main.ScreenToWorldPoint(transform.position);

                var dir = lastEnemy.transform.position - transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
            }
        }
	}*/
}
