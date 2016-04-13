using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VampireHunterSpawnScript : MonoBehaviour
{
    GameController gc;
    public List<GameObject> suspiciousEnemies;
    public GameObject VampireHunter;

    void Start()
    {
        suspiciousEnemies = new List<GameObject>();
        gc = GetComponent<GameController>();
    }
    void Update()
    {
        CheckSuspiciousEnemiesCount();   
    }
    void CheckSuspiciousEnemiesCount()
    {
        for (int i = 0; i < gc.enemies.Count; i++)
        {
            EnemyAI AI = gc.enemies[i].GetComponent<EnemyAI>();
            if (AI.currentEnemyState == EnemyAI.EnemyState.Suspicious && !suspiciousEnemies.Contains(gc.enemies[i]))
            {
                suspiciousEnemies.Add(gc.enemies[i]);
            }
            else if (AI.currentEnemyState != EnemyAI.EnemyState.Suspicious)
            {
                suspiciousEnemies.Remove(gc.enemies[i]);
            }
        }
        SpawnVampireHunter();
        
    }
    void SpawnVampireHunter()
    {
        if (suspiciousEnemies.Count >= 3)
        {
            //Spawn vampire hunter
            VampireHunter.SetActive(true);

        }
    }
}
