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
        
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        int milliseconds = Mathf.FloorToInt((currentTime * 100f) % 100f);
        
        timerText.text = string.Format("{0}:{1:00}:{2:00}", 
            minutes, seconds, milliseconds);
    }
}
