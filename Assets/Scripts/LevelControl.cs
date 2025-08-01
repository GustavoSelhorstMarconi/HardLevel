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
        LevelTransitionUIControl.Instance.StartFadeIn(() =>
        {
            LoadLevelAsync(levelIndex);
        });
    }

    private async void LoadLevelAsync(int levelIndex)
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

        Time.timeScale = 0f;
        AssetReferenceGameObject levelReference = levelsAvailable.levels[levelIndex].levelPrefab;
        
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(levelReference);
        await handle.Task;
        
        isLoading = false;
        Time.timeScale = 1f;
        
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            currentLevel = handle.Result;
            LevelTransitionUIControl.Instance.StartFadeOut();
            Debug.Log("Level loaded");
        }
        else
        {
            Debug.Log("Erro");
        }
        
        // Addressables.InstantiateAsync(levelReference).Completed += (obj) => {
        //     isLoading = false;
        //     
        //     OnLevelLoaded(obj);
        // };
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
        string levelString = LocalizationControl.Instance.GetLocalizedText(LocalizationControl.LOCALIZATION_TABLE_NAME, LocalizationControl.LEVEL_KEY_NAME);
        
        return levelString + " " + (currentLevelIndex + 1);
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

    public int GetLevelAmount()
    {
        return levelsAvailable.levels.Count;
    }
}
