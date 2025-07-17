using System;
using UnityEngine;

public class PlayerMovementControl : MonoBehaviour
{
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
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private bool canReverseGravity;

    private bool canJump = true;
    private Vector2 movementInput;
    private bool isGravityReversed = false;
    private int jumpGravityMultiplier => isGravityReversed ? -1 : 1;

    private void Update()
    {
        VerifyJump();
        
        HandleMovementInput();
        HandleJumpInput();
        HandleReverseGravityInput();

        if (!canJump)
            ApplyGravityForce();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void ApplyGravityForce()
    {
        Vector2 gravityForceApply = new Vector2(0f, -gravityForce * Time.deltaTime * jumpGravityMultiplier);
        
        rigidBody.AddForce(gravityForceApply);
    }

    private void HandleReverseGravityInput()
    {
        if (!canReverseGravity)
            return;

        if (GameInputControl.Instance.GetPlayerReverseGravity() && canJump)
        {
            isGravityReversed = !isGravityReversed;
            
            rigidBody.gravityScale = jumpGravityMultiplier;
        }
    }

    private void VerifyJump()
    {
        canJump = Physics2D.Raycast(transform.position, isGravityReversed ? Vector2.up : Vector2.down, distanceCanJump, groundLayer);
    }

    private void HandleMovementInput()
    {
        movementInput = GameInputControl.Instance.GetPlayerMovement();
    }

    private void HandleMovement()
    {
        Vector2 movementValue = movementInput * horizontalSpeed * Time.deltaTime;
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
            Vector2 jumpValue = new Vector2(0f, jumpForce * jumpGravityMultiplier);

            rigidBody.AddForce(jumpValue, ForceMode2D.Impulse);
            canJump = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        // Cor da linha para visualização
        Gizmos.color = Color.green;

        // Ponto inicial do raycast
        Vector3 start = transform.position;

        // Direção e distância
        Vector3 direction = isGravityReversed ? Vector2.up * distanceCanJump : Vector2.down * distanceCanJump;

        // Desenha a linha representando o Raycast
        Gizmos.DrawLine(start, start + direction);
    }
}
