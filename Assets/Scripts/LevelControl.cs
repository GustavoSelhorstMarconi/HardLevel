using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelControl : MonoBehaviour
{
    public static LevelControl Instance { get;  private set; }

    public event EventHandler OnLevelChange;

    [SerializeField]
    private string defaultLevelName;
    [SerializeField]
    private int maxLevel;

    private GameObject currentLevel;
    private string currentLevelName;
    public int currentLevelIndex = 1;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        currentLevelName = GetLevelName();
        LoadLevel(currentLevelName);
        OnLevelChange?.Invoke(this, EventArgs.Empty);
    }

    public void LoadNextLevel()
    {
        currentLevelIndex = currentLevelIndex + 1 > maxLevel ? maxLevel : currentLevelIndex + 1;
        currentLevelName = GetLevelName();
        
        LoadLevel(currentLevelName);
        OnLevelChange?.Invoke(this, EventArgs.Empty);
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelName);
    }
    
    public void LoadLevelByNumber(int levelNumber)
    {
        currentLevelIndex = levelNumber;
        currentLevelName = GetLevelName();
        
        LoadLevel(currentLevelName);
        OnLevelChange?.Invoke(this, EventArgs.Empty);
    }

    private void LoadLevel(string levelName)
    {
        if (currentLevel != null)
        {
            Addressables.ReleaseInstance(currentLevel);
            Destroy(currentLevel);
        }
        
        Addressables.InstantiateAsync(levelName).Completed += OnLevelLoaded;
    }

    private void OnLevelLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            currentLevel = obj.Result;
            Debug.Log("Level loaded");
        }
        else
        {
            Debug.Log("Erro");
        }
    }

    private void OnDestroy()
    {
        if (currentLevel != null)
        {
            Addressables.ReleaseInstance(currentLevel);
        }
    }

    private string GetLevelName()
    {
        return defaultLevelName + currentLevelIndex.ToString("D2");
    }

    public string GetLevelNameUI()
    {
        return defaultLevelName + " " + currentLevelIndex.ToString();
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }
}
