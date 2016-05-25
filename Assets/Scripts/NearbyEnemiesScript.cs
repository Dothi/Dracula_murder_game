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
    public Sprite bustSprite;

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
            if (this.timer >= bustTimer)
            {
                gc.GameOver(false, bustSprite);
                Debug.Log("busteed");
            }
        }
        else
        {
            this.timer = 0f;
        }
    }
}
