using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    Vector3[] path;

    int targetIndex;
    float speed;
    public float suspiciousSpeed = 6f;
    public float patrolSpeed = 3f;
    public float currWaitTime;
    public float idleTimer;
    public Waypoint[] waypoints;
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
    public GameObject bigroom;
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
        idleTimer = 0f;
        currentEnemyState = EnemyState.Patrolling;
        speed = 1f;
        currentIndex = randomizer;
      //  PathRequestManager.RequestPath(transform.position, RandomPointInRoom(), OnPathFound);
        PathRequestManager.RequestPath(transform.position, waypoints[currentIndex].transform.position, OnPathFound);
        isWaiting = false;
        seePlayer = false;
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
        Patrolling();

      /*  if (directionOfTravel == Vector3.zero && !isWaiting)
        {
            idleTimer += 1 * Time.deltaTime;
            if(idleTimer >= 20f)
            {
                isAtWaypoint = true;
                Debug.Log("vittutututu");
            }
        }*/

    }

    void Patrolling()
    {
        if (currentEnemyState != EnemyState.Dead)
        {
            IsBeingKilled();
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
                        
                        //PathRequestManager.RequestPath(transform.position, RandomPointInRoom(), OnPathFound);
                        PathRequestManager.RequestPath(transform.position, waypoints[currentIndex].transform.position, OnPathFound);
                       // idleTimer = 0f;
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

    Vector3 RandomPointInRoom()
    {
        

        float x = Random.Range(bigroom.GetComponentInParent<Transform>().position.x - Mathf.Round(bigroom.transform.localScale.x), bigroom.GetComponentInParent<Transform>().position.x + Mathf.Round(bigroom.transform.localScale.x));
        float y = Random.Range(bigroom.GetComponentInParent<Transform>().position.y - Mathf.Round(bigroom.transform.localScale.y), bigroom.GetComponentInParent<Transform>().position.y + Mathf.Round(bigroom.transform.localScale.y));

        return new Vector3(x, y);


    }
}

    








