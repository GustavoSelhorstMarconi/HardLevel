using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelMessageUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI pcLevelMessageText;
    [SerializeField]
    private TextMeshProUGUI mobileLevelMessageText;
    [SerializeField]
    private List<string> messages;
    
    private Dictionary<int, string> levelMessages;
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
        
        levelMessages = new Dictionary<int, string>();

        for (int i = 0; i < messages.Count; i++)
        {
            levelMessages.Add(i, messages[i]);
        }
    }

    private void LevelControlOnLevelChange(object sender, EventArgs e)
    {
        int levelIndex = LevelControl.Instance.GetCurrentLevelIndex();
        
        levelMessageText.text = levelMessages[levelIndex];
    }
}
