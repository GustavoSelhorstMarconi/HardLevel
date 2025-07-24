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
    [SerializeField]
    private bool destroy;
    [SerializeField]
    private float delayDestroy;
    [SerializeField]
    private bool otherObjectTrigger;

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
        if (other.TryGetComponent<PlayerControl>(out _))
        {
            HandleEnableMovement(other.transform);
        }
    }

    public void HandleEnableMovement(Transform playerCollided)
    {
        if (canMove && !followPlayerWhenTrigger)
            return;
        
        canMove = true;

        if (followPlayerWhenTrigger)
        {
            playerTransform = playerCollided;
            distancePlayer = playerTransform.position.x - transform.position.x;
        }
            
        if (destroy)
            Destroy(gameObject, delayDestroy);
    }
}
