using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyCounter : MonoBehaviour {

    int enemiesRemaining;

	void Start ()
    {
        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        enemiesRemaining = enemies.Count;
        GetComponent<Text>().text = "Enemies remaining: " + (enemiesRemaining).ToString();
	}

    public void DecreaseEnemiesRemaining()
    {
        GetComponent<Text>().text = "Enemies remaining: " + (enemiesRemaining - 1).ToString();
    }
}
