using UnityEngine;
using System.Collections;

public class EyeScript : MonoBehaviour
{

    SpriteRenderer spriteRend;
    EnemyAI AI;

    void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.enabled = false;
        AI = GetComponentInParent<EnemyAI>();
    }

    void Update()
    {
        if (AI.currentEnemyState == EnemyAI.EnemyState.Suspicious)
        {
            spriteRend.enabled = true;
        }
        else
        {
            spriteRend.enabled = false;
        }
    }
}
