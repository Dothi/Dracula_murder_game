using UnityEngine;
using System.Collections;

public class CollapseScript : MonoBehaviour {

    public bool isCollapsed;
    float collapseTime = 0;
    public float collapseDuration = 40;
    EnemyAI AI;

    void Start()
    {
        AI = GetComponent<EnemyAI>();
    }

    void Update()
    {
        if (isCollapsed)
        {
            transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.gray;
            collapseTime += 10 * Time.deltaTime;
            if (collapseTime >= collapseDuration)
            {
                isCollapsed = false;
                collapseTime = 0;
                AI.currentEnemyState = EnemyAI.EnemyState.Suspicious;

            }
        }
        else
        {
            transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.red;
        }

    }
}
