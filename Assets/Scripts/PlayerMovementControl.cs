using System;
using UnityEngine;

public class PlayerMovementControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private BoxCollider2D playerCollider;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [Header("Movement values")]
    [SerializeField]
    private float horizontalSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float distanceCanJump;
    [SerializeField]
    private float gravityForce;
    [SerializeField]
    private float maxSpeed;
    [Header("Configuration")]
    [SerializeField]
    private bool canReverseGravity;
    [SerializeField]
    private float reverseGravityAirMultiplier;
    [SerializeField]
    private float raycastMultiplier;
    [SerializeField]
    private bool startReversedMovement;

    private bool canJump = true;
    private Vector2 movementInput;
    private bool isGravityReversed = false;
    private float halfWidth;
    private bool isReversedMovement;
    private int jumpGravityMultiplier => isGravityReversed ? -1 : 1;
    private int movementReversedMultiplier => isReversedMovement ?  -1 : 1;

    private enum LastAirButtonPressed
    {
        jump,
        changeGravity
    }
    
    private LastAirButtonPressed lastAirButtonPressed;

    private void Start()
    {
        halfWidth = playerCollider.bounds.extents.x * raycastMultiplier;
        isReversedMovement = startReversedMovement;
    }

    private void Update()
    {
        VerifyJump();
        
        HandleMovementInput();
        HandleJumpInput();
        HandleReverseGravityInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        
        if (!canJump)
            ApplyGravityForce();
    }

    private void ApplyGravityForce()
    {
        Vector2 gravityForceApply = new Vector2(0f, -gravityForce * Time.deltaTime * jumpGravityMultiplier);
        
        if (canReverseGravity && lastAirButtonPressed == LastAirButtonPressed.changeGravity)
            gravityForceApply *= reverseGravityAirMultiplier;
        
        rigidBody.AddForce(gravityForceApply);
    }

    private void HandleReverseGravityInput()
    {
        if (!canReverseGravity)
            return;

        if (GameInputControl.Instance.GetPlayerReverseGravity() && canJump)
        {
            lastAirButtonPressed = LastAirButtonPressed.changeGravity;
            isGravityReversed = !isGravityReversed;
            
            rigidBody.gravityScale = jumpGravityMultiplier;
        }
    }

    private void VerifyJump()
    {
        Vector2 origin = transform.position;
        Vector2 leftOrigin = origin + Vector2.left * halfWidth;
        Vector2 rightOrigin = origin + Vector2.right * halfWidth;
        
        RaycastHit2D centerHit = Physics2D.Raycast(origin, isGravityReversed ? Vector2.up : Vector2.down, distanceCanJump, groundLayer);
        RaycastHit2D leftHit = Physics2D.Raycast(leftOrigin, isGravityReversed ? Vector2.up : Vector2.down, distanceCanJump, groundLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rightOrigin, isGravityReversed ? Vector2.up : Vector2.down, distanceCanJump, groundLayer);
        
        canJump = centerHit.collider != null || leftHit.collider != null || rightHit.collider != null;
    }

    private void HandleMovementInput()
    {
        movementInput = GameInputControl.Instance.GetPlayerMovement();
    }

    private void HandleMovement()
    {
        Vector2 movementValue = movementInput * horizontalSpeed * Time.deltaTime * movementReversedMultiplier;
        movementValue.y = 0f;
        
        rigidBody.AddForce(movementValue);
        
        if (Mathf.Abs(rigidBody.linearVelocity.x) > maxSpeed)
            rigidBody.linearVelocity = new Vector2(Mathf.Sign(rigidBody.linearVelocity.x) * maxSpeed, rigidBody.linearVelocity.y);
    }

    private void HandleJumpInput()
    {
        if (!canJump)
            return;

        if (GameInputControl.Instance.GetPlayerJump())
        {
            lastAirButtonPressed = LastAirButtonPressed.jump;
            Vector2 jumpValue = new Vector2(0f, jumpForce * jumpGravityMultiplier);

            rigidBody.AddForce(jumpValue, ForceMode2D.Impulse);
            canJump = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        Vector2 origin = transform.position;
        Vector2 leftOrigin = origin + Vector2.left * halfWidth;
        Vector2 rightOrigin = origin + Vector2.right * halfWidth;
        
        Vector2 direction = isGravityReversed ? Vector2.up * distanceCanJump : Vector2.down * distanceCanJump;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + direction);
        Gizmos.DrawLine(leftOrigin, leftOrigin + direction);
        Gizmos.DrawLine(rightOrigin, rightOrigin + direction);
    }

    public void HandleReverseMovement(bool reverseMovement)
    {
        isReversedMovement = reverseMovement;
    }
}
