using UnityEngine;
using System.Collections;

public class PlayerCurrentRoomScript : MonoBehaviour
{
    public GameObject currentRoom;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.gameObject.tag == "CameraArea")
        {
            currentRoom = other.gameObject;
        }
    }
}
