using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NearbyEnemiesScript : MonoBehaviour
{
    public List<GameObject> nearbyEnemies;
    public List<GameObject> suspiciousEnemies;
    public float timer;
    float bustTimer;
    GameController gc;
    InvisibilitySkill invisSkill;

    void Awake()
    {
        nearbyEnemies = new List<GameObject>();
        timer = 0f;
        bustTimer = 1f;
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        invisSkill = GetComponent<InvisibilitySkill>();
    }

    void Update()
    {
        GetNearbyEnemyState();
    }

    void GetNearbyEnemyState()
    {
        for (int i = 0; i < nearbyEnemies.Count; i++)
        {
            EnemyAI ai = nearbyEnemies[i].GetComponent<EnemyAI>();

            if (ai.currentEnemyState == EnemyAI.EnemyState.Suspicious && !suspiciousEnemies.Contains(nearbyEnemies[i]))
            {
                suspiciousEnemies.Add(nearbyEnemies[i]);
            }
        }
        for (int i = 0; i < suspiciousEnemies.Count; i++)
        {
            EnemyAI ai = suspiciousEnemies[i].GetComponent<EnemyAI>();
            if (!nearbyEnemies.Contains(suspiciousEnemies[i]))
            {
                suspiciousEnemies.Remove(suspiciousEnemies[i]);
            }
            else if (ai.currentEnemyState != EnemyAI.EnemyState.Suspicious)
            {
                suspiciousEnemies.Remove(suspiciousEnemies[i]);
            }
        }
        if (suspiciousEnemies.Count > 0 && !invisSkill.isInvisible)
        {
            timer += Time.deltaTime;
            if (timer >= bustTimer)
            {
                gc.gameOver = true;
                Debug.Log("busteed");
            }
        }
        else
        {
            timer = 0f;
        }
    }
}
