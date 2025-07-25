using System;
using UnityEngine;

public class ReversePlayerMovementByTime : MonoBehaviour
{
    [SerializeField]
    private bool useRandomTime;
    [SerializeField]
    private float minTimeReverseMovement;
    [SerializeField]
    private float maxTimeReverseMovement;
    [SerializeField]
    private Color cameraBackgroundColorEffect;
    [SerializeField]
    private float timeEffectDuration;

    private float currentTimerReverseMovement;
    private bool reversePlayerMovement = true;
    
    private Color baseBackgroundCameraColor;
    private float timerCameraEffect;
    private bool isCameraInEffect = false;
    private Camera mainCamera;

    private void Start()
    {
        currentTimerReverseMovement = UnityEngine.Random.Range(minTimeReverseMovement, maxTimeReverseMovement);
        mainCamera = Camera.main;
        baseBackgroundCameraColor = mainCamera.backgroundColor;
    }

    private void Update()
    {
        HandleTimeReverseMovement();
        HandleTimerEffect();
    }

    private void HandleTimerEffect()
    {
        if (!isCameraInEffect)
            return;
        
        timerCameraEffect -= Time.deltaTime;

        if (timerCameraEffect <= 0)
        {
            mainCamera.backgroundColor = baseBackgroundCameraColor;
            isCameraInEffect = false;
        }
    }

    private void HandleTimeReverseMovement()
    {
        currentTimerReverseMovement -= Time.deltaTime;

        if (currentTimerReverseMovement <= 0)
        {
            PlayerMovementControl.Instance.HandleReverseMovement(reversePlayerMovement);
            reversePlayerMovement = !reversePlayerMovement;
            HandleNextTimeReverseMovement();
            HandleReverseMovementEffect();
        }
    }

    private void HandleReverseMovementEffect()
    {
        mainCamera.backgroundColor = cameraBackgroundColorEffect;
        
        timerCameraEffect = timeEffectDuration;
        isCameraInEffect = true;
    }

    private void HandleNextTimeReverseMovement()
    {
        if (useRandomTime)
            currentTimerReverseMovement = UnityEngine.Random.Range(minTimeReverseMovement, maxTimeReverseMovement);
        else
            currentTimerReverseMovement = minTimeReverseMovement;
    }
}
