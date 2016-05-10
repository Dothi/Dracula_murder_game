using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IndicatorOverlay : MonoBehaviour {

    List<TriggerAreaScript> enemyTASList;
    bool gettingSuspicious;

    Image image;

	void Start ()
    {
        image = GetComponent<Image>();

        enemyTASList = new List<TriggerAreaScript>();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyTASList.Add(enemy.GetComponent<TriggerAreaScript>());
        }
	}
	
	void Update ()
    {
        foreach (TriggerAreaScript TAS in enemyTASList)
        {
            if (TAS.GetTimerValue() != 0)
            {
                gettingSuspicious = true;
            }
            else
            {
                gettingSuspicious = false;
            }
        }

        if (gettingSuspicious)
        {
            if (!image.enabled)
            {
                image.enabled = true;
            }
        }
        else
        {
            if (image.enabled)
            {
                image.enabled = false;
            }
        }
	}
}
