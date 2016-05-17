using UnityEngine;
using System.Collections;

public class EyeScript : MonoBehaviour
{
    SpriteRenderer spriteRend;
    EnemyAI AI;
    Vector2 eyeScale;
    float colorTimer = 0;
    TriggerAreaScript TAS;
    NearbyEnemiesScript NES;

    void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.enabled = false;
        AI = GetComponentInParent<EnemyAI>();
        eyeScale = transform.localScale;
        TAS = transform.parent.Find("TriggerArea").GetComponent<TriggerAreaScript>();
        NES = GameObject.FindGameObjectWithTag("Player").GetComponent<NearbyEnemiesScript>();
    }

    void Update()
    {
        if (AI.currentEnemyState == EnemyAI.EnemyState.Suspicious)
        {
            spriteRend.enabled = true;

            if (TAS.timer > NES.timer)
            {
                colorTimer = TAS.timer;
            }
            else
            {
                colorTimer = NES.timer;
            }

            if (colorTimer == 0)
            {
                spriteRend.color = new Color(1, 1, 1);
            }
            else if (colorTimer < 1)
            {
                spriteRend.color = new Color(1, (1 - colorTimer), (1 - colorTimer));
                if (TAS.timer > 1)
                {
                    spriteRend.color = new Color(1, 0, 0);
                }
            }
        }
        else
        {
            spriteRend.enabled = false;
            colorTimer = 0;
        }
    }
}
