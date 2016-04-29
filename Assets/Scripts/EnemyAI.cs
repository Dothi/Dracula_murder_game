using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    #region Variables
    Vector3[] path;
    public Sprite deadSprite;
    int targetIndex;
    public int roomStops;
    public float speed;
    public float suspiciousSpeed = 6f;
    public float patrolSpeed = 3f;
    float currWaitTime;
    public float idleTimer;
    public Waypoint[] waypoints;
    public Vector3 directionOfTravel;
    Vector3 previous;
    public int randomizer { get { return Random.Range(0, waypoints.Length); } }
    public int roomVisitRandomizer { get { return Random.Range(8, 13); } }
    private Waypoint currentWP;
    public int currentIndex;
    GameController gc;
    public bool isWaiting;
    public bool isAtWaypoint;
    public bool seePlayer;
    bool hasGasped;
    GameObject player;
    public GameObject targetRoom;
    public GameObject currentRoom;
    public GameObject target;
    KillScript ks;
    AudioSource audioSource;
    public AudioClip gasp;
    SpriteRenderer spriteRend;
    Collider2D collider;
    FieldOfView fov;
    CollapseScript cs;
    TriggerAreaScript tas;
    #endregion
    public enum EnemyState
    {
        Patrolling,
        Waiting,
        Investigating,
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
        hasGasped = false;
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
        audioSource = GetComponent<AudioSource>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        fov = GetComponent<FieldOfView>();
        cs = GetComponent<CollapseScript>();
        
        
        
    }
    void Update()
    {
        if (currentEnemyState == EnemyState.Patrolling)
        {
            IsBeingKilled();
            SpeedController();
            Patrolling();
            IdleController();
            if (fov.seeDeadEnemy)
            {
                currentEnemyState = EnemyAI.EnemyState.Investigating;
                
            }
        }
        if (currentEnemyState == EnemyState.Suspicious)
        {
            IsBeingKilled();
            SpeedController();
            targetRoom = waypoints[currentIndex].gameObject;
            Patrolling();
            IdleController();
            if (!hasGasped)
            {
                PlayGasp();
                hasGasped = true;
            }
        }
        if (currentEnemyState == EnemyState.Investigating)
        {
            IsBeingKilled();
            SpeedController();
            IdleController();

            

            if (target == null)
            {
                for (int i = 0; i < fov.enemiesInFOV.Count; i++)
                {
                    if (fov.enemiesInFOV[i].GetComponent<EnemyAI>().currentEnemyState == EnemyState.Dead)
                    {
                        target = fov.enemiesInFOV[i];

                    }
                }
            }
            else
            {
                if (!isAtWaypoint)
                PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
                
            }
            if (isAtWaypoint)
            {
                currWaitTime += 1 * Time.deltaTime;
                if (currWaitTime >= 2f)
                {
                    cs.isSuspicious = true;
                    currWaitTime = 0f;
                    
                }
            }
        }
        if (currentEnemyState == EnemyState.Dead)
        {
            collider.offset = new Vector2(0, .5f);
        }
        
        if (currentEnemyState != EnemyState.Suspicious)
        {
            hasGasped = false;
        }
        
        
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
                        
                        isAtWaypoint = true;
                        
                        roomStops++;
                        isWaiting = true;
                        targetIndex = 0;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                if (currentWaypoint != null)
                {
                    if (currentEnemyState == EnemyState.Patrolling || currentEnemyState == EnemyState.Suspicious || currentEnemyState == EnemyState.Investigating)
                    {
                        if (!isAtWaypoint)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                            directionOfTravel = (currentWaypoint - transform.position).normalized;
                        }
                        
                            
                        
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
                if (idleTimer >= 2f)
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
        else if (currentEnemyState == EnemyState.Investigating)
        {
            speed = 1f;
        }
        else
        {
            speed = patrolSpeed;
        }
    }
    Vector3 RandomPointInRoom()
    {
        float x = Random.Range(targetRoom.GetComponent<Transform>().position.x - (int)((targetRoom.transform.localScale.x / 2) - 1), targetRoom.GetComponent<Transform>().position.x + (int)((targetRoom.transform.localScale.x / 2) -1));
        float y = Random.Range(targetRoom.GetComponent<Transform>().position.y - (int)((targetRoom.transform.localScale.y / 2) - 1), targetRoom.GetComponent<Transform>().position.y + (int)((targetRoom.transform.localScale.y / 2) -1));

        return new Vector3(x, y);
    }
    void PlayGasp()
    {
        audioSource.clip = gasp;
        audioSource.Play();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.gameObject.tag == "CameraArea")
        {
            currentRoom = other.gameObject;
        }
    }
}

    








