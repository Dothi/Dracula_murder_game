using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BloodBar : MonoBehaviour {

    KillScript killScript;
    Slider bloodBar;

    float currentBlood;
    float maxBlood = 100;
    public float bloodDecreaseSpeed = 10;
    public float bloodFromKill = 65;

	void Start ()
    {
        currentBlood = maxBlood;
        killScript = GetComponent<KillScript>();
        bloodBar = GameObject.Find("BloodSlider").GetComponent<Slider>();
	}
	

	void Update ()
    {
        bloodBar.value = currentBlood;

        if (!killScript.isSuckingBlood && currentBlood > 0)
        {
            currentBlood -= bloodDecreaseSpeed * Time.deltaTime;
        }
        if (currentBlood > maxBlood)
        {
            currentBlood = maxBlood;
        }
        if (currentBlood < 0)
        {
            currentBlood = 0;
        }
	}

    public void GetBloodFromKill(float amount)
    {
        currentBlood += amount;
    }
}
