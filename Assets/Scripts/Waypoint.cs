using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour
{

    public float waitSeconds;
    public static int randomizer { get { return Random.Range(3, 10); } }

    void Awake()
    {
        waitSeconds = randomizer;
    }

    
    
}
