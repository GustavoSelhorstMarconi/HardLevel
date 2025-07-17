using System;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    [SerializeField]
    private Vector2 movePosition;
    [SerializeField]
    private float speedMovement;

    private Vector2 startPosition;
    private float currentMovement = 0f;
    private bool canMove = false;

    private void Awake()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!canMove)
            return;
        
        currentMovement = Mathf.MoveTowards(currentMovement, 1f, speedMovement * Time.deltaTime);
        
        transform.localPosition = Vector2.Lerp(startPosition, movePosition, currentMovement);
        
        if (currentMovement >= 1f)
            canMove = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canMove)
            return;
        
        if (other.TryGetComponent<PlayerControl>(out _))
            canMove = true;
    }
}
