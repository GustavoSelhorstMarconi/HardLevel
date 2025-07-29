using System;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField]
    private GameObject mobileUI;

    private void HandlePlatform()
    {
#if UNITY_ANDROID
        mobileUI.SetActive(true);
#else
        mobileUI.SetActive(false);
#endif
    }
}
