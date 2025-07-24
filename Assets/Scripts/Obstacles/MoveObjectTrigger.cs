using System;
using UnityEngine;

public class MoveObjectTrigger : MonoBehaviour
{
    [SerializeField]
    private MoveObject moveObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerControl>(out _))
        {
            moveObject.HandleEnableMovement(other.transform);
        }
    }
}
