using System;
using UnityEngine;

public class DataSaveControl : MonoBehaviour
{
    public const string DEATH_COUNT_KEY_NAME = "DeathCount";
    public const string LEVEL_UNLOCKED_KEY_NAME = "LevelUnlocked";
    public const string LOCALE_KEY_NAME = "LocaleId";
    
    public static DataSaveControl Instance { get; set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Save<T>(string key, T value)
    {
        switch (Type.GetTypeCode(typeof(T)))
        {
            case TypeCode.Int32:
                PlayerPrefs.SetInt(key, (int)(object)value);
                break;
            case TypeCode.Single:
                PlayerPrefs.SetFloat(key, (float)(object)value);
                break;
            case TypeCode.String:
                PlayerPrefs.SetString(key, (string)(object)value);
                break;
            default:
                string json = JsonUtility.ToJson(value);
                PlayerPrefs.SetString(key, json);
                break;
        }
        
        PlayerPrefs.Save();
    }

    public T Load<T>(string key, T defaultValue = default)
    {
        switch (Type.GetTypeCode(typeof(T)))
        {
            case TypeCode.Int32:
                return (T)(object)PlayerPrefs.GetInt(key, (int)(object)defaultValue);

            case TypeCode.Single:
                return (T)(object)PlayerPrefs.GetFloat(key, (float)(object)defaultValue);

            case TypeCode.String:
                return (T)(object)PlayerPrefs.GetString(key, (string)(object)defaultValue);

            default:
                string json = PlayerPrefs.GetString(key, "");
                if (string.IsNullOrEmpty(json)) return defaultValue;
                return JsonUtility.FromJson<T>(json);
        }
    }
}
