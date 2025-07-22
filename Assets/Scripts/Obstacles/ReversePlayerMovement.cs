using System;
using UnityEngine;

public class ReversePlayerMovement : MonoBehaviour
{
    [SerializeField]
    private bool reversePlayerMovement;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerMovementControl>(out PlayerMovementControl playerMovementControl))
        {
            playerMovementControl.HandleReverseMovement(reversePlayerMovement);
        }
    }
}
