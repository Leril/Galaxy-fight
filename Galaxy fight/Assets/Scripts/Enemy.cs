using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject enemyVFX;
    [SerializeField] private GameObject enemyHitVFX;
    [SerializeField] private int scoreToAdd = 15;
    [SerializeField] private int hitPoints = 3;

    private Scoreboard _scoreboard;

    private void Start()
    {
        _scoreboard = FindObjectOfType<Scoreboard>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    private void ProcessHit()
    {
        _scoreboard.IncreaseScore(scoreToAdd);
        Instantiate(enemyHitVFX, transform.position, Quaternion.identity);
        hitPoints--;

        if (hitPoints == 0)
        {
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        Instantiate(enemyVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
