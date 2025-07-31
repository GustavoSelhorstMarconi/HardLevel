using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms;

public class LevelMessageUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI pcLevelMessageText;
    [SerializeField]
    private TextMeshProUGUI mobileLevelMessageText;
    
    private TextMeshProUGUI levelMessageText;

    private void Awake()
    {
#if UNITY_ANDROID
        levelMessageText = mobileLevelMessageText;
        pcLevelMessageText.enabled = false;
#else
        levelMessageText = pcLevelMessageText;
        mobileLevelMessageText.enabled = false;
#endif
    }

    private void Start()
    {
        LevelControl.Instance.OnLevelChange += LevelControlOnLevelChange;

        ShowMessageForLevel();
    }

    private void LevelControlOnLevelChange(object sender, EventArgs e)
    {
        ShowMessageForLevel();
    }

    private void ShowMessageForLevel()
    {
        int levelIndex = LevelControl.Instance.GetCurrentLevelIndex();
        
        string key = LocalizationControl.LEVEL_MESSAGE_BASE_KEY_NAME + levelIndex;

        string message = LocalizationControl.Instance.GetLocalizedText(LocalizationControl.MESSAGES_TABLE_NAME,
            key);
        
        levelMessageText.text = message;

        LocalizationControl.Instance.SetLocalizedText(levelMessageText, key,
            true, null, true);
    }
}
