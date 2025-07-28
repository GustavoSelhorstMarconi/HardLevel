using System;
using UnityEngine;

public class MoveInPositions : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private bool afterPositionMoveBackToStart;
    [SerializeField]
    private Transform[] positions;
    [SerializeField]
    private MoveType moveType;
    [SerializeField]
    private float timeStartMove;
    [SerializeField]
    private Rigidbody2D objectRigidbody;

    private float currentMovement;
    private bool isMoving;
    private int currentPositionIndex = 0;
    private bool isBackwards = false;
    private float timerStartMove;

    private enum MoveType
    {
        onStart,
        onTrigger,
        afterTime,
    }

    private void Start()
    {
        isMoving = moveType == MoveType.onStart;
        timerStartMove = timeStartMove;
    }

    private void Update()
    {
        HandleMoveByTime();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    
    private void HandleMoveByTime()
    {
        if (moveType != MoveType.afterTime)
            return;
        
        timerStartMove -= Time.deltaTime;
        
        if (timerStartMove <= 0)
            isMoving = true;
    }

    private void HandleMovement()
    {
        if (!isMoving)
            return;
        
        currentMovement = Mathf.MoveTowards(currentMovement, 1f, movementSpeed * Time.deltaTime);
        
        Vector2 positionToMove = Vector2.Lerp(positions[currentPositionIndex].position, GetNextPosition(), currentMovement);

        if (currentMovement >= 1f)
        {
            if (!afterPositionMoveBackToStart && currentPositionIndex + 1 >= positions.Length)
                isBackwards = true;
            else if (isBackwards && currentPositionIndex - 1 < 0)
                isBackwards = false;
                
            currentPositionIndex = GetNextPositionIndex();
            currentMovement = 0f;
        }
        
        objectRigidbody.MovePosition(positionToMove);
    }

    private Vector2 GetNextPosition()
    {
        int nextIndexPosition = GetNextPositionIndex();
        
        return positions[nextIndexPosition].position;
    }

    private int GetNextPositionIndex()
    {
        int nextIndexPosition = 0;
        
        if (isBackwards)
            nextIndexPosition = currentPositionIndex - 1 < 0 ? currentPositionIndex + 1 : currentPositionIndex - 1;
        else
            nextIndexPosition = currentPositionIndex + 1 >= positions.Length ? afterPositionMoveBackToStart ? 0 : currentPositionIndex - 1 : currentPositionIndex + 1;

        return nextIndexPosition;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerControl>(out _))
            other.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerControl>(out _))
            other.transform.SetParent(LevelControl.Instance.GetCurrentLevelTransform());
    }

    public void StartMoveByTrigger()
    {
        if (isMoving || moveType != MoveType.onTrigger)
            return;
        
        isMoving = true;
    }
}
