using UnityEngine;
using System.Collections;

public class InvisibilitySkill : MonoBehaviour
{
    public int coolDown;
    public float invisTimer;
    float durationTime;
    public bool isInvisible;
    public bool coolDownReady;
    SpriteRenderer spriteRend;
    KillScript ks;

    void Awake()
    {
        coolDown = 2;
        durationTime = 5f;
        invisTimer = 0f;
        isInvisible = false;
        coolDownReady = true;
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        ks = GetComponent<KillScript>();
    }

    void Update()
    {
        if (!ks.isSuckingBlood && Input.GetKeyDown(KeyCode.C))
        {
            if (coolDownReady)
            {
                isInvisible = true;
                coolDownReady = false;
                coolDown = 0;
            }
        }
        if (isInvisible)
        {
            spriteRend.color = new Color(1, 1, 1, .4f);
            invisTimer += 1 * Time.deltaTime;
            if (invisTimer >= durationTime || ks.isSuckingBlood)
            {
                isInvisible = false;
                invisTimer = 0f;
            }
        }
        else
        {
            spriteRend.color = new Color(1, 1, 1, 1f);
        }
        if (!coolDownReady && coolDown == 2)
        {
            coolDownReady = true;
        }
    }
}
