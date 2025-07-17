using System;
using UnityEngine;

public class GameInputControl : MonoBehaviour
{
    public static GameInputControl Instance { get; private set; }

    private GameInput gameInputActions;

    private void Awake()
    {
        Instance = this;

        gameInputActions = new GameInput();

        gameInputActions.Enable();
    }

    public Vector2 GetPlayerMovement()
    {
        return gameInputActions.Player.Movement.ReadValue<Vector2>();
    }
    
    public bool GetPlayerJump()
    {
        return gameInputActions.Player.Jump.triggered;;
    }
    
    public bool GetPlayerReverseGravity()
    {
        return gameInputActions.Player.ReverseGravity.triggered;;
    }
    
    public bool GetPlayerPaused()
    {
        return gameInputActions.Player.Pause.triggered;;
    }

    private void OnDisable()
    {
        gameInputActions.Disable();
    }
}
