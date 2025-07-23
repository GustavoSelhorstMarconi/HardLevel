using System;
using System.Collections;
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
    private bool isLoading = false;
    private AsyncOperationHandle<GameObject> pendingLoadOperation;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentLevelName = GetLevelName();
        LoadLevel(currentLevelName);
        OnLevelChange?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
            RestartLevel();
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
        if (isLoading) return;
        
        StartCoroutine(LoadLevelCoroutine(levelName));
    }

    private IEnumerator LoadLevelCoroutine(string levelName)
    {
        isLoading = true;
        
        if (currentLevel != null)
        {
            Addressables.ReleaseInstance(currentLevel);
            
            Destroy(currentLevel);
            yield return null;
        }

        if (pendingLoadOperation.IsValid())
        {
            Addressables.Release(pendingLoadOperation);
        }

        pendingLoadOperation = Addressables.InstantiateAsync(levelName);
        yield return pendingLoadOperation;

        if (pendingLoadOperation.Status == AsyncOperationStatus.Succeeded)
        {
            currentLevel = pendingLoadOperation.Result;
            Debug.Log($"Level loaded: {levelName}");
        }
        else
        {
            Debug.LogError($"Error loading level: {levelName}");
        }

        isLoading = false;
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
