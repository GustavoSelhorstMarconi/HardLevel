using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChangeLevelColor : MonoBehaviour
{
    [Header("Generic configuration")]
    [SerializeField]
    private ActionType actionType;
    [SerializeField]
    private Color levelColor;
    [SerializeField]
    private float durationChangeColor;
    [SerializeField]
    private bool willBackBaseColor;
    [Header("By time configuration")]
    [SerializeField]
    private float minTimeChangeColor;
    [SerializeField]
    private float maxTimeChangeColor;
    
    private enum ActionType
    {
        byTime,
        byTimeRandom,
        byCollision
    }

    private Camera mainCamera;
    private Color baseLevelColor;
    private float currentTimeChangeColor;
    private float timerDurationChangeColor;
    private bool isChangingColor;
    private bool isAwaitingChangeColor;

    private void Start()
    {
        mainCamera = Camera.main;
        
        if (mainCamera != null)
            baseLevelColor = mainCamera.backgroundColor;

        if (actionType != ActionType.byCollision)
            GenerateNewTimeChangeColor();
    }

    private void Update()
    {
        HandleChangeColorByTimer();
        HandleChangeColor();
    }

    private void HandleChangeColorByTimer()
    {
        if (actionType == ActionType.byCollision || !isAwaitingChangeColor)
            return;
        
        currentTimeChangeColor -= Time.deltaTime;
        
        if (currentTimeChangeColor <= 0f)
            StartChangeColor();
    }

    private void HandleChangeColor()
    {
        if (!isChangingColor || !willBackBaseColor)
            return;
        
        timerDurationChangeColor -= Time.deltaTime;

        if (timerDurationChangeColor <= 0f)
            EndChangeColor();
    }

    private void StartChangeColor()
    {
        isAwaitingChangeColor = false;
        isChangingColor = true;
        timerDurationChangeColor = durationChangeColor;
        mainCamera.backgroundColor = levelColor;
    }

    private void EndChangeColor()
    {
        isChangingColor = false;
        mainCamera.backgroundColor = baseLevelColor;

        if (actionType != ActionType.byCollision)
            GenerateNewTimeChangeColor();
    }

    private void GenerateNewTimeChangeColor()
    {
        isAwaitingChangeColor = true;
        currentTimeChangeColor = actionType == ActionType.byTime ? minTimeChangeColor : Random.Range(minTimeChangeColor, maxTimeChangeColor);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (actionType != ActionType.byCollision)
            return;

        if (other.TryGetComponent<PlayerControl>(out _))
            StartChangeColor();
    }
}
