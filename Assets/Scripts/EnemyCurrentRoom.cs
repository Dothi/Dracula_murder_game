using UnityEngine;
using System.Collections;

public class EnemyCurrentRoom : MonoBehaviour {

    EnemyAI AI;

	void Start ()
    {
        AI = GetComponentInParent<EnemyAI>();
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (AI.currentRoom != other)
        {
            if (other != null && other.gameObject.tag == "CameraArea")
            {
                AI.currentRoom = other.gameObject;
            }
        }
    }
}
