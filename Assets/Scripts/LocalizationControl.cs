using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationControl : MonoBehaviour
{
    public static LocalizationControl Instance { get; set; }
    
    public const string LOCALIZATION_TABLE_NAME = "LocalizationTable";
    public const string MESSAGES_TABLE_NAME = "MessagesTable";
    public const string LEVEL_MESSAGE_BASE_KEY_NAME = "LevelMessage_";
    public const string CONTINUE_KEY_NAME = "ContinueKey";
    public const string LEVELS_KEY_NAME = "LevelsKey";
    public const string LEVEL_KEY_NAME = "LevelKey";
    public const string QUIT_KEY_NAME = "QuitKey";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
    
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        await LocalizationSettings.InitializationOperation.Task;
        SetLocale(DataSaveControl.Instance.Load(DataSaveControl.LOCALE_KEY_NAME, 0));
    }

    public void SetLocale(int localeIndex)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];
    }

    public string GetLocalizedText(string table, string key)
    {
        LocalizedString localizedString = new LocalizedString(table, key);
        
        return localizedString.GetLocalizedString();
    }
    
    public void SetLocalizedText(TMP_Text textComponent, string tableKey, bool setTextValue, Action<string> callBack = null, bool isMessage = false)
    {
        string tableName = isMessage ? MESSAGES_TABLE_NAME : LOCALIZATION_TABLE_NAME;
        LocalizedString localizedString = new LocalizedString(tableName,  tableKey);

        localizedString.StringChanged += (localizedValue) =>
        {
            if (setTextValue)
                textComponent.text = localizedValue;
            
            callBack?.Invoke(localizedValue);
        };
    }
}
