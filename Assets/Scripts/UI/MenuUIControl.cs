using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class MenuUIControl : MonoBehaviour
{
    public static MenuUIControl Instance { get; private set; }

    [SerializeField]
    private GameObject menuContainer;

    [SerializeField]
    private GameObject levelsContainer;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private TMP_Text continueText;

    [SerializeField]
    private Button levelsButton;

    [SerializeField]
    private TMP_Text levelsText;

    [SerializeField]
    private Button quitButton;

    [SerializeField]
    private TMP_Text quitText;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }

    private void Start()
    {
        Disable();
        Cursor.lockState = CursorLockMode.Locked;

        HandleButtonsListeners();
        HandleButtonsTextLocalization();
    }

    private void HandleButtonsListeners()
    {
        backButton.onClick.AddListener(ToggleBackButton);
        continueButton.onClick.AddListener(ToggleMenu);
        levelsButton.onClick.AddListener(HandleLevelSelection);
        quitButton.onClick.AddListener(Quit);
    }

    private void HandleButtonsTextLocalization()
    {
        LocalizationControl.Instance.SetLocalizedText(continueText, LocalizationControl.CONTINUE_KEY_NAME, true);
        LocalizationControl.Instance.SetLocalizedText(levelsText, LocalizationControl.LEVELS_KEY_NAME, true);
        LocalizationControl.Instance.SetLocalizedText(quitText, LocalizationControl.QUIT_KEY_NAME, true);
    }

    private void Update()
    {
        if (GameInputControl.Instance.GetPlayerPaused())
        {
            ToggleMenu();
        }
    }

    private void ToggleBackButton()
    {
        if (menuContainer.activeSelf)
        {
            ToggleMenu();
        }
        else
        {
            menuContainer.SetActive(true);
            levelsContainer.SetActive(false);
        }
    }

    private void ToggleMenu()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            Enable();
        }
        else
            CloseMenu();
    }

    public void CloseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        Disable();
        levelsContainer.SetActive(false);
    }

    private void Enable()
    {
        menuContainer.SetActive(true);
        backButton.gameObject.SetActive(true);
    }
    
    private void Disable()
    {
        menuContainer.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    private void HandleLevelSelection()
    {
        menuContainer.SetActive(false);
        levelsContainer.SetActive(true);
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
