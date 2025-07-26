using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelControl : MonoBehaviour
{
    public static LevelControl Instance { get;  private set; }

    public event EventHandler OnLevelChange;

    [SerializeField]
    private LevelsAvailable levelsAvailable;

    private GameObject currentLevel;
    public int currentLevelIndex = 1;
    private bool isLoading = false;
    private int maxLevel => levelsAvailable.levels.Count - 1;
    
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
        LoadLevel(currentLevelIndex);
        OnLevelChange?.Invoke(this, EventArgs.Empty);
        DataSaveControl.Instance.Save(DataSaveControl.DEATH_COUNT_KEY_NAME, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
            RestartLevel();
    }

    public void LoadNextLevel()
    {
        currentLevelIndex = currentLevelIndex + 1 > maxLevel ? maxLevel : currentLevelIndex + 1;
        
        LoadLevel(currentLevelIndex);
        OnLevelChange?.Invoke(this, EventArgs.Empty);
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
    }
    
    public void LoadLevelByNumber(int levelNumber)
    {
        currentLevelIndex = levelNumber;
        
        LoadLevel(currentLevelIndex);
        OnLevelChange?.Invoke(this, EventArgs.Empty); 
    }

    private void LoadLevel(int levelIndex)
    {
        if (isLoading)
            return;
        
        if (levelIndex < 0 || levelIndex >= levelsAvailable.levels.Count)
            return;
    
        isLoading = true;
    
        if (currentLevel != null)
        {
            Addressables.ReleaseInstance(currentLevel);
            Destroy(currentLevel);
        }
    
        AssetReferenceGameObject levelReference = levelsAvailable.levels[levelIndex].levelPrefab;
        Addressables.InstantiateAsync(levelReference).Completed += (obj) => {
            isLoading = false;
            OnLevelLoaded(obj);
        };
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

    public string GetLevelNameUI()
    {
        return levelsAvailable.levels[currentLevelIndex].levelName;
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    public Transform GetCurrentLevelTransform()
    {
        if (currentLevel != null)
            return currentLevel.transform;
        
        return null;
    }
}
