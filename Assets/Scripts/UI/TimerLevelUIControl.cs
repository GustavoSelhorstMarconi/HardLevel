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
        
        timerText.text = currentLevelTime.ToString("F2");
    }
}
