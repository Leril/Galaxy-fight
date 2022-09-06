using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("General setup settings")]
    [SerializeField] private InputAction movement;
    [SerializeField] private InputAction fire;
    
    [Header("Reference to all lasers")]
    [Tooltip("Put all lasers here")][SerializeField] private GameObject[] lasers;

    [Tooltip("How fast ship moves up and down")][SerializeField] private float controlSpeed = 30f;
    [SerializeField] private float xRange = 10f;
    [SerializeField] private float yRange = 7f;

    [Header("Screen position based tuning")]
    [SerializeField] private float positionPitchFactor = -2f;
    [SerializeField] private float positionYawFactor = 2;
    
    [Header("Player input based tuning")]
    [SerializeField] private float controlPitchFactor = -10f;
    [SerializeField] private float controlRollFactor = -20f;
    
    [SerializeField] private float rotationFactor = 5f;

    private float xThrow;
    private float yThrow;

    private void OnEnable()
    {
        movement.Enable();
        fire.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        fire.Disable();
    }

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    private void ProcessTranslation()
    {
        xThrow = movement.ReadValue<Vector2>().x;
        yThrow = movement.ReadValue<Vector2>().y;

        var localPosition = transform.localPosition;

        var xOffset = xThrow * Time.deltaTime * controlSpeed;
        var rawXPos = xOffset + localPosition.x;
        var clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        var yOffset = yThrow * Time.deltaTime * controlSpeed;
        var rawYPos = yOffset + localPosition.y;
        var clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, localPosition.z);
    }
    
    private void ProcessRotation()
    {
        var pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        var pitchDueToRotation = yThrow * controlPitchFactor;
        
        var pitch = pitchDueToPosition + pitchDueToRotation;

        var yaw = transform.localPosition.x * positionYawFactor;
        var roll = xThrow * controlRollFactor;
        
        var newRotation = Quaternion.Euler(pitch, yaw, roll);
        
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, newRotation, rotationFactor);
    }

    private void ProcessFiring()
    {
        if (fire.ReadValue<float>() > 0.5f)
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }

    private void SetLasersActive(bool isActive)
    {
        foreach (var laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
