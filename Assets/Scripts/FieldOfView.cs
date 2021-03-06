﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FieldOfView : MonoBehaviour
{
    #region Variables
    public float fieldOfViewAngle;
    public float sightDist;
    EnemyAI AI;
    public List<GameObject> enemiesInFOV;
    public List<GameObject> enemies;
    public bool inSight;
    public bool seeDeadEnemy;
    CollapseScript collapseScript;
    SpriteRenderer spriteRend;
    Sprite deadSprite;
    Animator anim;
    LayerMask layerMask;
    LayerMask playerLayerMask;
    LayerMask lineCastIgnoreMask;
    GameObject player;
    PlayerCurrentRoomScript playerCurrentRoom;
    Vector3 pos;
    public Vector3 vel;
    InvisibilitySkill invisSkill;
    

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
        fieldOfViewAngle = 160f;
        inSight = false;
        seeDeadEnemy = false;
        collapseScript = GetComponent<CollapseScript>();
        AI = GetComponent<EnemyAI>();
        sightDist = 10f;
        currentFacingState = FacingState.UP;
        enemiesInFOV = new List<GameObject>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        layerMask = 1 << LayerMask.NameToLayer("TriggerArea") | 1 << LayerMask.NameToLayer("Furniture") | 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("LinecastIgnore");
        playerLayerMask = 1 << LayerMask.NameToLayer("TriggerArea") | 1 << LayerMask.NameToLayer("Furniture") | 1 << LayerMask.NameToLayer("Enemy");
        lineCastIgnoreMask = 1 << LayerMask.NameToLayer("LinecastIgnore") | 1 << LayerMask.NameToLayer("TriggerArea");
        layerMask = ~layerMask;
        lineCastIgnoreMask = ~lineCastIgnoreMask;
        playerLayerMask = ~playerLayerMask;
        playerCurrentRoom = player.GetComponentInChildren<PlayerCurrentRoomScript>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        invisSkill = player.GetComponent<InvisibilitySkill>();

    }

    public void Update()
    {
        vel = (transform.position - pos).normalized;
        pos = transform.position;

        SpriteRend();

        if (AI.currentEnemyState != EnemyAI.EnemyState.Dead && AI.currentEnemyState != EnemyAI.EnemyState.Collapsed && AI.currentEnemyState != EnemyAI.EnemyState.IsBeingKilled)
        {
            //InSight();
            //Investigate();
            Facing();
            FoV();

            if (seeDeadEnemy && AI.currentEnemyState == EnemyAI.EnemyState.Patrolling)
            {
                AI.currentEnemyState = EnemyAI.EnemyState.Investigating;
            }
        }
        else if (AI.currentEnemyState == EnemyAI.EnemyState.Dead || AI.currentEnemyState == EnemyAI.EnemyState.Collapsed)
        {
            AI.seePlayer = false;
            enemiesInFOV.Clear();

        }
    }

    void Facing()
    {
       


        if (vel != Vector3.zero)
        {
            if (vel.y < 0 && vel.y < vel.x && vel.y < -vel.x)
            {
                if (currentFacingState != FacingState.DOWN)
                    currentFacingState = FacingState.DOWN;

            }
            else if (vel.x < 0 && vel.x < vel.y && vel.x < -vel.y)
            {
                if (currentFacingState != FacingState.LEFT)
                    currentFacingState = FacingState.LEFT;

            }
            else if (vel.y > 0 && vel.y > vel.x && vel.y > -vel.x)
            {
                if (currentFacingState != FacingState.UP)
                    currentFacingState = FacingState.UP;

            }

            else if (vel.x > 0 && vel.x > vel.y && vel.x > -vel.y)
            {
                if (currentFacingState != FacingState.RIGHT)
                    currentFacingState = FacingState.RIGHT;

            }
        }
    }

    void FoV()
    {
        Color color;
        if (AI.seePlayer)
        {
            color = Color.green;
        }
        else
        {
            color = Color.red;
        }
        switch (currentFacingState)
        {
            case FacingState.UP:
                Vector3 targetDir = player.transform.position - transform.position;
                Vector3 sightDir = transform.up;
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].GetComponent<EnemyAI>().currentRoom == AI.currentRoom)
                    {
                        Vector3 targetDire = enemies[i].transform.position - transform.position;
                        float angler = Vector3.Angle(targetDire, sightDir);

                        if (angler < fieldOfViewAngle * .5f)
                        {
                            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDire.normalized, Mathf.Infinity, layerMask);
                            Debug.DrawLine(transform.position, enemies[i].transform.position, Color.red);

                            if (hit && hit.collider.tag == "Enemy" && hit.collider.gameObject.GetComponent<EnemyAI>().currentRoom == AI.currentRoom)
                            {
                                if (!enemiesInFOV.Contains(enemies[i]))
                                {
                                    enemiesInFOV.Add(enemies[i]);
                                }
                            }
                            
                        }
                        else
                        {
                            enemiesInFOV.Remove(enemies[i]);
                        }
                    }

                }

                float angle = Vector3.Angle(targetDir, sightDir);

                if (angle < fieldOfViewAngle * .5f)
                {
                    if (playerCurrentRoom.currentRoom == AI.currentRoom)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir.normalized, Mathf.Infinity, playerLayerMask);
                        Debug.DrawLine(transform.position, player.transform.position, color);

                        if (hit && hit.collider.gameObject == player && !invisSkill.isInvisible)
                        {

                            AI.seePlayer = true;

                        }
                        else if (hit && hit.collider.tag == "PlayerFeet" && !invisSkill.isInvisible)
                        {

                            AI.seePlayer = true;
                        }
                        else
                        {
                            AI.seePlayer = false;
                        }
                    }
                    else
                    {
                        AI.seePlayer = false;
                    }
                }
                else
                {
                    AI.seePlayer = false;
                }
                break;
            case FacingState.RIGHT:
                targetDir = player.transform.position - transform.position;
                sightDir = transform.right;

                angle = Vector3.Angle(targetDir, sightDir);

                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].GetComponent<EnemyAI>().currentRoom == AI.currentRoom)
                    {
                        Vector3 targetDire = enemies[i].transform.position - transform.position;
                        float angler = Vector3.Angle(targetDire, sightDir);

                        if (angler < fieldOfViewAngle * .5f)
                        {
                            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDire.normalized, Mathf.Infinity, layerMask);
                            Debug.DrawLine(transform.position, enemies[i].transform.position, Color.red);

                            if (hit && hit.collider.tag == "Enemy" && hit.collider.gameObject.GetComponent<EnemyAI>().currentRoom == AI.currentRoom)
                            {
                                if (!enemiesInFOV.Contains(enemies[i]))
                                {
                                    enemiesInFOV.Add(enemies[i]);
                                }
                            }
                            
                        }
                        else
                        {
                            enemiesInFOV.Remove(enemies[i]);
                        }
                    }
                }

                if (angle < fieldOfViewAngle * .5f)
                {
                    if (playerCurrentRoom.currentRoom == AI.currentRoom)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir.normalized, Mathf.Infinity, playerLayerMask);
                        Debug.DrawLine(transform.position, player.transform.position, color);

                        if (hit && hit.collider.gameObject == player && !invisSkill.isInvisible)
                        {

                            AI.seePlayer = true;
                        }
                        else if (hit && hit.collider.tag == "PlayerFeet" && !invisSkill.isInvisible)
                        {

                            AI.seePlayer = true;
                        }
                        else
                        {
                            AI.seePlayer = false;
                        }
                    }
                    else
                    {
                        AI.seePlayer = false;
                    }
                }
                else
                {
                    AI.seePlayer = false;
                }
                break;
            case FacingState.DOWN:
                targetDir = player.transform.position - transform.position;
                sightDir = -transform.up;

                angle = Vector3.Angle(targetDir, sightDir);

                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].GetComponent<EnemyAI>().currentRoom == AI.currentRoom)
                    {
                        Vector3 targetDire = enemies[i].transform.position - transform.position;
                        float angler = Vector3.Angle(targetDire, sightDir);

                        if (angler < fieldOfViewAngle * .5f)
                        {
                            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDire.normalized, Mathf.Infinity, layerMask);
                            Debug.DrawLine(transform.position, enemies[i].transform.position, Color.red);

                            if (hit && hit.collider.tag == "Enemy")
                            {
                                if (!enemiesInFOV.Contains(enemies[i]))
                                {
                                    enemiesInFOV.Add(enemies[i]);
                                }
                            }
                        }
                        else
                        {
                            enemiesInFOV.Remove(enemies[i]);
                        }
                    }

                }

                if (angle < fieldOfViewAngle * .5f)
                {
                    if (playerCurrentRoom.currentRoom == AI.currentRoom)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir.normalized, Mathf.Infinity, playerLayerMask);
                        Debug.DrawLine(transform.position, player.transform.position, color);

                        if (hit && hit.collider.gameObject == player && !invisSkill.isInvisible)
                        {

                            AI.seePlayer = true;
                        }
                        else if (hit && hit.collider.tag == "PlayerFeet" && !invisSkill.isInvisible)
                        {

                            AI.seePlayer = true;
                        }
                        else
                        {
                            AI.seePlayer = false;
                        }
                    }
                    else
                    {
                        AI.seePlayer = false;
                    }
                }
                else
                {
                    AI.seePlayer = false;
                }
                break;
            case FacingState.LEFT:
                targetDir = player.transform.position - transform.position;
                sightDir = -transform.right;

                angle = Vector3.Angle(targetDir, sightDir);

                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].GetComponent<EnemyAI>().currentRoom == AI.currentRoom)
                    {
                        Vector3 targetDire = enemies[i].transform.position - transform.position;
                        float angler = Vector3.Angle(targetDire, sightDir);

                        if (angler < fieldOfViewAngle * .5f)
                        {
                            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDire.normalized, Mathf.Infinity, layerMask);
                            Debug.DrawLine(transform.position, enemies[i].transform.position, Color.red);

                            if (hit && hit.collider.tag == "Enemy")
                            {
                                if (!enemiesInFOV.Contains(enemies[i]))
                                {
                                    enemiesInFOV.Add(enemies[i]);
                                }
                            }
                            
                        }
                        else
                        {
                            enemiesInFOV.Remove(enemies[i]);
                        }
                    }
                }

                if (angle < fieldOfViewAngle * .5f)
                {
                    if (playerCurrentRoom.currentRoom == AI.currentRoom)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir.normalized, Mathf.Infinity, playerLayerMask);
                        Debug.DrawLine(transform.position, player.transform.position, color);

                        if (hit && hit.collider.gameObject == player && !invisSkill.isInvisible)
                        {

                            AI.seePlayer = true;
                        }
                        else if (hit && hit.collider.tag == "PlayerFeet" && !invisSkill.isInvisible)
                        {

                            AI.seePlayer = true;
                        }

                        else
                        {
                            AI.seePlayer = false;
                        }
                    }
                    else
                    {
                        AI.seePlayer = false;
                    }
                }
                else
                {
                    AI.seePlayer = false;
                }
                break;
        }
        if (enemiesInFOV.Count > 0)
        {
            for (int i = 0; i < enemiesInFOV.Count; i++)
            {
                EnemyAI _ai = enemiesInFOV[i].GetComponent<EnemyAI>();

                if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious && enemiesInFOV[i].activeInHierarchy)
                {
                    seeDeadEnemy = true;
                    //collapseScript.isSuspicious = true;
                    AI.isWaiting = false;
                }
                else if (!enemiesInFOV[i].activeInHierarchy)
                {
                    enemiesInFOV.Remove(enemiesInFOV[i]);
                }
                else if (_ai.currentRoom != AI.currentRoom)
                {
                    enemiesInFOV.Remove(enemiesInFOV[i]);
                }
                
            }
        }
        else
        {
            seeDeadEnemy = false;
            
        }
    }
    /* void Investigate()
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
                 RaycastHit2D hit6 = Physics2D.Raycast(rayPos, (transform.up + transform.right + new Vector3(-2.1f, 0, 0)), (sightDist), layerMask);
                 RaycastHit2D hit7 = Physics2D.Raycast(rayPos, (transform.up - transform.right + new Vector3(2.1f, 0, 0)), (sightDist), layerMask);


                 Debug.DrawRay(rayPos, (transform.up + transform.right + new Vector3(-.3f, 0, 0)) * (sightDist - 2f), Color.green);
                 Debug.DrawRay(rayPos, (transform.up - transform.right + new Vector3(.3f, 0, 0)) * (sightDist - 2f), Color.green);
                 Debug.DrawRay(rayPos, (transform.up - transform.right + new Vector3(.65f, 0, 0)) * sightDist, Color.green);
                 Debug.DrawRay(rayPos, (transform.up + transform.right + new Vector3(-.65f, 0, 0)) * sightDist, Color.green);
                 Debug.DrawRay(rayPos, (transform.up + transform.right + new Vector3(-2.1f, 0, 0)) * (sightDist - 4f), Color.green);
                 Debug.DrawRay(rayPos, (transform.up - transform.right + new Vector3(2.1f, 0, 0)) * (sightDist - 4f), Color.green);
                 Debug.DrawRay(rayPos, transform.up * (sightDist + 1f), Color.green);

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
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit2 && hit2.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit3 && hit3.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit4 && hit4.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit5 && hit5.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
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
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit2 && hit2.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit3 && hit3.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit4 && hit4.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit5 && hit5.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else
                     {
                         enemiesInFOV.Remove(enemiesInFOV[i]);
                         seeDeadEnemy = false;
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
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit2 && hit2.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit3 && hit3.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit4 && hit4.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit5 && hit5.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else
                     {
                         enemiesInFOV.Remove(enemiesInFOV[i]);
                         seeDeadEnemy = false;
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
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit2 && hit2.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit3 && hit3.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit4 && hit4.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else if (hit5 && hit5.collider.gameObject == enemiesInFOV[i])
                     {
                         if (_ai.currentEnemyState == EnemyAI.EnemyState.Dead && !collapseScript.isSuspicious)
                         {
                             seeDeadEnemy = true;
                             //collapseScript.isSuspicious = true;
                             AI.isWaiting = false;
                         }
                     }
                     else
                     {
                         enemiesInFOV.Remove(enemiesInFOV[i]);
                         seeDeadEnemy = false;
                     }
                 }

                 break;
         }
     }*/
    void SpriteRend()
    {
        if (AI.currentEnemyState == EnemyAI.EnemyState.Dead)
        {
            anim.SetBool("Dead", true);
            //anim.enabled = false;
            //spriteRend.sprite = deadSprite;
        }
        else
        {
            if (vel == Vector3.zero)
            {
                anim.SetBool("Walking", false);
            }
            else if (AI.currentEnemyState == EnemyAI.EnemyState.Collapsed ||
                        AI.currentEnemyState == EnemyAI.EnemyState.Dead ||
                        AI.currentEnemyState == EnemyAI.EnemyState.IsBeingKilled)
            {
                anim.SetBool("Walking", false);
            }
            else
            {
                anim.SetBool("Walking", true);
            }

            switch (currentFacingState)
            {
                case FacingState.LEFT:
                    anim.SetInteger("FacingState", 3);
                    //spriteRend.sprite = sprites[0];
                    break;
                case FacingState.UP:
                    anim.SetInteger("FacingState", 2);
                    //spriteRend.sprite = sprites[1];
                    break;
                case FacingState.DOWN:
                    anim.SetInteger("FacingState", 1);
                    //spriteRend.sprite = sprites[2];
                    break;
                case FacingState.RIGHT:
                    anim.SetInteger("FacingState", 4);
                    //spriteRend.sprite = sprites[3];
                    break;
            }
        }
    }

}



