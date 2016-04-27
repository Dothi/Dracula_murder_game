using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FieldOfView : MonoBehaviour
{
    #region Variables
    public float sightDist;
    EnemyAI AI;
    public List<GameObject> enemiesInFOV;
    public bool inSight;
    CollapseScript collapseScript;
    public RaycastHit2D[] hits;
    public Sprite[] sprites;
    SpriteRenderer spriteRend;
    LayerMask layerMask;
    LayerMask lineCastIgnoreMask;
    GameObject player;
    #endregion
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
        inSight = false;
        collapseScript = GetComponent<CollapseScript>();
        AI = GetComponent<EnemyAI>();
        sightDist = 10f;
        currentFacingState = FacingState.UP;
        enemiesInFOV = new List<GameObject>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        layerMask = 1 << LayerMask.NameToLayer("TriggerArea");
        lineCastIgnoreMask = 1 << LayerMask.NameToLayer("LinecastIgnore") | 1 << LayerMask.NameToLayer("TriggerArea");
        layerMask = ~layerMask;
        lineCastIgnoreMask = ~lineCastIgnoreMask;

    }

    public void Update()
    {
        Facing();
        if (AI.currentEnemyState != EnemyAI.EnemyState.Dead)
        {
            SpriteRend();
        }
        if (AI.currentEnemyState != EnemyAI.EnemyState.Dead && AI.currentEnemyState != EnemyAI.EnemyState.Collapsed && AI.currentEnemyState != EnemyAI.EnemyState.IsBeingKilled)
        {
            InSight();
            Investigate();
        }
        else
        {
            AI.seePlayer = false;
            enemiesInFOV.Clear();
        }
    }

    void Facing()
    {
        if (AI.directionOfTravel.y < 0 && AI.directionOfTravel.y < AI.directionOfTravel.x && AI.directionOfTravel.y < -AI.directionOfTravel.x)
        {
            currentFacingState = FacingState.DOWN;

        }
        else if (AI.directionOfTravel.x < 0 && AI.directionOfTravel.x < AI.directionOfTravel.y && AI.directionOfTravel.x < -AI.directionOfTravel.y)
        {
            currentFacingState = FacingState.LEFT;

        }
        else if (AI.directionOfTravel.y > 0 && AI.directionOfTravel.y > AI.directionOfTravel.x && AI.directionOfTravel.y > -AI.directionOfTravel.x)
        {
            currentFacingState = FacingState.UP;

        }

        else if (AI.directionOfTravel.x > 0 && AI.directionOfTravel.x > AI.directionOfTravel.y && AI.directionOfTravel.x > -AI.directionOfTravel.y)
        {
            currentFacingState = FacingState.RIGHT;

        }


    }

    void Investigate()
    {
        Vector3 rayPos = transform.position + new Vector3(0, 1, 0);
        switch (currentFacingState)
        {
            case FacingState.UP:
                RaycastHit2D hit1 = Physics2D.Raycast(rayPos, transform.up, (sightDist + .2f), layerMask);
                RaycastHit2D hit2 = Physics2D.Raycast(rayPos, (transform.up + transform.right + new Vector3(-.3f, 0, 0)), (sightDist + .6f), layerMask);
                RaycastHit2D hit3 = Physics2D.Raycast(rayPos, (transform.up - transform.right + new Vector3(.3f, 0, 0)), (sightDist + .6f), layerMask);
                RaycastHit2D hit4 = Physics2D.Raycast(rayPos, (transform.up - transform.right + new Vector3(.65f, 0, 0)), (sightDist + .6f), layerMask);
                RaycastHit2D hit5 = Physics2D.Raycast(rayPos, (transform.up + transform.right + new Vector3(-.65f, 0, 0)), (sightDist + .6f), layerMask);

                Debug.DrawRay(rayPos, (transform.up + transform.right + new Vector3(-.3f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, (transform.up - transform.right + new Vector3(.3f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, (transform.up - transform.right + new Vector3(.65f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, (transform.up + transform.right + new Vector3(-.65f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, transform.up * (sightDist + .2f), Color.green);

                //See player                                                                 
                if (hit1 && hit1.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;
                }
                else if (hit2 && hit2.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit3 && hit3.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit4 && hit4.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit5 && hit5.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;
                }
                else
                {
                    AI.seePlayer = false;
                }

                //See enemies
                if (hit1 && hit1.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit1.collider.gameObject) && hit1.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit1.collider.gameObject);
                }
                else if (hit2 && hit2.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit2.collider.gameObject) && hit2.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit2.collider.gameObject);
                }
                else if (hit3 && hit3.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit3.collider.gameObject) && hit3.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit3.collider.gameObject);
                }
                else if (hit4 && hit4.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit4.collider.gameObject) && hit4.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit4.collider.gameObject);
                }
                else if (hit5 && hit5.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit5.collider.gameObject) && hit5.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit5.collider.gameObject);
                }

                //"unsee" enemies
                for (int i = 0; i < enemiesInFOV.Count; i++)
                {
                    EnemyAI _ai = enemiesInFOV[i].GetComponent<EnemyAI>();
                    if (hit1 && hit1.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit2 && hit2.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit3 && hit3.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit4 && hit4.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit5 && hit5.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else
                    {
                        enemiesInFOV.Remove(enemiesInFOV[i]);
                    }
                }

                break;
            case FacingState.DOWN:
                hit1 = Physics2D.Raycast(rayPos, -transform.up, (sightDist + .2f), layerMask);
                hit2 = Physics2D.Raycast(rayPos, (-transform.up + transform.right + new Vector3(-.3f, 0, 0)), (sightDist + .6f), layerMask);
                hit3 = Physics2D.Raycast(rayPos, (-transform.up - transform.right + new Vector3(.3f, 0, 0)), (sightDist + .6f), layerMask);
                hit4 = Physics2D.Raycast(rayPos, (-transform.up - transform.right + new Vector3(.65f, 0, 0)), (sightDist + .6f), layerMask);
                hit5 = Physics2D.Raycast(rayPos, (-transform.up + transform.right + new Vector3(-.65f, 0, 0)), (sightDist + .6f), layerMask);

                Debug.DrawRay(rayPos, (-transform.up + transform.right + new Vector3(-.3f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, (-transform.up - transform.right + new Vector3(.3f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, (-transform.up - transform.right + new Vector3(.65f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, (-transform.up + transform.right + new Vector3(-.65f, 0, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, -transform.up * (sightDist + .2f), Color.green);


                if (hit1 && hit1.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit2 && hit2.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit3 && hit3.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit4 && hit4.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit5 && hit5.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else
                {
                    AI.seePlayer = false;
                }

                //See enemies
                if (hit1 && hit1.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit1.collider.gameObject) && hit1.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit1.collider.gameObject);
                }
                else if (hit2 && hit2.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit2.collider.gameObject) && hit2.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit2.collider.gameObject);
                }
                else if (hit3 && hit3.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit3.collider.gameObject) && hit3.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit3.collider.gameObject);
                }
                else if (hit4 && hit4.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit4.collider.gameObject) && hit4.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit4.collider.gameObject);
                }
                else if (hit5 && hit5.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit5.collider.gameObject) && hit5.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit5.collider.gameObject);
                }

                //"unsee" enemies

                for (int i = 0; i < enemiesInFOV.Count; i++)
                {
                    EnemyAI _ai = enemiesInFOV[i].GetComponent<EnemyAI>();
                    if (hit1 && hit1.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit2 && hit2.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit3 && hit3.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit4 && hit4.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit5 && hit5.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else
                    {
                        enemiesInFOV.Remove(enemiesInFOV[i]);
                    }
                }

                break;
            case FacingState.RIGHT:
                hit1 = Physics2D.Raycast(rayPos, transform.right, (sightDist + .2f), layerMask);
                hit2 = Physics2D.Raycast(rayPos, (transform.right + transform.up + new Vector3(0, -.3f, 0)), (sightDist + .6f), layerMask);
                hit3 = Physics2D.Raycast(rayPos, (transform.right - transform.up + new Vector3(0, .3f, 0)), (sightDist + .6f), layerMask);
                hit4 = Physics2D.Raycast(rayPos, (transform.right - transform.up + new Vector3(0, .65f, 0)), (sightDist + .6f), layerMask);
                hit5 = Physics2D.Raycast(rayPos, (transform.right + transform.up + new Vector3(0, -.65f, 0)), (sightDist + .6f), layerMask);

                Debug.DrawRay(rayPos, (transform.right + transform.up + new Vector3(0, -.3f, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, (transform.right - transform.up + new Vector3(0, .3f, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, (transform.right - transform.up + new Vector3(0, .65f, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, (transform.right + transform.up + new Vector3(0, -.65f, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, transform.right * (sightDist + .2f), Color.green);


                if (hit1 && hit1.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit2 && hit2.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit3 && hit3.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit4 && hit4.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit5 && hit5.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else
                {
                    AI.seePlayer = false;
                }

                //See enemies
                if (hit1 && hit1.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit1.collider.gameObject) && hit1.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit1.collider.gameObject);
                }
                else if (hit2 && hit2.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit2.collider.gameObject) && hit2.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit2.collider.gameObject);
                }
                else if (hit3 && hit3.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit3.collider.gameObject) && hit3.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit3.collider.gameObject);
                }
                else if (hit4 && hit4.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit4.collider.gameObject) && hit4.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit4.collider.gameObject);
                }
                else if (hit5 && hit5.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit5.collider.gameObject) && hit5.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit5.collider.gameObject);
                }

                //"unsee" enemies
                for (int i = 0; i < enemiesInFOV.Count; i++)
                {
                    EnemyAI _ai = enemiesInFOV[i].GetComponent<EnemyAI>();
                    if (hit1 && hit1.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit2 && hit2.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit3 && hit3.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit4 && hit4.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit5 && hit5.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else
                    {
                        enemiesInFOV.Remove(enemiesInFOV[i]);
                    }
                }

                break;
            case FacingState.LEFT:
                hit1 = Physics2D.Raycast(rayPos, -transform.right, (sightDist + .2f), layerMask);
                hit2 = Physics2D.Raycast(rayPos, (-transform.right + transform.up + new Vector3(0, -.3f, 0)), (sightDist + .6f), layerMask);
                hit3 = Physics2D.Raycast(rayPos, (-transform.right - transform.up + new Vector3(0, .3f, 0)), (sightDist + .6f), layerMask);
                hit4 = Physics2D.Raycast(rayPos, (-transform.right - transform.up + new Vector3(0, .65f, 0)), (sightDist + .6f), layerMask);
                hit5 = Physics2D.Raycast(rayPos, (-transform.right + transform.up + new Vector3(0, -.65f, 0)), (sightDist + .6f), layerMask);

                Debug.DrawRay(rayPos, (-transform.right + transform.up + new Vector3(0, -.3f, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, (-transform.right - transform.up + new Vector3(0, .3f, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, (-transform.right - transform.up + new Vector3(0, .65f, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, (-transform.right + transform.up + new Vector3(0, -.65f, 0)) * sightDist, Color.green);
                Debug.DrawRay(rayPos, -transform.right * (sightDist + .2f), Color.green);


                if (hit1 && hit1.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                if (hit2 && hit2.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit3 && hit3.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit4 && hit4.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;

                }
                else if (hit5 && hit5.collider.tag == "Player" && inSight)
                {
                    AI.seePlayer = true;
                }
                else
                {
                    AI.seePlayer = false;
                }

                //See enemies
                if (hit1 && hit1.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit1.collider.gameObject) && hit1.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit1.collider.gameObject);
                }
                else if (hit2 && hit2.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit2.collider.gameObject) && hit2.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit2.collider.gameObject);
                }
                else if (hit3 && hit3.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit3.collider.gameObject) && hit3.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit3.collider.gameObject);
                }
                else if (hit4 && hit4.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit4.collider.gameObject) && hit4.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit4.collider.gameObject);
                }
                else if (hit5 && hit5.collider.tag == "Enemy" && !enemiesInFOV.Contains<GameObject>(hit5.collider.gameObject) && hit5.collider.gameObject.GetComponent<EnemyAI>().currentRoom == GetComponent<EnemyAI>().currentRoom)
                {
                    enemiesInFOV.Add(hit5.collider.gameObject);
                }

                //"unsee" enemies
                for (int i = 0; i < enemiesInFOV.Count; i++)
                {
                    EnemyAI _ai = enemiesInFOV[i].GetComponent<EnemyAI>();
                    if (hit1 && hit1.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit2 && hit2.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit3 && hit3.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit4 && hit4.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else if (hit5 && hit5.collider.gameObject == enemiesInFOV[i])
                    {
                        if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                        {
                            collapseScript.isSuspicious = true;
                            AI.isWaiting = false;
                        }
                    }
                    else
                    {
                        enemiesInFOV.Remove(enemiesInFOV[i]);
                    }
                }

                break;
        }
    }
    void SpriteRend()
    {
        switch (currentFacingState)
        {
            case FacingState.LEFT:
                spriteRend.sprite = sprites[0];
                break;
            case FacingState.UP:
                spriteRend.sprite = sprites[1];
                break;
            case FacingState.DOWN:
                spriteRend.sprite = sprites[2];
                break;
            case FacingState.RIGHT:
                spriteRend.sprite = sprites[3];
                break;
        }
    }
    void InSight()
    {
        RaycastHit2D lineHit = Physics2D.Linecast(transform.position, player.transform.position, lineCastIgnoreMask);

        if (lineHit && lineHit.collider.tag != "PlayerFeet")
        {
            inSight = false;
        }
        else
        {
            inSight = true;
        }
    }
}



