using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour
{

    public float waitSeconds;
    public float speedOut;
    public static int randomizer { get { return Random.Range(8, 15); } }

    void Awake()
    {
        waitSeconds = randomizer;
    }
}
