using UnityEngine;
using System.Collections;

public class CollapseScript : MonoBehaviour {

    public bool isCollapsed;
    public bool isSuspicious = false;
    float collapseTime = 0;
    public float collapseDuration = 40f;
    float suspiciousTime = 0f;
    float suspiciousDuration = 600f;
    
    EnemyAI AI;

    void Start()
    {
        AI = GetComponent<EnemyAI>();
    }

    void Update()
    {
        if (AI.currentEnemyState != EnemyAI.EnemyState.Dead)
        {
            if (isCollapsed)
            {
                AI.currentEnemyState = EnemyAI.EnemyState.Collapsed;
                transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.gray;
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
                transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (isSuspicious && !isCollapsed)
            {
                suspiciousTime += 10 * Time.deltaTime;
                AI.currentEnemyState = EnemyAI.EnemyState.Suspicious;
                if (suspiciousTime >= suspiciousDuration)
                {
                    isSuspicious = false;
                    suspiciousTime = 0;
                }
            }
            else if (!isSuspicious && !isCollapsed)
            {
                AI.currentEnemyState = EnemyAI.EnemyState.Patrolling;
            }
        }
    }
}
