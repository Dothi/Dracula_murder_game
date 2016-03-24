using UnityEngine;
using System.Collections;

public class PlayerStatusScript : MonoBehaviour {

    CollapseScript cs;

    public enum PlayerStatus
    {
        Harmless,
        Suspicious,
        Caught
    };

    public PlayerStatus currentPlayerStatus = PlayerStatus.Harmless;

    void Awake()
    {
        cs = GameObject.FindGameObjectWithTag("Enemy").GetComponent<CollapseScript>();
        
    }

    void Update()
    {

    }
    

}
