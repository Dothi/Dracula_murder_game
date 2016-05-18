using UnityEngine;
using System.Collections;

public class CollapseScript : MonoBehaviour {

    public bool isCollapsed;
    public bool isSuspicious = false;
    float collapseTime = 0;
    public float collapseDuration = 40f;
    float suspiciousTime = 0f;
    float suspiciousDuration = 600f;
    Animator anim;
    
    
    EnemyAI AI;

    void Start()
    {
        AI = GetComponent<EnemyAI>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (AI.currentEnemyState != EnemyAI.EnemyState.Dead)
        {
            if (isCollapsed)
            {
                AI.currentEnemyState = EnemyAI.EnemyState.Collapsed;
                //transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.gray;
                if (!anim.GetBool("Stunned"))
                {
                    anim.transform.localScale = new Vector3(0.16f, 0.16f, 1f);
                    anim.SetBool("Stunned", true);
                }
                collapseTime += 10 * Time.deltaTime;
                if (collapseTime >= collapseDuration)
                {
                    isCollapsed = false;
                    collapseTime = 0;
                    isSuspicious = true;

                }
            }
            else
            {
                //transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;
                if (anim.GetBool("Stunned"))
                {
                    anim.transform.localScale = new Vector3(0.21f, 0.21f, 1f);
                    anim.SetBool("Stunned", false);
                }
            }
            if (isSuspicious && !isCollapsed && AI.currentEnemyState != EnemyAI.EnemyState.IsBeingKilled)
            {
                
                suspiciousTime += 10 * Time.deltaTime;
                
                AI.currentEnemyState = EnemyAI.EnemyState.Suspicious;
                if (suspiciousTime >= suspiciousDuration)
                {
                    isSuspicious = false;
                    suspiciousTime = 0;
                }
            }
            else if (!isSuspicious && !isCollapsed && AI.currentEnemyState != EnemyAI.EnemyState.IsBeingKilled && AI.currentEnemyState != EnemyAI.EnemyState.Investigating)
            {
                AI.currentEnemyState = EnemyAI.EnemyState.Patrolling;
            }
        }
    }
}
