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
        
        LocalizationControl.Instance.SetLocalizedText(levelText, LocalizationControl.LEVEL_KEY_NAME, false, (value) =>
        {
            levelText.text = LevelControl.Instance.GetLevelNameUI();
        });
    }

    private void LevelControlOnLevelChange(object sender, EventArgs e)
    {
        levelText.text = LevelControl.Instance.GetLevelNameUI();
    }
}