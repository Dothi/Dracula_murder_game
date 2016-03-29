using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FieldOfView : MonoBehaviour
{
    public float sightDist;
    float timer;
    EnemyAI AI;
    PlayerStatusScript playerStatus;
    public List<GameObject> enemiesInFOV;
    
    public RaycastHit2D[] hits;

    public enum FacingState
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    };

    public FacingState currentFacingState;
    public void Awake()
    {
        AI = GetComponent<EnemyAI>();
        sightDist = 6f;
        currentFacingState = FacingState.UP;
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatusScript>();
        enemiesInFOV = new List<GameObject>();
        
    }

    public void Update()
    {
        Facing();
        if (AI.currentEnemyState != EnemyAI.EnemyState.Dead && AI.currentEnemyState != EnemyAI.EnemyState.Collapsed)
        {
            Investigate();
        }
        else
        {
            AI.seePlayer = false;
        }      

        if (AI.seePlayer && playerStatus.currentPlayerStatus == PlayerStatusScript.PlayerStatus.Suspicious)
        {
            Debug.Log("ded");
        }

    }

    void Facing()
    {
        if (AI.directionOfTravel.y < -.5f && AI.directionOfTravel.x > AI.directionOfTravel.y)
        {
            currentFacingState = FacingState.DOWN;
            
        }
        else if (AI.directionOfTravel.x < 0 && AI.directionOfTravel.x < AI.directionOfTravel.y && AI.directionOfTravel.x < AI.directionOfTravel.y * -1)
        {
            currentFacingState = FacingState.LEFT;

        }
        else if (AI.directionOfTravel.y > .5f && AI.directionOfTravel.x < AI.directionOfTravel.y)
        {
            currentFacingState = FacingState.UP;
            
        }
       
        else if (AI.directionOfTravel.x > 0 && AI.directionOfTravel.x > AI.directionOfTravel.y)
        {
            currentFacingState = FacingState.RIGHT;
            
        }
        
        
    }

    void Investigate()
    {
        timer += Time.deltaTime;

        
        

        switch (currentFacingState)
        {
            case FacingState.UP:
                RaycastHit2D hit1 = Physics2D.Raycast(transform.position, transform.up, (sightDist + .2f));
                RaycastHit2D hit2 = Physics2D.Raycast(transform.position, (transform.up + transform.right + new Vector3(-.3f, 0, 0)), (sightDist + .6f));
                RaycastHit2D hit3 = Physics2D.Raycast(transform.position, (transform.up - transform.right + new Vector3(.3f, 0, 0)), (sightDist + .6f));
                RaycastHit2D hit4 = Physics2D.Raycast(transform.position, (transform.up - transform.right + new Vector3(.65f, 0, 0)), (sightDist + .6f));
                RaycastHit2D hit5 = Physics2D.Raycast(transform.position, (transform.up + transform.right + new Vector3(-.65f, 0, 0)), (sightDist + .6f));

                Debug.DrawRay(transform.position, (transform.up + transform.right + new Vector3(-.3f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, (transform.up - transform.right + new Vector3(.3f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, (transform.up - transform.right + new Vector3(.65f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, (transform.up + transform.right + new Vector3(-.65f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, transform.up * (sightDist + .2f), Color.green);

                //See player                                                                 
                if (hit1 && hit1.collider.tag == "Player")
                {
                    AI.seePlayer = true;                    
                }                
                else if (hit2 && hit2.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                else if (hit3 && hit3.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                else if (hit4 && hit4.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                   
                }
                else if (hit5 && hit5.collider.tag == "Player")
                {
                    AI.seePlayer = true;                    
                }
                else
                {
                    AI.seePlayer = false;                    
                }

                //See enemies
                if (hit1 && hit1.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit1.collider.gameObject))
                {
                        enemiesInFOV.Add(hit1.collider.gameObject);
                }
                else if (hit2 && hit2.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit2.collider.gameObject))
                {
                    enemiesInFOV.Add(hit2.collider.gameObject);
                }
                else if (hit3 && hit3.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit3.collider.gameObject))
                {
                    enemiesInFOV.Add(hit3.collider.gameObject);
                }
                else if (hit4 && hit4.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit4.collider.gameObject))
                {
                    enemiesInFOV.Add(hit4.collider.gameObject);
                }
                else if (hit5 && hit5.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit5.collider.gameObject))
                {
                        enemiesInFOV.Add(hit5.collider.gameObject);
                }

                //"unsee" enemies
                for (int i = 0; i < enemiesInFOV.Count; i++)
                {
                    if (hit1 && hit1.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit2 && hit2.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit3 && hit3.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit4 && hit4.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit5 && hit5.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else
                    {
                        enemiesInFOV.Remove(enemiesInFOV[i]);
                    }
                }

                break;
            case FacingState.DOWN:
                hit1 = Physics2D.Raycast(transform.position, -transform.up, (sightDist + .2f));
                hit2 = Physics2D.Raycast(transform.position, (-transform.up + transform.right + new Vector3(-.3f, 0, 0)), (sightDist + .6f));
                hit3 = Physics2D.Raycast(transform.position, (-transform.up - transform.right + new Vector3(.3f, 0, 0)), (sightDist + .6f));
                hit4 = Physics2D.Raycast(transform.position, (-transform.up - transform.right + new Vector3(.65f, 0, 0)), (sightDist + .6f));
                hit5 = Physics2D.Raycast(transform.position, (-transform.up + transform.right + new Vector3(-.65f, 0, 0)), (sightDist + .6f));

                Debug.DrawRay(transform.position, (-transform.up + transform.right + new Vector3(-.3f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, (-transform.up - transform.right + new Vector3(.3f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, (-transform.up - transform.right + new Vector3(.65f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, (-transform.up + transform.right + new Vector3(-.65f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, -transform.up * (sightDist + .2f), Color.green);


                if (hit1 && hit1.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                else if (hit2 && hit2.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                else if (hit3 && hit3.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                else if (hit4 && hit4.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                   
                }
                else if (hit5 && hit5.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                 
                }
                else
                {
                    AI.seePlayer = false;
                }

                //See enemies
                if (hit1 && hit1.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit1.collider.gameObject))
                {
                    enemiesInFOV.Add(hit1.collider.gameObject);
                }
                else if (hit2 && hit2.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit2.collider.gameObject))
                {
                    enemiesInFOV.Add(hit2.collider.gameObject);
                }
                else if (hit3 && hit3.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit3.collider.gameObject))
                {
                    enemiesInFOV.Add(hit3.collider.gameObject);
                }
                else if (hit4 && hit4.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit4.collider.gameObject))
                {
                    enemiesInFOV.Add(hit4.collider.gameObject);
                }
                else if (hit5 && hit5.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit5.collider.gameObject))
                {
                    enemiesInFOV.Add(hit5.collider.gameObject);
                }

                //"unsee" enemies
                for (int i = 0; i < enemiesInFOV.Count; i++)
                {
                    if (hit1 && hit1.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit2 && hit2.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit3 && hit3.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit4 && hit4.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit5 && hit5.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else
                    {
                        enemiesInFOV.Remove(enemiesInFOV[i]);
                    }
                }

                break;
            case FacingState.RIGHT:
                hit1 = Physics2D.Raycast(transform.position, transform.right, (sightDist + .2f));
                hit2 = Physics2D.Raycast(transform.position, (transform.right + transform.up + new Vector3(0, -.3f, 0)), (sightDist + .6f));
                hit3 = Physics2D.Raycast(transform.position, (transform.right - transform.up + new Vector3(0, .3f, 0)), (sightDist + .6f));
                hit4 = Physics2D.Raycast(transform.position, (transform.right - transform.up + new Vector3(0, .65f, 0)), (sightDist + .6f));
                hit5 = Physics2D.Raycast(transform.position, (transform.right + transform.up + new Vector3(0, -.65f, 0)), (sightDist + .6f));

                Debug.DrawRay(transform.position, (transform.right + transform.up + new Vector3(0, -.3f, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, (transform.right - transform.up + new Vector3(0, .3f, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, (transform.right - transform.up + new Vector3(0, .65f, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, (transform.right + transform.up + new Vector3(0, -.65f, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, transform.right * (sightDist + .2f), Color.green);


                if (hit1 && hit1.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                else if (hit2 && hit2.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                else if (hit3 && hit3.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                else if (hit4 && hit4.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                else if (hit5 && hit5.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                else
                {
                    AI.seePlayer = false;
                }

                //See enemies
                if (hit1 && hit1.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit1.collider.gameObject))
                {
                    enemiesInFOV.Add(hit1.collider.gameObject);
                }
                else if (hit2 && hit2.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit2.collider.gameObject))
                {
                    enemiesInFOV.Add(hit2.collider.gameObject);
                }
                else if (hit3 && hit3.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit3.collider.gameObject))
                {
                    enemiesInFOV.Add(hit3.collider.gameObject);
                }
                else if (hit4 && hit4.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit4.collider.gameObject))
                {
                    enemiesInFOV.Add(hit4.collider.gameObject);
                }
                else if (hit5 && hit5.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit5.collider.gameObject))
                {
                    enemiesInFOV.Add(hit5.collider.gameObject);
                }

                //"unsee" enemies
                for (int i = 0; i < enemiesInFOV.Count; i++)
                {
                    if (hit1 && hit1.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit2 && hit2.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit3 && hit3.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit4 && hit4.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit5 && hit5.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else
                    {
                        enemiesInFOV.Remove(enemiesInFOV[i]);
                    }
                }

                break;
            case FacingState.LEFT:
                hit1 = Physics2D.Raycast(transform.position, -transform.right, (sightDist + .2f));
                hit2 = Physics2D.Raycast(transform.position, (-transform.right + transform.up + new Vector3(0, -.3f, 0)), (sightDist + .6f));
                hit3 = Physics2D.Raycast(transform.position, (-transform.right - transform.up + new Vector3(0, .3f, 0)), (sightDist + .6f));
                hit4 = Physics2D.Raycast(transform.position, (-transform.right - transform.up + new Vector3(0, .65f, 0)), (sightDist + .6f));
                hit5 = Physics2D.Raycast(transform.position, (-transform.right + transform.up + new Vector3(0, -.65f, 0)), (sightDist + .6f));

                Debug.DrawRay(transform.position, (-transform.right + transform.up + new Vector3(0, -.3f, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, (-transform.right - transform.up + new Vector3(0, .3f, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, (-transform.right - transform.up + new Vector3(0, .65f, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, (-transform.right + transform.up + new Vector3(0, -.65f, 0)) * sightDist, Color.green);
                Debug.DrawRay(transform.position, -transform.right * (sightDist + .2f), Color.green);


             if (hit1 && hit1.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                if (hit2 && hit2.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                   
                }
                else if (hit3 && hit3.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                else if (hit4 && hit4.collider.tag == "Player")
                {
                    AI.seePlayer = true;
                    
                }
                else if (hit5 && hit5.collider.tag == "Player")
                {
                    AI.seePlayer = true;                    
                }
                else
                {
                    AI.seePlayer = false;
                }

                //See enemies
                if (hit1 && hit1.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit1.collider.gameObject))
                {
                    enemiesInFOV.Add(hit1.collider.gameObject);
                }
                else if (hit2 && hit2.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit2.collider.gameObject))
                {
                    enemiesInFOV.Add(hit2.collider.gameObject);
                }
                else if (hit3 && hit3.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit3.collider.gameObject))
                {
                    enemiesInFOV.Add(hit3.collider.gameObject);
                }
                else if (hit4 && hit4.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit4.collider.gameObject))
                {
                    enemiesInFOV.Add(hit4.collider.gameObject);
                }
                else if (hit5 && hit5.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit5.collider.gameObject))
                {
                    enemiesInFOV.Add(hit5.collider.gameObject);
                }

                //"unsee" enemies
                for (int i = 0; i < enemiesInFOV.Count; i++)
                {
                    if (hit1 && hit1.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit2 && hit2.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit3 && hit3.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit4 && hit4.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else if (hit5 && hit5.collider.gameObject == enemiesInFOV[i])
                    {
                    }
                    else
                    {
                        enemiesInFOV.Remove(enemiesInFOV[i]);
                    }
                }

                break;
        }
    }
}



