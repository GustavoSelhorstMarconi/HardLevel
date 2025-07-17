using System;
using UnityEngine;

public class LevelEndControl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerMovementControl>(out _))
        {
            LevelControl.Instance.LoadNextLevel();
        }
    }
}
