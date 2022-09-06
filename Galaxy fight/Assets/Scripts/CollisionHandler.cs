using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticles;
    private void OnTriggerEnter(Collider other)
    {
        StartCrashSequence();
    }

    private void StartCrashSequence()
    {
        GetComponent<PlayerController>().enabled = false;
        explosionParticles.Play();
        var renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var varRenderer in renderers)
        {
            varRenderer.enabled = false;
        }
        GetComponent<BoxCollider>().enabled = false;
        Invoke(nameof(ReloadLevel), 1f);
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
