using UnityEngine;
using System.Collections;

public class VampireHunterScript : MonoBehaviour
{
    Vector3[] path;
    int targetIndex;
    public float speed;
    GameObject player;
    public bool isAtWaypoint = true;

    void Start()
    {
        speed = 6f;
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Update()
    {
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
