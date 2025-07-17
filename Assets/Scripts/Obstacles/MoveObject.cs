using System;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField]
    private Vector2 directionMovement;
    [SerializeField]
    private float speedMovement;
    [SerializeField]
    private bool moveOnStart;
    [SerializeField]
    private bool followPlayerWhenTrigger;

    private bool canMove;
    private Transform playerTransform;
    private float distancePlayer;

    private void Start()
    {
        canMove = moveOnStart;
    }

    private void Update()
    {
        if (!canMove)
            return;

        ApplyMovement();
    }

    private void ApplyMovement()
    {
        if (playerTransform != null)
        {
            transform.position = new Vector2(playerTransform.position.x - distancePlayer, transform.position.y);   
        }
        else
        {
            Vector2 movement = directionMovement * Time.deltaTime * speedMovement;
            transform.Translate(movement);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canMove && !followPlayerWhenTrigger )
            return;

        if (other.TryGetComponent<PlayerControl>(out _))
        {
            canMove = true;

            if (followPlayerWhenTrigger)
            {
                playerTransform = other.transform;
                distancePlayer = playerTransform.position.x - transform.position.x;
            }
        }
    }
}
