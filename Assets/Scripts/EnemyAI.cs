﻿using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    Vector3[] path;

    int targetIndex;
    public int roomStops;
    float speed;
    public float suspiciousSpeed = 6f;
    public float patrolSpeed = 3f;
    float currWaitTime;
    float idleTimer;
    public Waypoint[] waypoints;
    public Vector3 directionOfTravel;
    Vector3 previous;
    int randomizer { get { return Random.Range(0, waypoints.Length); } }
    int roomVisitRandomizer { get { return Random.Range(8, 13); } }
    private Waypoint currentWP;
    public int currentIndex;
    GameController gc;
    public bool isWaiting;
    bool isAtWaypoint;
    public bool seePlayer;
    GameObject player;
    public GameObject targetRoom;
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
        roomStops = 0;
        currentEnemyState = EnemyState.Patrolling;
        speed = 1f;
        currentIndex = randomizer;
        targetRoom = waypoints[currentIndex].gameObject;
        PathRequestManager.RequestPath(transform.position, RandomPointInRoom(), OnPathFound);
        isWaiting = false;
        seePlayer = false;
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        ks = player.GetComponent<KillScript>();
    }
    void Update()
    {
        Patrolling();
        IdleController();
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
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        targetIndex = 0;
                        isAtWaypoint = true;
                        roomStops++;
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
    void IdleController()
    {
        if (currentEnemyState != EnemyState.Dead && currentEnemyState != EnemyState.Collapsed)
        {
            if (directionOfTravel == Vector3.zero && !isWaiting)
            {
                idleTimer += 1 * Time.deltaTime;
                if (idleTimer >= 5f)
                {
                    isAtWaypoint = true;
                }
            }
        }
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
                    if (roomStops >= roomVisitRandomizer)
                    {
                        targetRoom = waypoints[currentIndex].gameObject;
                        roomStops = 0;
                    }
                    if (currentIndex == oldIndex)
                    {
                        isAtWaypoint = true;
                    }
                    else
                    {
                        PathRequestManager.RequestPath(transform.position, RandomPointInRoom(), OnPathFound);
                        idleTimer = 0f;
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
        float x = Random.Range(targetRoom.GetComponentInParent<Transform>().position.x - targetRoom.transform.localScale.x, targetRoom.GetComponentInParent<Transform>().position.x + targetRoom.transform.localScale.x);
        float y = Random.Range(targetRoom.GetComponentInParent<Transform>().position.y - targetRoom.transform.localScale.y, targetRoom.GetComponentInParent<Transform>().position.y + targetRoom.transform.localScale.y);

        return new Vector3(x, y);
    }
}

    








