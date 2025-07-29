using System;
using UnityEngine;

public class MultiPlatformSingleControl : MonoBehaviour
{
    [SerializeField]
    private Platform platform;
    
    private enum Platform
    {
        pc,
        mobile
    }

    private void Awake()
    {
#if UNITY_ANDROID
        if (platform == Platform.pc)
            gameObject.SetActive(false);
#else
        if (platform == Platform.mobile)
            gameObject.SetActive(false);
#endif
    }
}
