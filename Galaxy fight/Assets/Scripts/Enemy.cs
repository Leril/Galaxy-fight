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
    private GameObject _parent;

    private void Start()
    {
        _scoreboard = FindObjectOfType<Scoreboard>();
        _parent = GameObject.FindWithTag("SpawnAtRuntime");

        GetRigidbody();
    }

    private void GetRigidbody()
    {
        var rBody = GetComponent<Rigidbody>();

        if (rBody == null)
        {
            rBody = gameObject.AddComponent<Rigidbody>();
        }

        rBody.useGravity = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    private void ProcessHit()
    {
        _scoreboard.IncreaseScore(scoreToAdd);
        Instantiate(enemyHitVFX, transform.position, Quaternion.identity).transform.parent = _parent.transform;
        hitPoints--;

        if (hitPoints == 0)
        {
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        Instantiate(enemyVFX, transform.position, Quaternion.identity).transform.parent = _parent.transform ;
        Destroy(gameObject);
    }
}
