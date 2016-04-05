using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    Vector3[] path;

    int targetIndex;
    float speed;
    public float suspiciousSpeed = 6f;
    public float patrolSpeed = 3f;
    public float velocity;
    public float currWaitTime;
    public Waypoint[] waypoints;
    public bool isCircular;
    public bool inReverse;
    public Vector3 directionOfTravel;
    public Vector3 previous;
    public int randomizer { get { return Random.Range(0, waypoints.Length); } }
    private Waypoint currentWP;
    public int currentIndex;
    GameController gc;
    public bool isWaiting;
    public bool isAtWaypoint;
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
        currWaitTime = 0f;
        currentEnemyState = EnemyState.Patrolling;
        speed = 1f;
        inReverse = false;
        currentIndex = randomizer;
        PathRequestManager.RequestPath(transform.position, waypoints[currentIndex].transform.position, OnPathFound);
        isWaiting = false;
        seePlayer = false;
        isCircular = true;
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        ks = player.GetComponent<KillScript>();
    }
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }

    }
    IEnumerator FollowPath()
    {


        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            Debug.Log(currentWaypoint);
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        Debug.Log("Stopped");
                        targetIndex = 0;
                        isAtWaypoint = true;
                        isWaiting = true;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                if (currentWaypoint != null)
                {
                    if (currentEnemyState == EnemyState.Patrolling || currentEnemyState == EnemyState.Suspicious)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                        

                        directionOfTravel = (currentWaypoint - transform.position).normalized;   
                    }
                }
                yield return null;
            }
        }
    }


    void Update()
    {
        IsBeingKilled();
        if (currentEnemyState != EnemyState.Dead)
        {
            
            SpeedController();

            if (currentEnemyState == EnemyState.Patrolling)
            {
                if (isWaiting)
                {
                    currWaitTime += 1 * Time.deltaTime;
                    if (currWaitTime >= waypoints[currentIndex].waitSeconds)
                    {
                        isWaiting = false;
                        currWaitTime = 0f;
                        waypoints[currentIndex].waitSeconds = Waypoint.randomizer;
                    }
                }
            }
            else
            {
                isWaiting = false;
            }
            if (currentEnemyState != EnemyState.Collapsed)
            {
                if (isAtWaypoint && !isWaiting)
                {

                    isAtWaypoint = false;

                    int oldIndex = currentIndex;
                    currentIndex = randomizer;
                    if (currentIndex == oldIndex)
                    {
                        isAtWaypoint = true;
                    }
                    else
                    {
                        Debug.Log("Requesting Path");
                        PathRequestManager.RequestPath(transform.position, waypoints[currentIndex].transform.position, OnPathFound);
                    }
                }
            }
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


        
    }
    void SpeedController()
    {

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
            speed = suspiciousSpeed;
        }
        else
        {
            speed = patrolSpeed;
        }
    }
}

    








