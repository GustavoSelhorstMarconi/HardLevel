using System;
using UnityEngine;

public class TriggerStartMovePlatform : MonoBehaviour
{
    [SerializeField]
    private MoveInPositions moveInPositions;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerControl>(out _))
            moveInPositions.StartMoveByTrigger();
    }
}
