using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseOnTouch : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerControl>(out PlayerControl playerControl))
        {
            playerControl.HandleDestroyPlayer();

            if (LevelControl.Instance != null)
                LevelControl.Instance.RestartLevel();
            else
                SceneManager.LoadScene("TestScene");
        }
    }
}
