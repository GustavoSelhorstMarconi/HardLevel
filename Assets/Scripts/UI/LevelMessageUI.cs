using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelMessageUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelMessageText;
    [SerializeField]
    private List<string> messages;
    
    private Dictionary<int, string> levelMessages;
    
    private void Start()
    {
        LevelControl.Instance.OnLevelChange += LevelControlOnLevelChange;
        
        levelMessages = new Dictionary<int, string>();

        for (int i = 0; i < messages.Count; i++)
        {
            levelMessages.Add(i + 1, messages[i]);
        }
    }

    private void LevelControlOnLevelChange(object sender, EventArgs e)
    {
        int levelIndex = LevelControl.Instance.GetCurrentLevelIndex();
        
        levelMessageText.text = levelMessages[levelIndex];
    }
}
