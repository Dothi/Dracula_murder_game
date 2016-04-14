using UnityEngine;
using System.Collections;

public class VampireHunterScript : MonoBehaviour
{
    Vector3[] path;
    int targetIndex;
    public float speed;
    GameObject player;
    public bool isAtWaypoint = true;
    public bool isAtRange = false;
    public bool isInSight = false;
    GameController gc;

    void Start()
    {
        speed = 6f;
        player = GameObject.FindGameObjectWithTag("Player");
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.transform.position);

        if (hit && hit.collider.tag == "Player")
        {
            isInSight = true;
        }
        else
        {
            isInSight = false;
        }
        if (isAtRange && isInSight)
        {
            speed = 0f;
            Debug.Log("Bang");
           //gc.gameOver = true;   
        }
        if (isAtWaypoint)
        {
            PathRequestManager.RequestPath(transform.position, player.transform.position, OnPathFound);
            isAtWaypoint = false;
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isAtRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isAtRange = false;
            speed = 6f;
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
                        isAtWaypoint = true;
                        targetIndex = 0;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                if (currentWaypoint != null)
                {
                        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);    
                }
                yield return null;
            }
        }
    }
}
