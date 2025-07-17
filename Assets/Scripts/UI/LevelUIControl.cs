using System;
using TMPro;
using UnityEngine;

public class LevelUIControl : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;
    
    private void Start()
    {
        LevelControl.Instance.OnLevelChange += LevelControlOnLevelChange;
    }

    private void LevelControlOnLevelChange(object sender, EventArgs e)
    {
        levelText.text = LevelControl.Instance.GetLevelNameUI();
    }
}