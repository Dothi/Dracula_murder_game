using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NearbyEnemiesScript : MonoBehaviour
{
    public List<GameObject> nearbyEnemies;
    

    void Awake()
    {
        nearbyEnemies = new List<GameObject>();
    }

    void Update()
    {

    }
}
