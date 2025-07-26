using System;
using TMPro;
using UnityEngine;

public class TimerUIControl : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    private float currentTime = 0f;

    private void Update()
    {
        HandleTimeUpdate();
    }

    private void HandleTimeUpdate()
    {
        currentTime += Time.deltaTime;
        
        timerText.text = currentTime.ToString("F2");
    }
}
