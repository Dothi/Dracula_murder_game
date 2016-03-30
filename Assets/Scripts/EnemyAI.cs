using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    float speed;
    public Waypoint[] waypoints;
    public bool isCircular;
    public bool inReverse;
    public Vector3 directionOfTravel;
    public int randomizer { get { return Random.Range(0, waypoints.Length); } }

    private Waypoint currentWaypoint;
    public int currentIndex;
    private float speedStorage;
    GameController gc;
    public bool isWaiting;
    public bool seePlayer;
    GameObject player;

    KillScript ks;
    public enum EnemyState
    {
        Patrolling,
        Waiting,
        Suspicious,
        IsBeingKilled,
        Collapsed,
        Dead
    };

    public EnemyState currentEnemyState;

    void Start()
    {
        currentEnemyState = EnemyState.Patrolling;
        speed = 1f;
        inReverse = false;
        currentIndex = randomizer;
        speedStorage = 0f;
        isWaiting = false;
        seePlayer = false;
        isCircular = true;
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();


        
        player = GameObject.FindGameObjectWithTag("Player");
        ks = player.GetComponent<KillScript>();
        if (waypoints.Length > 0)
        {
            currentWaypoint = waypoints[0];
        }
    }

    void Update()
    {
        IsBeingKilled();
        PlayerBusting();

        if (currentWaypoint != null && !isWaiting)
        {
            MoveTowardsWaypoint();
        }
    }

    void Pause()
    {
        if (currentEnemyState == EnemyState.Patrolling)
        {
            isWaiting = !isWaiting;
        }
    }
    void PlayerBusting()
    {
        switch (currentEnemyState)
        {
            case EnemyState.Patrolling:
                break;
            case EnemyState.Suspicious:
                if (seePlayer)
                {
                    
                }
                break;
        }
    }
    void IsBeingKilled()
    {
        
        if (ks.isSuckingBlood && this.gameObject == ks.killTarget)
        {
            if (currentEnemyState != EnemyState.Dead)
            {
                currentEnemyState = EnemyState.IsBeingKilled;
            }          
        }
        

        if (currentEnemyState == EnemyState.IsBeingKilled) 
        {
            speed = 0f;
        }
        else if (currentEnemyState == EnemyState.Collapsed)
        {
            speed = 0f;  
        }
        else if (currentEnemyState == EnemyState.Dead)
        {
            speed = 0f;
        }
        else if (currentEnemyState == EnemyState.Suspicious)
        { 
            speed = 4f;
        }
        else
        {   
            speed = 1f;
        }
    }

    private void MoveTowardsWaypoint()
    {
        Vector3 currentPosition = this.transform.position;
        Vector3 targetPosition = currentWaypoint.transform.position;

        if (Vector3.Distance(currentPosition, targetPosition) > 2f)
        {
            directionOfTravel = targetPosition - currentPosition;
            directionOfTravel.Normalize();

            this.transform.Translate(directionOfTravel.x * speed * Time.deltaTime,
                                     directionOfTravel.y * speed * Time.deltaTime,
                                     directionOfTravel.z * speed * Time.deltaTime,
                                     Space.World);
        }

        else
        {
            if (currentWaypoint.waitSeconds > 0)
            {
                Pause();
                Invoke("Pause", currentWaypoint.waitSeconds);
            }

            if (currentWaypoint.speedOut > 0)
            {
                speedStorage = speed;
                speed = currentWaypoint.speedOut;
            }
            else if (speedStorage != 0)
            {
                speed = speedStorage;
                speedStorage = 0f;
            }

            NextWaypoint();

        }
    }

    private void NextWaypoint()
    {
        if (isCircular)
        {
            if (!inReverse)
            { 
                    foreach (var wp in waypoints)
                    {
                        wp.waitSeconds = wp.randomizer;
                    }
                    currentIndex = randomizer;
            }
           /* else
            {
                if ((!inReverse && currentIndex + 1 >= waypoints.Length) || (inReverse && currentIndex == 0))
                {
                    inReverse = !inReverse;
                }
                currentIndex = randomizer;
            }*/
            currentWaypoint = waypoints[currentIndex];
        }
    }
}








