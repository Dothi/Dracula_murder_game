using UnityEngine;
using System.Collections;

public class BodyFallSound : MonoBehaviour
{
    bool hasPlayed;
    AudioSource audioSource;
    EnemyAI AI;
    public AudioClip bodyFall;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        AI = GetComponentInParent<EnemyAI>();
        hasPlayed = false;
    }

    void Update()
    {
        if(AI.currentEnemyState == EnemyAI.EnemyState.Dead && !hasPlayed)
        {
            audioSource.clip = bodyFall;
            audioSource.Play();
            hasPlayed = true;
        }
    }
}
