using System;
using UnityEngine;

public class LoseOnTouch : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerControl>(out PlayerControl playerControl))
        {
            playerControl.HandleDestroyPlayer();
            LevelControl.Instance.RestartLevel();
        }
    }
}
