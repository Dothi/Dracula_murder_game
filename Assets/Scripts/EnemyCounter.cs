using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyCounter : MonoBehaviour {

    public int enemiesRemaining;

	void Start ()
    {
        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        enemiesRemaining = enemies.Count;
        GetComponent<Text>().text = "Enemies remaining: " + (enemiesRemaining).ToString();
	}

    public void DecreaseEnemiesRemaining()
    {
        enemiesRemaining -= 1;
        GetComponent<Text>().text = "Enemies remaining: " + (enemiesRemaining).ToString();
    }
    public int GetEnemiesRemaining()
    {
        return enemiesRemaining;
    }
}
