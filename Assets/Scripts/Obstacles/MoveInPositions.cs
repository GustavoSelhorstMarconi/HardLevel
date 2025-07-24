using System;
using UnityEngine;

public class MoveInPositions : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private bool moveOnStart;
    [SerializeField]
    private bool afterPositionMoveBackToStart;
    [SerializeField]
    private Transform[] positions;

    private float currentMovement;
    private bool isMoving;
    private int currentPositionIndex = 0;
    private bool isBackwards = false;

    private void Start()
    {
        isMoving = moveOnStart;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!isMoving)
            return;
        
        currentMovement = Mathf.MoveTowards(currentMovement, 1f, movementSpeed * Time.deltaTime);
        
        Vector2 positionToMove = Vector2.Lerp(positions[currentPositionIndex].localPosition, GetNextPosition(), currentMovement);

        if (currentMovement >= 1f)
        {
            if (!afterPositionMoveBackToStart && currentPositionIndex + 1 >= positions.Length)
                isBackwards = true;
            else if (isBackwards && currentPositionIndex - 1 < 0)
                isBackwards = false;
                
            currentPositionIndex = GetNextPositionIndex();
            currentMovement = 0f;
        }
        
        transform.localPosition = positionToMove;
    }

    private Vector2 GetNextPosition()
    {
        int nextIndexPosition = GetNextPositionIndex();
        
        return positions[nextIndexPosition].localPosition;
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
        if (isMoving)
            return;
        
        isMoving = true;
    }
}
