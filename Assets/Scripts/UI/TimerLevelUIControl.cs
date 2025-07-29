using System;
using TMPro;
using UnityEngine;

public class TimerLevelUIControl : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    private float currentLevelTime = 0f;

    private void Start()
    {
        LevelControl.Instance.OnLevelChange += LevelControlOnOnLevelChange;
    }

    private void LevelControlOnOnLevelChange(object sender, EventArgs e)
    {
        currentLevelTime = 0f;
    }

    private void Update()
    {
        HandleTimeUpdate();
    }

    private void HandleTimeUpdate()
    {
        currentLevelTime += Time.deltaTime;
        
        int minutes = Mathf.FloorToInt(currentLevelTime / 60f);
        int seconds = Mathf.FloorToInt(currentLevelTime % 60f);
        int milliseconds = Mathf.FloorToInt((currentLevelTime * 100f) % 100f);
        
        timerText.text = string.Format("{0}:{1:00}:{2:00}", 
            minutes, seconds, milliseconds);
    }
}
