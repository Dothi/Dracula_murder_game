using UnityEngine;
using System.Collections;

public class DeadSpriteScript : MonoBehaviour
{
    public Sprite deadSprite;
    SpriteRenderer spriteRend;
    EnemyAI ai;

    void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        ai = GetComponentInParent<EnemyAI>();
    }

    void Update()
    {
        
        if (ai.currentEnemyState == EnemyAI.EnemyState.Dead)
        {
            spriteRend.sprite = deadSprite;
        }
        
    }
}
