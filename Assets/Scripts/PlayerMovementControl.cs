using System;
using UnityEngine;

public class PlayerMovementControl : MonoBehaviour
{
    public static PlayerMovementControl Instance { get; set; }
    
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
    [SerializeField]
    private float skinWidth;

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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        
        Instance = this;
    }

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
        Vector2 boxSize = new Vector2(
            playerCollider.bounds.size.x - 2 * skinWidth,
            0.1f
        );

        Vector2 origin = (Vector2)playerCollider.bounds.center
                         + (isGravityReversed ? Vector2.up : Vector2.down)
                         * (playerCollider.bounds.extents.y - boxSize.y / 2 + skinWidth);

        Vector2 direction = isGravityReversed ? Vector2.up : Vector2.down;

        RaycastHit2D hit = Physics2D.BoxCast(
            origin,
            boxSize,
            0f,
            direction,
            distanceCanJump,
            groundLayer
        );

        canJump = hit.collider != null;
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
        float width = playerCollider.bounds.size.x - 2 * skinWidth;
        float height = 0.1f;
        Vector2 boxSize = new Vector2(width, height);

        Vector2 origin = (Vector2)playerCollider.bounds.center
                         + (isGravityReversed ? Vector2.up : Vector2.down)
                         * (playerCollider.bounds.extents.y - height / 2 + skinWidth);

        // Desenha a caixa usada no BoxCast
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(origin, boxSize);
    }

    public void HandleReverseMovement(bool reverseMovement)
    {
        isReversedMovement = reverseMovement;
    }
}
